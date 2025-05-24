variable "resource_group_name" {
  description = "Azure resource group to deploy resources into"
  type        = string
}

variable "tenant_id" {
  description = "Azure resource group to deploy resources into"
  type        = string
}

variable "keyvault_name" {
  description = "Shared Homeautomation keyvault for credential storage"
  type        = string
}

variable "storage_account_name" {
  description = "Storage account for storing data"
  type        = string
}

variable "environment" {
  description = "environment type"
  type        = string

  validation {
    condition     = contains(["dev", "prod"], var.environment)
    error_message = "The environment variable must be either 'DEV' or 'PROD'."
  }
}

variable "raspberries" {
  type = [string]
}