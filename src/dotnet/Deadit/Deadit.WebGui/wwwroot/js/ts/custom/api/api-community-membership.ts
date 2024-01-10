import { ApiEndpoints, HttpMethods } from "../domain/constants/api-constants";



export class ApiCommunityMembership
{
    /*    private readonly _urlPrefix = ApiEndpoints.COMMUNITY;*/

    private readonly _communityName: string;
    private readonly _url: string;

    constructor(communityName: string)
    {
        this._communityName = communityName;
        this._url = `${ApiEndpoints.COMMUNITY}/${this._communityName}/members`;
    }


    public put = async () : Promise<Response> =>
    {
        const url = this._url;

        return await fetch(url, {
            method: HttpMethods.PUT,
        });
    }

    public delete = async () =>
    {
        const url = this._url;

        return await fetch(url, {
            method: HttpMethods.DELETE,
        });
    }

    
}