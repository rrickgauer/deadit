import { ApiEndpoints, HttpMethods } from "../domain/constants/api-constants";


export class ApiCommunity
{
    private readonly _url: string = ApiEndpoints.COMMUNITY;

    post = async (newCommunityFormData: FormData) : Promise<Response> =>
    {
        const url = this._url;

        return await fetch(url, {
            method: HttpMethods.POST,
            body: newCommunityFormData,
        });
    }
}