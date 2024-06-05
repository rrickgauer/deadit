import { ApiEndpoints, HttpMethods } from "../domain/constants/api-constants";
import { GetPostApiRequest, ModeratePostForm } from "../domain/model/post-models";
import { Guid } from "../domain/types/aliases";
import { UrlUtility } from "../utilities/url-utility";
import { ApiUtility } from "./api-base";



export class ApiPosts
{
    private _urlNew: string;

    constructor()
    {
        this._urlNew = `${ApiEndpoints.POSTS}`;
    }

    public async delete(postId: Guid): Promise<Response>
    {
        const url = `${this._urlNew}/${postId}`;

        return await fetch(url, {
            method: HttpMethods.DELETE,
        });
    }

    public async get(postData: GetPostApiRequest): Promise<Response>
    {
        const query = UrlUtility.getQueryParmsString({
            sort: postData.commentsSort,
        });

        const url = `${this._urlNew}/${postData.postId}?${query}`;

        return await fetch(url);
    }

    public async patch(postId: Guid, form: ModeratePostForm)
    {
        const url = `${this._urlNew}/${postId}`;

        return await ApiUtility.fetchJson(url, {
            body: JSON.stringify(form),
            method: HttpMethods.PATCH,
        });
    }
}

