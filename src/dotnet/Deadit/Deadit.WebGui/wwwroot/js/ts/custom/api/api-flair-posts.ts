import { ApiEndpoints, HttpMethods } from "../domain/constants/api-constants";
import { FlairPostApiRequestBody } from "../domain/model/flair-models";
import { ApiUtility } from "./api-base";



export class ApiFlairPosts 
{
    private readonly _url: string = ApiEndpoints.FLAIR_POSTS;

    public async getAll(communityName: string) 
    {
        const url = `${this._url}/${communityName}`;

        return await fetch(url);
    }

    public async get(flairPostId: number)
    {
        const url = `${this._url}/${flairPostId}`;

        return await fetch(url);
    }

    public async post(flairPostData: FlairPostApiRequestBody)
    {
        const url = `${this._url}`;

        return await ApiUtility.fetchJson(url, {
            body: JSON.stringify(flairPostData),
            method: HttpMethods.POST,
        });
    }

    public async put(flairId: number, flairPostData: FlairPostApiRequestBody)
    {
        const url = `${this._url}/${flairId}`;

        return await ApiUtility.fetchJson(url, {
            body: JSON.stringify(flairPostData),
            method: HttpMethods.PUT,
        });
    }

    public async delete(flairId: number)
    {
        const url = `${this._url}/${flairId}`;

        return await fetch(url, {
            method: HttpMethods.DELETE,
        });

    }
}
