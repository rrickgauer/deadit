import { ApiResponseBase, ServiceResponse } from "../domain/model/api-response";

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


    static async toServiceResponse<T>(response: Response) {

        const data: ApiResponseBase<T> = await response.json();
        const result = new ServiceResponse<T>(data, response.ok);

        return result;
    }


}