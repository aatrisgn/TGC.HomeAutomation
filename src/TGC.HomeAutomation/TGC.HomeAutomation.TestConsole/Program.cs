// See https://aka.ms/new-console-template for more information

using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

Console.WriteLine("Starting application...");


string keyVaultUrl = "https://tgckvhadev.vault.azure.net/";
string secretName = "test-value";

var client = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());

KeyVaultSecret secret = await client.GetSecretAsync(secretName);

Console.WriteLine($"Secret value: {secret.Value}");