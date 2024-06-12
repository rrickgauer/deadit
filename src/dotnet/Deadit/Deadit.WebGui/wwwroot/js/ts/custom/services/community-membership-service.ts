import { ApiCommunityMembership } from "../api/api-community-membership"
import { GetJoinedCommunityApiRequest } from "../domain/model/api-community-membership-models";
import { ServiceResponse } from "../domain/model/api-response";
import { ServiceUtility } from "../utilities/service-utility";


export class CommunityMembershipService
{

    public async joinCommunity (communityName: string): Promise<ServiceResponse<GetJoinedCommunityApiRequest>>
    {
        const api = new ApiCommunityMembership(communityName);
        const response = await api.put();

        return await ServiceUtility.toServiceResponse<GetJoinedCommunityApiRequest>(response);
    }

    public async leaveCommunity (communityName: string): Promise<ServiceResponse<any>>
    {
        const api = new ApiCommunityMembership(communityName);
        const response = await api.delete();

        return await ServiceUtility.toServiceResponseNoContent(response);
    }

    public async removeMember(communityName: string, username: string)
    {
        const api = new ApiCommunityMembership(communityName);
        const response = await api.delete();

        return await ServiceUtility.toServiceResponseNoContent(response);
    }

}