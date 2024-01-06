import { ApiLogin } from "../api/api-login"
import { ApiSignup } from "../api/api-signup";
import { LoginApiRequest, SignupApiRequest } from "../domain/model/api-auth-models";
import { ApiResponse, ServiceResponse } from "../domain/model/api-response";
import { FormDataMapper } from "../mappers/form-data-mapper";
import { ServiceUtilities } from "../utilities/service-utilities";


export class AuthService
{

    login = async (loginCredentials: LoginApiRequest): Promise<ServiceResponse<any>> =>
    {
        const formData = FormDataMapper.toFormData(loginCredentials);
        const api = new ApiLogin();
        const response = await api.post(formData);

        return await ServiceUtilities.toServiceResponse<any>(response);
    }

    signup = async (signupInfo: SignupApiRequest): Promise<ServiceResponse<any>> =>
    {
        const formData = FormDataMapper.toFormData(signupInfo);
        const api = new ApiSignup();
        const response = await api.post(formData);

        return await ServiceUtilities.toServiceResponse<any>(response);
    }

}

