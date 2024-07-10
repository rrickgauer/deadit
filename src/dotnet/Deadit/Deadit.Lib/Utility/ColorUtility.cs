using System.Drawing;

namespace Deadit.Lib.Utility;

public static class ColorUtility
{
    public static string GetForegroundColor(string backgroundColorHex)
    {
        var color = ColorTranslator.FromHtml(backgroundColorHex);

        var lum = color.GetLuminance();

        var foregroundColor = lum > 0.179 ? Color.Black : Color.White;

        var hexResult = ColorTranslator.ToHtml(foregroundColor);

        return hexResult;
    }

    public static double GetLuminance(this Color color)
    {
        return GetLuminance(color.R, color.G,color.B);
    }

    private static double GetLuminance(int r, int g, int b)
    {
        double rLinear = r / 255.0;
        double gLinear = g / 255.0;
        double bLinear = b / 255.0;

        rLinear = rLinear <= 0.03928 ? rLinear / 12.92 : Math.Pow((rLinear + 0.055) / 1.055, 2.4);
        gLinear = gLinear <= 0.03928 ? gLinear / 12.92 : Math.Pow((gLinear + 0.055) / 1.055, 2.4);
        bLinear = bLinear <= 0.03928 ? bLinear / 12.92 : Math.Pow((bLinear + 0.055) / 1.055, 2.4);

        return 0.2126 * rLinear + 0.7152 * gLinear + 0.0722 * bLinear;
    }
}
