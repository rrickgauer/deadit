import { ApiEndpoints, HttpMethods } from "../domain/constants/api-constants";
import { Guid } from "../domain/types/aliases";



export class ApiPostLock
{
    private readonly _communityName: string;
    private readonly _postId: string;
    private readonly _url: string;

    constructor(communityName: string, postId: Guid)
    {
        this._communityName = communityName;
        this._postId = postId;
        this._url = `${ApiEndpoints.COMMUNITY}/${this._communityName}/posts/${this._postId}/lock`;
    }


    public async put()
    {
        const url = this._url;

        return await fetch(url, {
            method: HttpMethods.PUT,
        });
    }

    public async delete()
    {
        const url = this._url;

        return await fetch(url, {
            method: HttpMethods.DELETE,
        });
    }

}

