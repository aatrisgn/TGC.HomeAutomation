using System.Security.Cryptography;

namespace TGC.HomeAutomation.API.Device;

public class DeviceAPIKeyGenerator : IDeviceAPIKeyGenerator
{
	public Task<string> GenerateDeviceAPIKey()
	{
		byte[] randomBytes = new byte[32];
		using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
		{
			rng.GetBytes(randomBytes);
		}

		// Compute the hash value
		using SHA256 sha256 = SHA256.Create();
		byte[] hashBytes = sha256.ComputeHash(randomBytes);
		string hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();

		return Task.FromResult(hashString);
	}

	public Task<MaskedApiKey> MaskApiKey(string apiKey)
	{
		byte[] salt = GenerateSalt();
		byte[] hashedString = HashStringWithSalt(apiKey, salt);
		var maskedSecret = Convert.ToBase64String(hashedString);

		var result = new MaskedApiKey { Secret = maskedSecret, Salt = salt };
		return Task.FromResult(result);
	}

	public Task<MaskedApiKey> MaskApiKey(string apiKey, byte[] salt)
	{
		byte[] hashedString = HashStringWithSalt(apiKey, salt);
		var maskedSecret = Convert.ToBase64String(hashedString);

		var result = new MaskedApiKey { Secret = maskedSecret, Salt = salt };
		return Task.FromResult(result);
	}

	private static byte[] GenerateSalt(int size = 32)
	{
		byte[] salt = new byte[size];
		using RandomNumberGenerator rng = RandomNumberGenerator.Create();
		rng.GetBytes(salt);
		return salt;
	}

	private static byte[] HashStringWithSalt(string input, byte[] salt, int iterations = 10000)
	{
		using var pbkdf2 = new Rfc2898DeriveBytes(input, salt, iterations);
		return pbkdf2.GetBytes(32);
	}
}
