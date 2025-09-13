// See https://aka.ms/new-console-template for more information

using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

Console.WriteLine("Starting application...");

string keyVaultUrl = "https://tgckvhadev.vault.azure.net/";
string secretName = "test-value";

var options = new DefaultAzureCredentialOptions
{
	ManagedIdentityClientId = "19ec7ba5-8986-4d46-84a3-c27524ceb2b6"
};

var client = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential(options));

KeyVaultSecret secret = await client.GetSecretAsync(secretName);

Console.WriteLine($"Secret value from KV via MI: {secret.Value}");

string envSecret = Environment.GetEnvironmentVariable("MY_SECRET_ENV_VAR");

if (string.IsNullOrEmpty(envSecret))
{
	Console.WriteLine("Could not find CSI key.");	
}

Console.WriteLine($"Secret value from KV via CSI: {envSecret}");