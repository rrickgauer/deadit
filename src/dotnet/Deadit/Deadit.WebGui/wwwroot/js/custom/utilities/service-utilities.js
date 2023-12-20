export class ServiceUtilities
{
    /**
     * Handle a bad response
     * @param {Response} response The response to handle
     */
    static handleBadResponse = async (response) =>
    {
        if (!response.ok)
        {
            const text = await response.text();
            throw new Error(text);
        }
    }
}