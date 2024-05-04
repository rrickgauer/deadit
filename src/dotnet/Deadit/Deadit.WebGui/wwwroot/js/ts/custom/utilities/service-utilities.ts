import { HttpStatusCode } from "../domain/enum/http-status-code";
import { ApiNotFoundException, ApiResponse, ApiResponseNoContent, ApiValidationException, ServiceResponse, ValidationErrorsApiResponse } from "../domain/model/api-response";



export class ServiceUtilities
{
    public static async toServiceResponseNoContent(response: Response): Promise<ServiceResponse<any>>
    {
        if (response.ok || response.status === HttpStatusCode.BadRequest)
        {
            const apiResponse = new ApiResponseNoContent(response);
            return await apiResponse.toServiceResponse();
        }
        else if (response.status === HttpStatusCode.UnprocessableEntity)
        {
            await ServiceUtilities.handleUnprocessableEntityApiResponse(response);
        }
        else if (response.status === HttpStatusCode.NotFound)
        {
            ServiceUtilities.handleNotFoundApiResponse(response);
        }

        const responseText = await response.text();
        throw new Error(responseText);
    }


    public static async toServiceResponse<T>(response: Response) 
    {
        if (response.ok || response.status === HttpStatusCode.BadRequest)
        {
            const data: ApiResponse<T> = await response.json();
            const result = new ServiceResponse<T>(data, response.ok);

            return result;
        }
        else if (response.status === HttpStatusCode.UnprocessableEntity)
        {
            await ServiceUtilities.handleUnprocessableEntityApiResponse(response);
        }
        else if (response.status === HttpStatusCode.NotFound)
        {
            ServiceUtilities.handleNotFoundApiResponse(response);
        }

        const responseText = await response.text();
        throw new Error(responseText);
    }

    private static handleUnprocessableEntityApiResponse = async (response: Response) =>
    {
        const validationErrors: ValidationErrorsApiResponse = await response.json();
        throw new ApiValidationException(validationErrors);
    }

    private static handleNotFoundApiResponse = (response: Response) =>
    {
        throw new ApiNotFoundException();
    }

}







