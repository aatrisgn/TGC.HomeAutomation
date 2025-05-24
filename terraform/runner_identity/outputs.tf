output "secret_names" {
    value = [for s in azurerm_key_vault_secret.runner_secret : s.name]
}