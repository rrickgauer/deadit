


export class Nullable
{

    public static nullerize(data: any)
    {
        const keys = Object.keys(data);

        for (const key of keys)
        {
            data[key] = Nullable.getValue(data[key]);
        }

        return data;
    }

    public static getValue<T>(data: T, defaultValue: T | null = null): T | null
    {
        return (Nullable.hasValue(data) ? data : defaultValue);
    }


    public static hasValue = (data: any) =>
    {
        if (data === null)
        {
            return false;
        }
        else if (typeof data === 'undefined')
        {
            return false;
        }
        else if (data === '')
        {
            return false;
        }

        return true;
    }

}

