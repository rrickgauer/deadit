import { ApiPostsLink, ApiPostsText } from "../api/api-posts";
import { CreateLinkPostApiRequest, LinkPostApiResponse, CreateTextPostApiRequest, TextPostApiResponse, UpdateTextPostApiRequest } from "../domain/model/post-models";
import { Guid } from "../domain/types/aliases";
import { ServiceUtility } from "../utilities/service-utility";

export class PostService
{
    private readonly _communityName: string;
    
    constructor(communityName: string)
    {
        this._communityName = communityName;        
    }

    public createTextPost = async (textPost: CreateTextPostApiRequest) =>
    {
        const api = new ApiPostsText(this._communityName);

        const apiResponse = await api.post(textPost);

        return await ServiceUtility.toServiceResponse<TextPostApiResponse>(apiResponse);
    }

    public createLinkPost = async (linkPost: CreateLinkPostApiRequest) =>
    {
        const api = new ApiPostsLink(this._communityName);

        const apiResponse = await api.post(linkPost);

        return await ServiceUtility.toServiceResponse<LinkPostApiResponse>(apiResponse);
    }

    public async updateTextPost(postId: Guid, request: UpdateTextPostApiRequest)
    {
        const api = new ApiPostsText(this._communityName);

        const apiResponse = await api.put(postId, request);

        return await ServiceUtility.toServiceResponse<TextPostApiResponse>(apiResponse);
    }
}