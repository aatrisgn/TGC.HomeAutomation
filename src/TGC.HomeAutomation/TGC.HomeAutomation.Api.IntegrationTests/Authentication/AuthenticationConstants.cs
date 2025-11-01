namespace TGC.HomeAutomation.Api.IntegrationTests.Authentication;

internal class AuthenticationConstants
{
	public const string WrongAudience = "6E5DFBDB-D264-46B2-9BB7-13837AF03288";
	public const string WrongIssuer = "https://login.microsoftonline.com/D08FD8DD-9C3B-4E34-B330-110D08FFAB3A/v2.0";
	public const string WrongSigningKey = "WowPleaseDontStealThisKeyUncool!";

	public const string Audience = "79AA60B2-2BC5-4885-A003-B4A324FC8E45";
	public const string Issuer = "https://login.microsoftonline.com/8CE14250-9EC1-447D-A087-6D92C2F52261/v2.0";
	public const string SigningKey = "PleaseDontStealThisEitherLolPlzz";
}
