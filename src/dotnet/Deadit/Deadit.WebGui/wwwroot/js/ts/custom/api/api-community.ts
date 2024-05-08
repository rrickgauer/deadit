import { ApiEndpoints, HttpMethods } from "../domain/constants/api-constants";
import { ApplicationTypes } from "../domain/constants/application-types";
import { JsonObject } from "../domain/types/aliases";


export class ApiCommunity
{
    private readonly _url: string = ApiEndpoints.COMMUNITY;

    public post = async (newCommunityFormData: JsonObject) : Promise<Response> =>
    {
        const url = this._url;

        return await fetch(url, {
            method: HttpMethods.POST,
            body: newCommunityFormData,
            headers: ApplicationTypes.GetJsonHeaders(),
        });
    }
}