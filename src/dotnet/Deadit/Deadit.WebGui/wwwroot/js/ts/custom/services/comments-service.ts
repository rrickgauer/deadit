
import { ApiComments } from "../api/api-comments";
import { CommentApiResponse, ModerateCommentRequest, SaveCommentRequest } from "../domain/model/comment-models";
import { PostPageParms } from "../domain/model/post-models";
import { Guid } from "../domain/types/aliases";
import { ServiceUtility } from "../utilities/service-utility";

export class CommentsService
{
    private readonly _api: ApiComments;

    constructor(args: PostPageParms)
    {
        this._api = new ApiComments(args);
    }


    public saveComment = async (comment: SaveCommentRequest) =>
    {
        const apiResponse = await this._api.put(comment);

        return await ServiceUtility.toServiceResponse<CommentApiResponse>(apiResponse);
    }

    public async deleteComment(commentId: Guid)
    {
        const apiResponse = await this._api.delete(commentId);

        return await ServiceUtility.toServiceResponseNoContent(apiResponse);
    }

    public async getComment(commentId: Guid)
    {
        const apiResponse = await this._api.get(commentId);

        return await ServiceUtility.toServiceResponse<CommentApiResponse>(apiResponse);
    }

    public async moderateComment(request: ModerateCommentRequest)
    {
        const apiResponse = await this._api.patch(request);

        return await ServiceUtility.toServiceResponse<CommentApiResponse>(apiResponse);
    }
}




