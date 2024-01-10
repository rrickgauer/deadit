import { HttpStatusCode } from "../domain/enum/http-status-code";
import { ApiResponse, ApiResponseNoContent, ApiValidationException, ServiceResponse, ValidationErrorsApiResponse } from "../domain/model/api-response";

export class ServiceUtilities
{
    public static handleBadResponse = async (response: Response): Promise<void> =>
    {
        if (!response.ok)
        {
            const text = await response.text();
            throw new Error(text);
        }
    }

    public static async toServiceResponseNoContent(response: Response): Promise<ServiceResponse<any>>
    {
        if (response.status === HttpStatusCode.UnprocessableEntity)
        {
            await ServiceUtilities.handleUnprocessableEntityApiResponse(response);
        }

        const apiResponse = new ApiResponseNoContent(response);
        return await apiResponse.toServiceResponse();
    }


    public static async toServiceResponse<T>(response: Response) 
    {
        if (response.status === HttpStatusCode.UnprocessableEntity)
        {
            await ServiceUtilities.handleUnprocessableEntityApiResponse(response);
        }

        const data: ApiResponse<T> = await response.json();
        const result = new ServiceResponse<T>(data, response.ok);

        return result;
    }

    private static handleUnprocessableEntityApiResponse = async (response: Response) =>
    {
        const validationErrors: ValidationErrorsApiResponse = await response.json();
        throw new ApiValidationException(validationErrors);
    }

}



