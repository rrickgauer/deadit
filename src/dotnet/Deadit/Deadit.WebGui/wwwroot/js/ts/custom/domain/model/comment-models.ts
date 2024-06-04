import { DateTimeString, Guid, JsonObject, Nullable } from "../types/aliases";
import { UserVoteScores } from "./vote-scores";


export type CommentApiResponse = UserVoteScores & {

    commentId?: Guid;
    commentAuthorId?: number;
    commentPostId?: Guid;
    commentContent?: string;
    commentParentId?: Nullable<Guid>;
    commentCreatedOn?: DateTimeString;
    commentDeletedOn?: DateTimeString | null;
    commentAuthorUsername?: string;
    commentIsLocked?: boolean;
    commentIsRemoved?: boolean;
    commentIsAuthor?: boolean;
    communityId?: number;
    communityName?: string;

    createdOnDifferenceDisplay?: string;
    commentReplies?: CommentApiResponse[];
    userIsCommunityModerator?: boolean;
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




export type PatchCommentApiRequest = {
    locked: boolean;
    removed: boolean;
}

export class ModerateCommentRequest
{
    public readonly commentId: string;
    public readonly form: PatchCommentApiRequest;

    constructor(commentId: Guid, form: PatchCommentApiRequest)
    {
        this.commentId = commentId;
        this.form = form;
    }

    public toJson(): JsonObject
    {
        return JSON.stringify(this.form);
    }
}


