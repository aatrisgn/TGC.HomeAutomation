data "azurerm_resource_group" "default_resource_group" {
  name = var.resource_group_name
}

data "azurerm_key_vault_secret" "openweather_api_key" {
  name         = "openweatherapikey"
  key_vault_id = azurerm_key_vault.shared_keyvault.id
}

data "azurerm_client_config" "current" {}