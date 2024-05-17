import { PostType } from "../enum/post-type";
import { VoteType } from "../enum/vote-type";
import { DateTimeString, Guid } from "../types/aliases";
import { VoteScores } from "./common-api-response-types";



export type VoteApiRequest = {
    voteType: VoteType;
}

export type CommentVoteApiRequest = VoteApiRequest & {
    commentId: Guid;
}

export type PostVoteApiRequest = VoteApiRequest & {
    postId: Guid;
}


export type CommentVoteApiResponse = VoteScores & {
    voteCommentId?: Guid;
    voteCommentUserId?: number;
    voteCommentType?: VoteType;
    voteCommentCreatedOn?: DateTimeString;
    voteCommentPostId?: Guid;
    voteCommentUserName?: string;
}

export type PostVoteApiResponse = VoteScores & {
    votePostId?: Guid;
    votePostUserId?: number;
    votePostVoteType?: VoteType;
    votePostCreatedOn?: DateTimeString;
    votePostUserName?: string;
    votePostCommunityId?: number;
    votePostCommunityName?: string;
    votePostPostType?: PostType;
}

