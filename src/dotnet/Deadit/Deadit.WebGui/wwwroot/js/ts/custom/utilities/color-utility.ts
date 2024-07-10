


export class ColorUtility
{

    public static getTextColorBasedOnBackground(bgColor: string) 
    {

        let rgb = ColorUtility.getRGBFromHex(bgColor);
        let luminance = ColorUtility.getLuminance(rgb[0], rgb[1], rgb[2]);
        let textColor = luminance > 0.179 ? 'black' : 'white';
        return textColor;
    }

    private static getRGBFromHex(hex)
    {
        hex = hex.replace('#', '');
        let bigint = parseInt(hex, 16);
        let r = (bigint >> 16) & 255;
        let g = (bigint >> 8) & 255;
        let b = bigint & 255;
        return [r, g, b];
    }

    private static getLuminance(r, g, b) 
    {
        // Convert RGB to a value between 0 and 1
        r = r / 255;
        g = g / 255;
        b = b / 255;

        // Apply the gamma correction
        r = r <= 0.03928 ? r / 12.92 : Math.pow((r + 0.055) / 1.055, 2.4);
        g = g <= 0.03928 ? g / 12.92 : Math.pow((g + 0.055) / 1.055, 2.4);
        b = b <= 0.03928 ? b / 12.92 : Math.pow((b + 0.055) / 1.055, 2.4);

        // Calculate luminance
        return 0.2126 * r + 0.7152 * g + 0.0722 * b;
    }




}


