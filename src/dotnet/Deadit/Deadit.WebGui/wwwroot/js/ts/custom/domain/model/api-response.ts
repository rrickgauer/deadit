
export class ErrorMessage
{
    public id?: number;
    public message?: string;
}

export interface IApiErrors
{
    errors: ErrorMessage[];
}

export class ApiResponse<T> implements IApiErrors
{
    public errors: ErrorMessage[] = [];
    public data?: T = null;
}

export class ApiResponseNoContent implements IApiErrors
{
    public errors: ErrorMessage[] = [];
    public readonly response: Response;

    constructor(response: Response)
    {
        this.response = response;
    }

    public toServiceResponse = async () =>
    {
        let apiResponse = new ApiResponse<any>();

        if (!this.response.ok)
        {
            apiResponse = await this.response.json() as ApiResponse<any>;
        }

        return new ServiceResponse<any>(apiResponse, this.response.ok);
    }
}

export class ServiceResponse<T>
{
    public response: ApiResponse<T>;
    public successful: boolean;

    constructor(response: ApiResponse<T>, successful: boolean=true) {
        this.response = response;
        this.successful = successful;
    }
}



export type ValidationErrorsApiResponse = Map<string, string[]>;


export class ApiValidationException extends Error
{
    public errors: ValidationErrorsApiResponse;

    constructor(apiResponse: ValidationErrorsApiResponse, message?: string)
    {
        super(message);
        this.errors = apiResponse;
    }
}

export class ApiNotFoundException extends Error
{
    constructor(message?: string)
    {
        super(message);
    }
}

