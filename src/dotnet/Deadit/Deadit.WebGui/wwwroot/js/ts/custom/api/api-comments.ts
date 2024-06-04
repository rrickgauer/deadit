import { ApiEndpoints, HttpMethods } from "../domain/constants/api-constants";
import { ModerateCommentRequest, SaveCommentRequest } from "../domain/model/comment-models";
import { PostPageParms } from "../domain/model/post-models";
import { Guid } from "../domain/types/aliases";
import { ApiUtility } from "./api-base";

export class ApiComments
{
    private readonly _postId: string;
    private readonly _communityName: string;

    private get _url(): string
    {
        return `${ApiEndpoints.COMMUNITY}/${this._communityName}/posts/${this._postId}/comments`;
    }

    constructor(args: PostPageParms)
    {
        this._communityName = args.communityName;
        this._postId = args.postId;
    }

    public async put (comment: SaveCommentRequest): Promise<Response>
    {
        const url = `${this._url}/${comment.commentId}`;

        return await ApiUtility.fetchJson(url, {
            body: comment.toJson(),
            method: HttpMethods.PUT,
        });
    }

    public async delete(commentId: Guid): Promise<Response>
    {
        const url = `${this._url}/${commentId}`;

        return await fetch(url, {
            method: HttpMethods.DELETE,
        });
    }

    public async get(commentId: Guid): Promise<Response>
    {
        const url = `${this._url}/${commentId}`;

        return await fetch(url);
    }


    public async patch(request: ModerateCommentRequest)
    {
        const url = `${this._url}/${request.commentId}`;

        return await ApiUtility.fetchJson(url, {
            body: request.toJson(),
            method: HttpMethods.PATCH,
        });
    }
}


