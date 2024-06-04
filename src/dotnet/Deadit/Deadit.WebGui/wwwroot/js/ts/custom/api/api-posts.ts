import { ApiEndpoints, HttpMethods } from "../domain/constants/api-constants";
import { GetPostApiRequest, ModeratePostForm } from "../domain/model/post-models";
import { Guid } from "../domain/types/aliases";
import { UrlUtility } from "../utilities/url-utility";
import { ApiUtility } from "./api-base";



export class ApiPosts
{
    protected readonly _url: string;

    constructor(communityName: string)
    {
        this._url = `${ApiEndpoints.COMMUNITY}/${communityName}/posts`;
    }

    public async delete(postId: Guid): Promise<Response>
    {
        const url = `${this._url}/${postId}`;

        return await fetch(url, {
            method: HttpMethods.DELETE,
        });
    }

    public async get(postData: GetPostApiRequest): Promise<Response>
    {
        const query = UrlUtility.getQueryParmsString({
            sort: postData.commentsSort,
        });

        const url = `${this._url}/${postData.postId}?${query}`;

        return await fetch(url);
    }

    public async patch(postId: Guid, form: ModeratePostForm)
    {
        const url = `${this._url}/${postId}`;

        return await ApiUtility.fetchJson(url, {
            body: JSON.stringify(form),
            method: HttpMethods.PATCH,
        });
    }
}

