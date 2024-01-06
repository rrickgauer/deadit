
export class ErrorMessage
{
    public id?: number;
    public message?: string;
}


export class ApiResponse<T>
{
    public errors: ErrorMessage[] = [];
    public data?: T = null;
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

