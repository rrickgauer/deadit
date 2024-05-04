


export class ApplicationTypes
{
    public static readonly JSON = 'application/json';


    public static GetJsonHeaders = () =>
    {
        const headers = {
            "Content-Type": ApplicationTypes.JSON,
        };
        return headers;
    }
}