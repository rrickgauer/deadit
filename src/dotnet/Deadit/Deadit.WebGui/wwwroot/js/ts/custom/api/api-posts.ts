import { ApiEndpoints, HttpMethods } from "../domain/constants/api-constants";
import { ApplicationTypes } from "../domain/constants/application-types";
import { LinkPostApiRequest, TextPostApiRequest } from "../domain/model/post-models";



export class ApiPosts
{
    protected readonly _url: string;

    constructor(communityName: string)
    {
        this._url = `${ApiEndpoints.COMMUNITY}/${communityName}/posts`;
    }
}

export class ApiPostsText 
{
    protected readonly _url: string;

    constructor(communityName: string)
    {
        this._url = `${ApiEndpoints.COMMUNITY}/${communityName}/posts/text`;
    }

    public post = async (textPost: TextPostApiRequest) =>
    {
        const url = this._url;

        return await fetch(url, {
            body: JSON.stringify(textPost),
            method: HttpMethods.POST,
            headers: ApplicationTypes.GetJsonHeaders(),
        });
    }
}


export class ApiPostsLink
{
    protected readonly _url: string;

    constructor(communityName: string)
    {
        this._url = `${ApiEndpoints.COMMUNITY}/${communityName}/posts/link`;
    }

    public post = async (textPost: LinkPostApiRequest) =>
    {
        const url = this._url;

        return await fetch(url, {
            body: JSON.stringify(textPost),
            method: HttpMethods.POST,
            headers: ApplicationTypes.GetJsonHeaders(),
        });
    }
}