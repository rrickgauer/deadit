export class ServiceUtilities
{
    static handleBadResponse = async (response: Response): Promise<void> =>
    {
        if (!response.ok)
        {
            const text = await response.text();
            throw new Error(text);
        }
    }
}