terraform {
  backend "azurerm" {
    use_azuread_auth = true
    use_oidc         = true
  }

  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "=4.30.0"
    }
  }
}

resource "random_password" "admin_password" {
  length  = 20
  special = true
  upper   = true
  lower   = true
  numeric = true
}

# Store secrets
resource "azurerm_key_vault_secret" "pihole_admin_username" {
  name         = "PiholeAdminPassword"
  value        = random_password.admin_password
  key_vault_id = data.azurerm_key_vault.shared_keyvault.id
}
