

export class UrlUtility
{
    public static getCurrentPathValue = (index: number): string =>
    {
        const url = new URL(window.location.href);
        return UrlUtility.getPathValue(index, url);
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


    public static getQueryParmsString(data: object): string
    {
        const urlParms = new URLSearchParams();

        for (const key in data)
        {
            urlParms.set(key, `${data[key]}`);
        }

        return urlParms.toString();
    }

}