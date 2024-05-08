import { ApiPostsLink, ApiPostsText } from "../api/api-posts";
import { LinkPostApiRequest, LinkPostApiResponse, TextPostApiRequest, TextPostApiResponse } from "../domain/model/post-models";
import { ServiceUtilities } from "../utilities/service-utilities";

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

        return await ServiceUtilities.toServiceResponse<TextPostApiResponse>(apiResponse);
    }






    public createLinkPost = async (linkPost: LinkPostApiRequest) =>
    {
        const api = new ApiPostsLink(this._communityName);

        const apiResponse = await api.post(linkPost);

        return await ServiceUtilities.toServiceResponse<LinkPostApiResponse>(apiResponse);
    }
}