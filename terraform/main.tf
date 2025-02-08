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

resource "azurerm_storage_account" "ha_storage_account" {
  name = "tgcststate${each.key}"

  location            = azurerm_resource_group.state_file_resource_group[each.key].location
  resource_group_name = azurerm_resource_group.state_file_resource_group[each.key].name

  account_tier             = "Standard"
  account_replication_type = "LRS"

  tags = {
    "provision" = "homeautomation"
  }
}

resource "azuread_application" "rasperry_spn_app_registration" {
  display_name = "tgc-homeautomation-raspberry-spn"
}

resource "azuread_service_principal" "rasperry_spn_enterprise_application" {
  client_id = azuread_application.rasperry_service_principal.client_id
}

resource "azuread_application_password" "rasperry_spn_secret" {
  application_id = azuread_application.rasperry_service_principal.id
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
