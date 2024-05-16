import { ApiVotes } from "../api/api-votes";
import { ServiceResponse } from "../domain/model/api-response";
import { CommentVoteApiRequest, CommentVoteApiResponse, PostVoteApiRequest, PostVoteApiResponse } from "../domain/model/vote-models";
import { ServiceUtility } from "../utilities/service-utility";



export class VoteService
{
    public async voteComment(vote: CommentVoteApiRequest): Promise<ServiceResponse<CommentVoteApiResponse>>
    {
        const api = new ApiVotes();

        const apiResponse = await api.putComment(vote);

        return await ServiceUtility.toServiceResponse<CommentVoteApiResponse>(apiResponse);
    }

    public async votePost(vote: PostVoteApiRequest): Promise<ServiceResponse<PostVoteApiResponse>>
    {
        const api = new ApiVotes();

        const apiResponse = await api.putPost(vote);

        return await ServiceUtility.toServiceResponse<PostVoteApiResponse>(apiResponse);
    }
}