import { ApiCommunityMembership } from "../api/api-community-membership"
import { ServiceResponse } from "../domain/model/api-response";
import { ServiceUtilities } from "../utilities/service-utilities";


export class CommunityMembershipService
{

    joinCommunity = async (communityName: string) =>
    {
        const api = new ApiCommunityMembership(communityName);
        const response = await api.put();



        return response;
    }

    leaveCommunity = async (communityName: string): Promise<ServiceResponse<any>> =>
    {
        const api = new ApiCommunityMembership(communityName);
        const response = await api.delete();

        return await ServiceUtilities.toServiceResponseNoContent(response);
    }
}