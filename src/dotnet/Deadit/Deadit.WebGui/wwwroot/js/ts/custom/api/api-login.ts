import { ApiEndpoints, HttpMethods } from "../domain/constants/api-constants"



export class ApiLogin
{
    #url = ApiEndpoints.LOGIN;

    /**
     * POST: /auth/login
     * @param {FormData} credentials 
     * @returns result
     */
    post = async (credentials) => {
        const url = this.#url;

        return await fetch(url, {
            method: HttpMethods.POST,
            body: credentials,
        });
    }
}


