import { ApiEndpoints, HttpMethods } from "../domain/constants/api-constants";
import { Nullable } from "../utilities/nullable";



export class ApiCommunityMembership
{
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

    public delete = async (username?: string) =>
    {
        let url = this._url;

        if (Nullable.hasValue(username))
        {
            url = `${this._url}/${username}`;
        }

        return await fetch(url, {
            method: HttpMethods.DELETE,
        });
    }

    
}