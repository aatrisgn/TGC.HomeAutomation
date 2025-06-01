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

resource "azurerm_static_web_app" "frontend_app" {
  name                = "swa-homeautomation-${var.environment}-weu"
  location            = data.azurerm_resource_group.default_resource_group.location
  resource_group_name = data.azurerm_resource_group.default_resource_group.name
}

######################################
# Web App authentication application #
######################################
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
    "https://homeautomation.dev.tgcportal.com/auth"
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
