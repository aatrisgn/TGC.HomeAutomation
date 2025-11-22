namespace TGC.HomeAutomation.API.Configuration;

public class HomeAutomationConfiguration
{
	public const string SectionName = "HomeAutomation";

	public TestingConfiguration Testing { get; set; } = new();
	public AuthConfiguration Auth { get; set; } = new();
	public string[] AllowedHosts { get; set; } = [];
	public Coordinates Coordinates { get; set; } = new();
}

public class Coordinates
{
	public string Longitude { get; set; } = string.Empty;
	public string Latitude { get; set; } = string.Empty;
}
