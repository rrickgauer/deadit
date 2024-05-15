import { ApiEndpoints, HttpMethods } from "../domain/constants/api-constants";
import { VoteType } from "../domain/enum/vote-type";
import { CommentVoteServiceArgs } from "../domain/model/vote-models";
import { Guid } from "../domain/types/aliases";



export class ApiCommentVotes {
    private readonly _postId: Guid;
    private readonly _communityName: string;
    private readonly _commentId: Guid;

    private get _url(): string {
        return `${ApiEndpoints.COMMUNITY}/${this._communityName}/posts/${this._postId}/comments/${this._commentId}/votes`;
    }

    constructor(args: CommentVoteServiceArgs) 
    {
        this._communityName = args.communityName;
        this._postId = args.postId;
        this._commentId = args.commentId;
    }

    public async put(voteType: VoteType): Promise<Response> 
    {
        const url = `${this._url}/${voteType}`;

        return await fetch(url, {
            method: HttpMethods.PUT,
        });
    }
}
