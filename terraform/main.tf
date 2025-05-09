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
      version = "=4.0.0"
    }
  }
}

########################
# Azure infrastructure #
########################

resource "azurerm_storage_account" "ha_storage_account" {
  name = "tgcstha${var.environment}"

  location            = data.azurerm_resource_group.default_resource_group.location
  resource_group_name = data.azurerm_resource_group.default_resource_group.name

  account_tier             = "Standard"
  account_replication_type = "LRS"

  network_rules {
    default_action = "Deny"
    virtual_network_subnet_ids = [ azurerm_subnet.applictions.id, azurerm_subnet.storage.id ]
  }

  tags = {
    "provision" = "homeautomation"
  }
}

resource "azurerm_role_assignment" "table_storage_contributor" {
  scope                = azurerm_storage_account.ha_storage_account.id
  role_definition_name = "Storage Table Data Contributor"
  principal_id         = azuread_service_principal.rasperry_spn_enterprise_application.object_id
}

resource "azurerm_role_assignment" "storage_account_reader" {
  scope                = azurerm_storage_account.ha_storage_account.id
  role_definition_name = "Reader"
  principal_id         = azuread_service_principal.rasperry_spn_enterprise_application.object_id
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

resource "azurerm_virtual_network" "primary_virtual_network" {
  name = "vnet-homeautomation-${var.environment}-weu"
  location            = data.azurerm_resource_group.default_resource_group.location
  resource_group_name = data.azurerm_resource_group.default_resource_group.name
  address_space = ["10.0.0.0/16"]
}

resource "azurerm_subnet" "applictions" {
  name = "applications"
  address_prefixes = ["10.0.1.0/28"]
  virtual_network_name = azurerm_virtual_network.primary_virtual_network.name
  resource_group_name = data.azurerm_resource_group.default_resource_group.name
}

resource "azurerm_subnet" "storage" {
  name = "storage"
  address_prefixes = ["10.0.1.16/28"]
  virtual_network_name = azurerm_virtual_network.primary_virtual_network.name
  resource_group_name = data.azurerm_resource_group.default_resource_group.name
}

#########################################
# Pseudo managed identity for Raspberry #
#########################################
resource "azuread_application_registration" "rasperry_spn_app_registration" {
  display_name = "tgc-homeautomation-raspberry-spn"
}

resource "azuread_service_principal" "rasperry_spn_enterprise_application" {
  client_id = azuread_application_registration.rasperry_spn_app_registration.client_id
}

resource "azuread_application_password" "rasperry_spn_secret" {
  application_id = azuread_application_registration.rasperry_spn_app_registration.id
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
    "http://localhost:4200"
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