import { ApiCommentVotes } from "../api/api-comment-votes";
import { VoteType } from "../domain/enum/vote-type";
import { ServiceResponse } from "../domain/model/api-response";
import { CommentVoteApiResponse, CommentVoteServiceArgs } from "../domain/model/vote-models";
import { ServiceUtility } from "../utilities/service-utility";



export class CommentVotesService
{
    private readonly _api: ApiCommentVotes;

    constructor(args: CommentVoteServiceArgs)
    {
        this._api = new ApiCommentVotes(args);
    }

    public async vote(voteType: VoteType): Promise<ServiceResponse<CommentVoteApiResponse>>
    {
        const apiResponse = await this._api.put(voteType);

        return await ServiceUtility.toServiceResponse<CommentVoteApiResponse>(apiResponse);
    }
}