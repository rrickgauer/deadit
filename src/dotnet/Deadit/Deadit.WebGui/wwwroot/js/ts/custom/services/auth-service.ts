import { ApiLogin } from "../api/api-login"
import { LoginApiRequest } from "../domain/model/api-auth-models";
import { FormDataMapper } from "../mappers/form-data-mapper";
import { ServiceUtilities } from "../utilities/service-utilities";


export class AuthService
{

    /**
     * 
     * @param {LoginApiRequest} loginCredentials 
     * @returns 
     */
    login = async(loginCredentials) => {
        
        const formData = FormDataMapper.toFormData(loginCredentials);
        
        const api = new ApiLogin();
        const response = await api.post(formData);
        await ServiceUtilities.handleBadResponse(response);

        return await response.text();
    }

}

