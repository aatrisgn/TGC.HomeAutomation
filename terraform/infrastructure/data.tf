data "azurerm_resource_group" "default_resource_group" {
  name = var.resource_group_name
}

data "azurerm_client_config" "current" {}

data "azurerm_log_analytics_workspace" "shared_log_analytic_workspace" {
  name                = "law-logging-shared-${var.environment}-westeurope"
  resource_group_name = "rg-logging-shared-${var.environment}-westeurope"
}
