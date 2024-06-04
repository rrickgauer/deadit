import { ValidationErrorsApiResponse } from "./api-response";


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

