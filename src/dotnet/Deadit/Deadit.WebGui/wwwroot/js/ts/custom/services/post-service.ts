import { ApiPostsLink, ApiPostsText } from "../api/api-posts";
import { LinkPostApiRequest, LinkPostApiResponse, TextPostApiRequest, TextPostApiResponse } from "../domain/model/post-models";
import { ServiceUtility } from "../utilities/service-utility";

export class PostService
{
    private readonly _communityName: string;
    
    constructor(communityName: string)
    {
        this._communityName = communityName;        
    }

    public createTextPost = async (textPost: TextPostApiRequest) =>
    {
        const api = new ApiPostsText(this._communityName);

        const apiResponse = await api.post(textPost);

        return await ServiceUtility.toServiceResponse<TextPostApiResponse>(apiResponse);
    }






    public createLinkPost = async (linkPost: LinkPostApiRequest) =>
    {
        const api = new ApiPostsLink(this._communityName);

        const apiResponse = await api.post(linkPost);

        return await ServiceUtility.toServiceResponse<LinkPostApiResponse>(apiResponse);
    }
}