import { ApiLogin } from "../api/api-login"
import { ApiSignup } from "../api/api-signup";
import { LoginApiRequest, SignupApiRequest } from "../domain/model/api-auth-models";
import { ApiResponseBase } from "../domain/model/api-response";
import { FormDataMapper } from "../mappers/form-data-mapper";
import { ServiceUtilities } from "../utilities/service-utilities";


export class AuthService
{
    
    login = async(loginCredentials: LoginApiRequest) : Promise<string> =>  {
        
        const formData = FormDataMapper.toFormData(loginCredentials);
        
        const api = new ApiLogin();
        const response = await api.post(formData);
        await ServiceUtilities.handleBadResponse(response);

        return await response.text();
    }

    signup = async (signupInfo: SignupApiRequest) : Promise<ApiResponseBase<number>> => {

        const formData = FormDataMapper.toFormData(signupInfo);
        const api = new ApiSignup();
        const response = await api.post(formData);  

        const data = await response.json();
        console.log({ data });

/*        await ServiceUtilities.handleBadResponse(response);*/

        return data;
/*        return await response.text();*/
    }

}

