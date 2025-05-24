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

resource "azuread_application_registration" "web_auth_app_registration" {
    for_each = var.raspberries
    
    display_name     = "tgc-homeautomation-${each.key}-${lower(var.environment)}"
}

resource "azuread_service_principal" "web_auth_enterprise_application" {
    for_each = var.raspberries

    client_id = azuread_application_registration.web_auth_app_registration[each.key].client_id
}

resource "azuread_application_password" "runner_secret" {
    for_each = var.raspberries

    application_id = azuread_application_registration.web_auth_app_registration[each.key].id
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