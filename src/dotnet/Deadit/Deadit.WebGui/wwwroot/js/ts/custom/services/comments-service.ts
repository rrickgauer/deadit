
import { ApiComments } from "../api/api-comments";
import { ServiceResponse } from "../domain/model/api-response";
import { CommentApiResponse, GetCommentsApiResponse, SaveCommentRequest } from "../domain/model/comment-models";
import { PostPageParms } from "../domain/model/post-models";
import { ServiceUtility } from "../utilities/service-utility";

export class CommentsService
{
    private readonly _api: ApiComments;

    constructor(args: PostPageParms)
    {
        this._api = new ApiComments(args);
    }

    public getAllComments = async (): Promise<ServiceResponse<GetCommentsApiResponse>> =>
    {
        const apiResponse = await this._api.getAll();

        return await ServiceUtility.toServiceResponse<GetCommentsApiResponse>(apiResponse);
    }


    public saveComment = async (comment: SaveCommentRequest) =>
    {
        const apiResponse = await this._api.put(comment);

        return await ServiceUtility.toServiceResponse<CommentApiResponse>(apiResponse);
    }
}




