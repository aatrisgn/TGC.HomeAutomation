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

resource "azuread_application_registration" "runner_app_registration" {
  for_each = var.raspberries

  display_name = "tgc-homeautomation-${each.key}-${lower(var.environment)}"
}

resource "azuread_service_principal" "runner_enterprise_application" {
  for_each = var.raspberries

  client_id = azuread_application_registration.runner_app_registration[each.key].client_id
}

resource "azuread_application_password" "runner_secret" {
  for_each = var.raspberries

  application_id = azuread_application_registration.runner_app_registration[each.key].id
}

resource "azurerm_key_vault_secret" "runner_client_id" {
  for_each = var.raspberries

  name         = "${each.key}ClientId"
  key_vault_id = data.azurerm_key_vault.shared_keyvault.id
  value        = azuread_application_registration.runner_app_registration[each.key].client_id
}

resource "azurerm_key_vault_secret" "runner_object_id" {
  for_each = var.raspberries

  name         = "${each.key}ObjectId"
  key_vault_id = data.azurerm_key_vault.shared_keyvault.id
  value        = azuread_service_principal.runner_enterprise_application[each.key].object_id
}

resource "azurerm_key_vault_secret" "runner_secret" {
  for_each = var.raspberries

  name         = "${each.key}Password"
  key_vault_id = data.azurerm_key_vault.shared_keyvault.id
  value        = azuread_application_password.runner_secret[each.key].value
}

resource "azurerm_role_assignment" "table_storage_contributor" {
  for_each = var.raspberries

  scope                = data.azurerm_storage_account.homeautomation_storage_account.id
  role_definition_name = "Storage Table Data Contributor"
  principal_id         = azuread_service_principal.runner_enterprise_application[each.key].object_id
}

resource "azurerm_role_assignment" "storage_account_reader" {
  for_each = var.raspberries

  scope                = data.azurerm_storage_account.homeautomation_storage_account.id
  role_definition_name = "Reader"
  principal_id         = azuread_service_principal.runner_enterprise_application[each.key].object_id
}

