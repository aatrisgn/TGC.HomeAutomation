namespace TGC.HomeAutomation.API.Configuration;

public class HomeAutomationConfiguration
{
	public const string SectionName = "HomeAutomation";

	public TestingConfiguration Testing { get; set; } = new TestingConfiguration();
	public AuthConfiguration Auth { get; set; } = new AuthConfiguration();
	public string AllowedHosts { get; set; } = string.Empty;
}
