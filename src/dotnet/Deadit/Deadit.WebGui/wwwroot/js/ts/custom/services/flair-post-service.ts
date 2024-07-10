import { ApiFlairPosts } from "../api/api-flair-posts";
import { ServiceResponse } from "../domain/model/api-response";
import { FlairPostApiRequestBody, GetFlairPostApiResponse } from "../domain/model/flair-models";
import { ServiceUtility } from "../utilities/service-utility";



export class FlairPostService
{

    public async getCommunityFlairPosts(communityName: string): Promise<ServiceResponse<GetFlairPostApiResponse[]>>
    {
        const api = new ApiFlairPosts();
        const response = await api.getAll(communityName);

        return await ServiceUtility.toServiceResponse<GetFlairPostApiResponse[]>(response);
    }

    public async getFlairPost(flairPostId: number): Promise<ServiceResponse<GetFlairPostApiResponse>>
    {
        const api = new ApiFlairPosts();
        const response = await api.get(flairPostId);

        return await ServiceUtility.toServiceResponse<GetFlairPostApiResponse>(response);
    }

    public async createFlair(flairData: FlairPostApiRequestBody)
    {
        const api = new ApiFlairPosts();
        const response = await api.post(flairData);

        return await ServiceUtility.toServiceResponse<GetFlairPostApiResponse>(response);
    }

    public async updateFlair(flairId: number, flairData: FlairPostApiRequestBody)
    {
        const api = new ApiFlairPosts();
        const response = await api.put(flairId, flairData);

        return await ServiceUtility.toServiceResponse<GetFlairPostApiResponse>(response);
    }

    public async deleteFlair(flairId: number)
    {
        const api = new ApiFlairPosts();
        const response = await api.delete(flairId);

        return await ServiceUtility.toServiceResponseNoContent(response);
    }
}