data "azurerm_resource_group" "default_resource_group" {
  name = var.resource_group_name
}

data "azurerm_key_vault" "shared_keyvault" {
  name                = var.keyvault_name
  resource_group_name = var.resource_group_name
}