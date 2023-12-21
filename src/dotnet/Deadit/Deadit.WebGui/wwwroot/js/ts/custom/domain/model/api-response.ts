
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
