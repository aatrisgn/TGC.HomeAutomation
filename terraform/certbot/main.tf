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

# Virtual Network
resource "azurerm_virtual_network" "vnet" {
  name                = "vnet-homeautomation-certbot-${var.environment}-weu"
  address_space       = ["10.0.0.0/16"]
  location            = data.azurerm_resource_group.default_resource_group.location
  resource_group_name = data.azurerm_resource_group.default_resource_group.name
}

# Subnet
resource "azurerm_subnet" "subnet" {
  name                 = "vm-subnet"
  resource_group_name  = data.azurerm_resource_group.default_resource_group.name
  virtual_network_name = azurerm_virtual_network.vnet.name
  address_prefixes     = ["10.0.1.0/24"]
}

# Public IP
resource "azurerm_public_ip" "public_ip" {
  name                = "pip-homeautomation-certbot-${var.environment}-we"
  location            = data.azurerm_resource_group.default_resource_group.location
  resource_group_name = data.azurerm_resource_group.default_resource_group.name
  allocation_method   = "Dynamic"
}

# Network Interface
resource "azurerm_network_interface" "nic" {
  name                = "nic-homeautomation-certbot-${var.environment}-weu"
  location            = data.azurerm_resource_group.default_resource_group.location
  resource_group_name = data.azurerm_resource_group.default_resource_group.name

  ip_configuration {
    name                          = "internal"
    subnet_id                     = azurerm_subnet.subnet.id
    private_ip_address_allocation = "Dynamic"
    public_ip_address_id          = azurerm_public_ip.public_ip.id
  }
}

# Network Security Group
resource "azurerm_network_security_group" "nsg" {
  name                = "nsg-homeautomation-certbot-${var.environment}-weu"
  location            = data.azurerm_resource_group.default_resource_group.location
  resource_group_name = data.azurerm_resource_group.default_resource_group.name

  security_rule {
    name                       = "SSH"
    priority                   = 1001
    direction                  = "Inbound"
    access                     = "Allow"
    protocol                   = "Tcp"
    source_port_range          = "*"
    destination_port_range     = "22"
    source_address_prefix      = "*"
    destination_address_prefix = "*"
  }

  security_rule {
    name                       = "HTTP"
    priority                   = 1002
    direction                  = "Inbound"
    access                     = "Allow"
    protocol                   = "Tcp"
    source_port_range          = "*"
    destination_port_range     = "80"
    source_address_prefix      = "*"
    destination_address_prefix = "*"
  }
}

# Associate NSG with NIC
resource "azurerm_network_interface_security_group_association" "nsg_assoc" {
  network_interface_id      = azurerm_network_interface.nic.id
  network_security_group_id = azurerm_network_security_group.nsg.id
}

resource "random_password" "admin_password" {
  length           = 20
  special         = true
  upper           = true
  lower           = true
  numeric         = true
}

# Store secrets
resource "azurerm_key_vault_secret" "admin_username" {
  name         = "CertbotAdminUsername"
  value        = "certbotadmin"
  key_vault_id = data.azurerm_key_vault.shared_keyvault.id
}

resource "azurerm_key_vault_secret" "admin_password" {
  name         = "admin-password"
  value        = random_password.admin_password
  key_vault_id = data.azurerm_key_vault.shared_keyvault.id
}

# Virtual Machine
resource "azurerm_linux_virtual_machine" "vm" {
  name                = "vm-homeautomation-certbot-${var.environment}-weu"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  size                = "Standard_B1s"
  admin_username      = data.azurerm_key_vault_secret.username.value
  admin_password      = data.azurerm_key_vault_secret.password.value
  disable_password_authentication = false

  network_interface_ids = [
    azurerm_network_interface.nic.id,
  ]

  os_disk {
    caching              = "ReadWrite"
    storage_account_type = "Standard_LRS"
    name                 = "dsc-homeautomation-certbot-${var.environment}-weu"
  }

  source_image_reference {
    publisher = "Canonical"
    offer     = "0001-com-ubuntu-server-focal"
    sku       = "22_04-lts"
    version   = "latest"
  }
}
