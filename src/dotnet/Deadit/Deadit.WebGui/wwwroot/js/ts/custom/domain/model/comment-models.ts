import { DateTimeString, Guid, JsonObject, Nullable } from "../types/aliases";
import { UserVoteScores, UserVoteSelection, VoteScores } from "./common-api-response-types";





export type CommentApiResponse = UserVoteScores & {

    commentId?: Guid;
    commentAuthorId?: number;
    commentPostId?: Guid;
    commentContent?: string;
    commentParentId?: Nullable<Guid>;
    commentCreatedOn?: DateTimeString;
    commentDeletedOn?: DateTimeString | null;
    commentAuthorUsername?: string;
    communityId?: number;
    communityName?: string;
    commentIsAuthor?: boolean;
    createdOnDifferenceDisplay?: string;
    commentReplies?: CommentApiResponse[];
}


export type GetCommentsApiResponse = {
    comments?: CommentApiResponse[];
    isLoggedIn?: boolean;
}


export type CommentApiRequestForm = {
    content: string;
    parentId: Guid | null;
}

export type CommentApiRequestUrlFields = {
    communityName: string;
    postId: Guid;
    commentId: Guid;
}


export class SaveCommentRequest
{
    public commentId: Guid;
    public form: CommentApiRequestForm;

    constructor(commentId: Guid, form: CommentApiRequestForm)
    {
        this.commentId = commentId;
        this.form = form;
    }

    public toJson(): JsonObject
    {
        return JSON.stringify(this.form);
    }
}