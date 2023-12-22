
export class ErrorMessage
{
    public id?: number;
    public message?: string;
}


export class ApiResponseBase<T>
{
    public errors: ErrorMessage[] = [];
    public data?: T = null;
}


export class ServiceResponse<T>
{
    public response: ApiResponseBase<T>;
    public successful: boolean;

    constructor(response: ApiResponseBase<T>, successful: boolean=true) {
        this.response = response;
        this.successful = successful;
    }

    
}
