

export class UrlUtilities
{
    public static getCurrentPathValue = (index: number): string =>
    {
        const url = new URL(window.location.href);
        return UrlUtilities.getPathValue(index, url);
    }

    public static getPathValue = (index: number, url: URL): string =>
    {
        const pathValues = url.pathname.split('/').filter(v => v !== "");

        if (index > pathValues.length)
        {
            return null;
        }

        return pathValues[index];
    }
}