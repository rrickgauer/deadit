
import { ApiComments } from "../api/api-comments";
import { ServiceResponse } from "../domain/model/api-response";
import { CommentApiResponse, GetCommentsApiRequest, GetCommentsApiResponse, SaveCommentRequest } from "../domain/model/comment-models";
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

    public getAllComments = async (request: GetCommentsApiRequest): Promise<ServiceResponse<GetCommentsApiResponse>> =>
    {
        const apiResponse = await this._api.getAll(request);

        return await ServiceUtility.toServiceResponse<GetCommentsApiResponse>(apiResponse);
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
}




