import { ApiEndpoints, HttpMethods } from "../domain/constants/api-constants";
import { CommentVoteApiRequest, PostVoteApiRequest } from "../domain/model/vote-models";

export class ApiVotes
{

    public async putComment(vote: CommentVoteApiRequest): Promise<Response>
    {
        const url = `${ApiEndpoints.VOTES_COMMENT}/${vote.commentId}/${vote.voteType}`;

        return await fetch(url, {
            method: HttpMethods.PUT,
        });
    }

    public async putPost(vote: PostVoteApiRequest): Promise<Response>
    {
        const url = `${ApiEndpoints.VOTES_POST}/${vote.postId}/${vote.voteType}`;

        return await fetch(url, {
            method: HttpMethods.PUT,
        });
    }

}