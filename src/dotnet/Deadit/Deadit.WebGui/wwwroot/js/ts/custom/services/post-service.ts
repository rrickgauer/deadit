import { ApiPosts } from "../api/api-posts";
import { ApiPostsLink } from "../api/api-posts-link";
import { ApiPostsText } from "../api/api-posts-text";
import { ServiceResponse } from "../domain/model/api-response";
import { CreateLinkPostApiRequest, CreateTextPostApiRequest, TextPostApiResponse, UpdateTextPostApiRequest, GetPostPageApiResponse, GetPostApiRequest, ModeratePostForm } from "../domain/model/post-models";
import { Guid } from "../domain/types/aliases";
import { ServiceUtility } from "../utilities/service-utility";

export class PostService
{    

    public createTextPost = async (textPost: CreateTextPostApiRequest) =>
    {
        const api = new ApiPostsText();

        const apiResponse = await api.post(textPost);

        return await ServiceUtility.toServiceResponse<GetPostPageApiResponse>(apiResponse);
    }

    public createLinkPost = async (linkPost: CreateLinkPostApiRequest) =>
    {
        const api = new ApiPostsLink();

        const apiResponse = await api.post(linkPost);

        return await ServiceUtility.toServiceResponse<GetPostPageApiResponse>(apiResponse);
    }

    public async updateTextPost(postId: Guid, request: UpdateTextPostApiRequest)
    {
        const api = new ApiPostsText();

        const apiResponse = await api.put(postId, request);

        return await ServiceUtility.toServiceResponse<TextPostApiResponse>(apiResponse);
    }

    public async deletePost(postId: Guid)
    {
        const api = new ApiPosts();

        const response = await api.delete(postId);

        return await ServiceUtility.toServiceResponseNoContent(response);
    }

    public async getPost(postData: GetPostApiRequest): Promise<ServiceResponse<GetPostPageApiResponse>>
    {
        const api = new ApiPosts();

        const response = await api.get(postData);

        return await ServiceUtility.toServiceResponse<GetPostPageApiResponse>(response);
    }

    public async moderatePost(postId: Guid, moderateForm: ModeratePostForm)
    {
        const api = new ApiPosts();

        const response = await api.patch(postId, moderateForm);

        return await ServiceUtility.toServiceResponseNoContent(response);
    }



}