
provider "azurerm" {
  use_oidc = true
  features {}
}

provider "random" {}