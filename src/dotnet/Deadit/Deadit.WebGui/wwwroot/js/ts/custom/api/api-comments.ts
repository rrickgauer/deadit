import { ApiEndpoints, HttpMethods } from "../domain/constants/api-constants";
import { ModerateCommentRequest, SaveCommentRequest } from "../domain/model/comment-models";
import { Guid } from "../domain/types/aliases";
import { ApiUtility } from "./api-base";

export class ApiComments
{

    private get _urlNew(): string
    {
        return `${ApiEndpoints.COMMENTS}`;
    }

    public async put (comment: SaveCommentRequest): Promise<Response>
    {
        const url = `${this._urlNew}/${comment.commentId}`;

        return await ApiUtility.fetchJson(url, {
            body: comment.toJson(),
            method: HttpMethods.PUT,
        });
    }

    public async delete(commentId: Guid): Promise<Response>
    {
        const url = `${this._urlNew}/${commentId}`;

        return await fetch(url, {
            method: HttpMethods.DELETE,
        });
    }

    public async get(commentId: Guid): Promise<Response>
    {
        const url = `${this._urlNew}/${commentId}`;

        return await fetch(url);
    }


    public async patch(request: ModerateCommentRequest)
    {
        const url = `${this._urlNew}/${request.commentId}`;

        return await ApiUtility.fetchJson(url, {
            body: request.toJson(),
            method: HttpMethods.PATCH,
        });
    }
}


