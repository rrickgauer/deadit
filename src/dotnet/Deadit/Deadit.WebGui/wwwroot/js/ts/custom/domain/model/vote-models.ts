import { VoteType } from "../enum/vote-type";
import { DateTimeString, Guid } from "../types/aliases";
import { VoteScores } from "./common-api-response-types";
import { PostPageParms } from "./post-models";


export type CommentVoteApiResponse = VoteScores & {
    voteCommentId?: Guid;
    voteCommentUserId?: number;
    voteCommentType?: VoteType;
    voteCommentCreatedOn?: DateTimeString;
    voteCommentPostId?: Guid;
    voteCommentUserName?: string;
}


export type CommentVoteServiceArgs = PostPageParms & {
    commentId: Guid;
}