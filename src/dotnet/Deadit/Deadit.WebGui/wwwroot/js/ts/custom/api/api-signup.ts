import { ApiEndpoints, HttpMethods } from "../domain/constants/api-constants";



export class ApiSignup {

    //#region Private Members
    private readonly _url: string = ApiEndpoints.SIGNUP;
    //#endregion


    /**
     * POST: /api/auth/signup
     * @param signupFormData the signup form data
     * @returns the api response
     */
    post = async (signupFormData: FormData): Promise<Response> => {
        const url = this._url;

        return await fetch(url, {
            body: signupFormData,
            method: HttpMethods.POST,
        })
    }
}
