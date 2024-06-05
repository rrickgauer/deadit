import { ApiEndpoints, HttpMethods } from "../domain/constants/api-constants";
import { ApplicationTypes } from "../domain/constants/application-types";
import { CreateLinkPostApiRequest } from "../domain/model/post-models";


export class ApiPostsLink {
    protected readonly _url: string;

    constructor() 
    {
        this._url = `${ApiEndpoints.POSTS}/link`;
    }

    public post = async (textPost: CreateLinkPostApiRequest) => {
        const url = this._url;

        return await fetch(url, {
            body: JSON.stringify(textPost),
            method: HttpMethods.POST,
            headers: ApplicationTypes.GetJsonHeaders(),
        });
    };
}
