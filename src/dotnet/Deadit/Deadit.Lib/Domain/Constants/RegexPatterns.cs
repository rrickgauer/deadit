namespace Deadit.Lib.Domain.Constants;

public class RegexPatterns
{
    public const string UrlCharactersOnly = @"^[a-zA-Z0-9_]*$";
    public const string HexColor = @"^#([A-Fa-f0-9]{6})$";
}
