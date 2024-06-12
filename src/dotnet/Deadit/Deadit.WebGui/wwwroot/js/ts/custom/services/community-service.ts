import { ApiCommunity } from "../api/api-community";
import { CommunityApiRequest, CreateCommunityApiRequest, UpdateCommunityRequest } from "../domain/model/api-community-models";
import { ServiceResponse } from "../domain/model/api-response";
import { ServiceUtility } from "../utilities/service-utility";

export class CommunityService
{

    public createCommunity = async (newCommunity: CreateCommunityApiRequest) : Promise<ServiceResponse<CommunityApiRequest>> =>
    {
        const formData = JSON.stringify(newCommunity);
        const api = new ApiCommunity();
        const response = await api.post(formData);

        return await ServiceUtility.toServiceResponse<CommunityApiRequest>(response);
    }


    public async updateCommunity(community: UpdateCommunityRequest)
    {   
        const api = new ApiCommunity();
        const response = await api.put(community.communityName, community.data);

        return await ServiceUtility.toServiceResponse<CommunityApiRequest>(response);
    }

}

