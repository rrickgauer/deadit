import { ApiCommunityMembership } from "../api/api-community-membership"
import { GetJoinedCommunityApiRequest } from "../domain/model/api-community-membership-models";
import { ServiceResponse } from "../domain/model/api-response";
import { ServiceUtilities } from "../utilities/service-utilities";


export class CommunityMembershipService
{

    joinCommunity = async (communityName: string): Promise<ServiceResponse<GetJoinedCommunityApiRequest>> =>
    {
        const api = new ApiCommunityMembership(communityName);
        const response = await api.put();

        return await ServiceUtilities.toServiceResponse<GetJoinedCommunityApiRequest>(response);
    }

    leaveCommunity = async (communityName: string): Promise<ServiceResponse<any>> =>
    {
        const api = new ApiCommunityMembership(communityName);
        const response = await api.delete();

        return await ServiceUtilities.toServiceResponseNoContent(response);
    }
}