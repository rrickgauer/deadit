import { ApiEndpoints, HttpMethods } from "../domain/constants/api-constants";
import { ApplicationTypes } from "../domain/constants/application-types";
import { UpdateCommunityApiRequest } from "../domain/model/api-community-models";
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

    public async put(communityName: string, data: UpdateCommunityApiRequest)
    {
        const url = `${this._url}/${communityName}`;

        return await fetch(url, {
            method: HttpMethods.PUT,
            body: JSON.stringify(data),
            headers: ApplicationTypes.GetJsonHeaders(),
        });
    }
}