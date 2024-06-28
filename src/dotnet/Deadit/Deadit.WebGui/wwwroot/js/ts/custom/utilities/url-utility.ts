import { Nullable } from "./nullable";


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

    public static getQueryParmValueNumber(key: string): number | null
    {
        const stringValue = this.getQueryParmValue(key);

        if (!Nullable.hasValue(stringValue))
        {
            return null;
        }

        return parseInt(stringValue);
    }


    public static getQueryParmValue(key: string): string | null
    {
        const currentUrl = new URL(window.location.href);
        return currentUrl.searchParams.get(key);
    }

    public static getQueryParmValueStringTyped<T>(key: string): T | null
    {
        const currentUrl = new URL(window.location.href);
        return currentUrl.searchParams.get(key) as T;
    }


    
}