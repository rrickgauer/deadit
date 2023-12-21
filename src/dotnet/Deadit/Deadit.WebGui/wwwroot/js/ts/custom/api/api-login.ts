import { ApiEndpoints, HttpMethods } from "../domain/constants/api-constants"



export class ApiLogin
{
    private readonly _url: string  = ApiEndpoints.LOGIN;

    post = async (credentials: FormData) => {
        const url = this._url;

        return await fetch(url, {
            method: HttpMethods.POST,
            body: credentials,
        });
    }
}


