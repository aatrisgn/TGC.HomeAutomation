terraform {
  backend "azurerm" {
    use_azuread_auth = true
    use_oidc         = true
  }

  required_providers {
    azuread = {
      source  = "hashicorp/azuread"
      version = "~> 3.0.2"
    }
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "=4.30.0"
    }

    random = {
      source  = "hashicorp/random"
      version = "= 3.7.2"
    }
  }
}

########################
# Azure infrastructure #
########################

resource "azurerm_key_vault" "shared_keyvault" {
  name                      = "tgckvha${var.environment}"
  location                  = data.azurerm_resource_group.default_resource_group.location
  resource_group_name       = data.azurerm_resource_group.default_resource_group.name
  sku_name                  = "standard"
  tenant_id                 = var.tenant_id
  enable_rbac_authorization = true
}

resource "azurerm_role_assignment" "spn_kv_access" {
  scope                = azurerm_key_vault.shared_keyvault.id
  role_definition_name = "Key Vault Secrets Officer" # or "Key Vault Administrator"
  principal_id         = data.azurerm_client_config.current.object_id
}

resource "azurerm_storage_account" "ha_storage_account" {
  name = "tgcstha${var.environment}"

  location            = data.azurerm_resource_group.default_resource_group.location
  resource_group_name = data.azurerm_resource_group.default_resource_group.name

  account_tier             = "Standard"
  account_replication_type = "LRS"

  tags = {
    "provision" = "homeautomation"
  }
}

resource "azurerm_application_insights" "application_insights" {
  location            = data.azurerm_resource_group.default_resource_group.location
  resource_group_name = data.azurerm_resource_group.default_resource_group.name
  name                = "ai-homeautomation-${var.environment}-weu" #Should change WEU to use location and translate
  application_type    = "web"
}

resource "azurerm_key_vault_secret" "storage_account_table_url" {
  key_vault_id = azurerm_key_vault.shared_keyvault.id
  name         = "storage-account-table-url"
  value        = sensitive(azurerm_storage_account.ha_storage_account.primary_table_endpoint)
}

resource "azurerm_key_vault_secret" "ai_connectionkey" {
  key_vault_id = azurerm_key_vault.shared_keyvault.id
  name         = "application-insights-connection-string"
  value        = sensitive(azurerm_application_insights.application_insights.connection_string)
}

resource "azurerm_user_assigned_identity" "k8_uaid" {
  resource_group_name = data.azurerm_resource_group.default_resource_group.name
  location            = data.azurerm_resource_group.default_resource_group.location
  name                = "mi-homeautomation-k8-${var.environment}-weu"
}

resource "azurerm_role_assignment" "table_storage_contributor" {
  scope                = azurerm_storage_account.ha_storage_account.id
  role_definition_name = "Storage Table Data Contributor"
  principal_id         = azurerm_user_assigned_identity.k8_uaid.principal_id
}

resource "azurerm_role_assignment" "storage_account_reader" {
  scope                = azurerm_storage_account.ha_storage_account.id
  role_definition_name = "Reader"
  principal_id         = azurerm_user_assigned_identity.k8_uaid.principal_id
}

resource "azurerm_role_assignment" "uaid_secret_reader_ai_connectionkey" {
  scope                = azurerm_key_vault_secret.ai_connectionkey.resource_versionless_id
  role_definition_name = "Key Vault Secrets User" # or "Key Vault Administrator"
  principal_id         = azurerm_user_assigned_identity.k8_uaid.principal_id
}

resource "azurerm_role_assignment" "uaid_secret_reader_storage_account_table_url" {
  scope                = azurerm_key_vault_secret.storage_account_table_url.resource_versionless_id
  role_definition_name = "Key Vault Secrets User" # or "Key Vault Administrator"
  principal_id         = azurerm_user_assigned_identity.k8_uaid.principal_id
}

# Use this when we know how it'll work
resource "azurerm_federated_identity_credential" "api-server-credentials" {
  parent_id           = azurerm_user_assigned_identity.k8_uaid.id
  name                = "homeautomation-api-credentials"
  resource_group_name = data.azurerm_resource_group.default_resource_group.name
  audience            = ["api://AzureADTokenExchange"]
  issuer              = "https://westeurope.oic.prod-aks.azure.com/0552bf76-d799-4ea1-8459-74106c5d04aa/0b3bcfef-ec19-488d-afad-7d89bba3c65a/"
  subject             = "system:serviceaccount:homeautomation:api-server"
}

######################################
# Web App authentication application #
######################################

resource "random_uuid" "api_read_id" {
}

resource "random_uuid" "api_manage_id" {
}

resource "random_uuid" "api_signalr" {
}

resource "azuread_application" "api_auth_app_registration" {
  display_name    = "tgc-homeautomation-api-auth-${lower(var.environment)}"
  identifier_uris = ["api://tgc-homeautomation-${lower(var.environment)}"]
  api {
    requested_access_token_version = 2

    oauth2_permission_scope {
      admin_consent_description  = "Allow the app to read data"
      admin_consent_display_name = "Read data"
      id                         = random_uuid.api_read_id.result
      type                       = "User"
      value                      = local.api_read_value
      enabled                    = true
    }

    oauth2_permission_scope {
      admin_consent_description  = "Allow the app to manage data"
      admin_consent_display_name = "Manage data"
      id                         = random_uuid.api_manage_id.result
      type                       = "User"
      value                      = local.api_manage_value
      enabled                    = true
    }

    oauth2_permission_scope {
      admin_consent_description  = "Allows the app to communicate via SignalR"
      admin_consent_display_name = "Communicate via SignalR"
      id                         = random_uuid.api_signalr.result
      type                       = "User"
      value                      = local.api_signalr_value
      enabled                    = true
    }
  }
}

resource "azuread_service_principal" "api_auth_enterprise_application" {
  client_id = azuread_application.api_auth_app_registration.client_id
}

resource "azuread_application_registration" "web_auth_app_registration" {
  display_name     = "tgc-homeautomation-web-auth-${lower(var.environment)}"
  sign_in_audience = "AzureADMyOrg"
}


resource "azuread_service_principal" "web_auth_enterprise_application" {
  client_id = azuread_application_registration.web_auth_app_registration.client_id
}

resource "azuread_application_redirect_uris" "example_spa" {
  application_id = azuread_application_registration.web_auth_app_registration.id
  type           = "SPA"

  redirect_uris = [
    "http://localhost:4400",
    "http://localhost:4300",
    "http://localhost:4200",
    "https://homeautomation.dev.tgcportal.com",
    "https://homeautomation.dev.tgcportal.com/auth",
    "https://homeautomation.tgcportal.com",
    "https://homeautomation.tgcportal.com/auth",
    "https://app.homeautomation.dev.tgcportal.com",
    "https://app.homeautomation.dev.tgcportal.com/auth"
  ]
}

data "azuread_application_published_app_ids" "well_known" {}

data "azuread_service_principal" "msgraph" {
  client_id = data.azuread_application_published_app_ids.well_known.result["MicrosoftGraph"]
}

resource "azuread_application_api_access" "example_msgraph" {
  application_id = azuread_application_registration.web_auth_app_registration.id
  api_client_id  = data.azuread_application_published_app_ids.well_known.result["MicrosoftGraph"]

  scope_ids = [
    data.azuread_service_principal.msgraph.oauth2_permission_scope_ids["User.ReadWrite"],
  ]
}

resource "azuread_application_api_access" "api_scope_permissions" {
  application_id = azuread_application_registration.web_auth_app_registration.id
  api_client_id  = azuread_application.api_auth_app_registration.client_id

  scope_ids = [
    azuread_application.api_auth_app_registration.oauth2_permission_scope_ids[local.api_read_value],
    azuread_application.api_auth_app_registration.oauth2_permission_scope_ids[local.api_manage_value],
    azuread_application.api_auth_app_registration.oauth2_permission_scope_ids[local.api_signalr_value],
  ]
}
