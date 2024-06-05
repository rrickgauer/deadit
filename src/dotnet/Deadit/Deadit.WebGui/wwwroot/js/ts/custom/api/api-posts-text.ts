import { ApiEndpoints, HttpMethods } from "../domain/constants/api-constants";
import { ApplicationTypes } from "../domain/constants/application-types";
import { CreateTextPostApiRequest, UpdateTextPostApiRequest } from "../domain/model/post-models";
import { Guid } from "../domain/types/aliases";


export class ApiPostsText {
    protected readonly _url: string;

    constructor() 
    {
        this._url = `${ApiEndpoints.POSTS}/text`;
    }

    public async post (textPost: CreateTextPostApiRequest){
        const url = this._url;

        return await fetch(url, {
            body: JSON.stringify(textPost),
            method: HttpMethods.POST,
            headers: ApplicationTypes.GetJsonHeaders(),
        });
    };

    public async put(postId: Guid, request: UpdateTextPostApiRequest)
    {
        const url = `${this._url}/${postId}`;

        return await fetch(url, {
            body: JSON.stringify(request),
            method: HttpMethods.PUT,
            headers: ApplicationTypes.GetJsonHeaders(),
        });
    }
}
