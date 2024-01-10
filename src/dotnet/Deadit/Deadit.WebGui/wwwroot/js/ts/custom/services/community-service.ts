import { ApiCommunity } from "../api/api-community";
import { CommunityApiRequest, CreateCommunityApiRequest } from "../domain/model/api-community-models";
import { ServiceResponse } from "../domain/model/api-response";
import { FormDataMapper } from "../mappers/form-data-mapper";
import { ServiceUtilities } from "../utilities/service-utilities";

export class CommunityService
{

    createCommunity = async (newCommunity: CreateCommunityApiRequest) : Promise<ServiceResponse<CommunityApiRequest>> =>
    {
        const formData = FormDataMapper.toFormData(newCommunity);
        const api = new ApiCommunity();
        const response = await api.post(formData);

        return await ServiceUtilities.toServiceResponse<CommunityApiRequest>(response);
    }

}

