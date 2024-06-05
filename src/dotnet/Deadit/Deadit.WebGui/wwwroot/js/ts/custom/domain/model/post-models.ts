import { PostType } from "../enum/post-type";
import { SortOption } from "../enum/sort-option";
import { VoteType } from "../enum/vote-type";
import { DateTimeString, Guid } from "../types/aliases";
import { CommunityApiRequest } from "./api-community-models";
import { CommentApiResponse } from "./comment-models";
import { VoteScores } from "./vote-scores";


export type PostPageParms = {
    communityName: string;
    postId: Guid;
}


export type CreatePostApiRequest = {
    title: string;
    communityName: string;
}

export type CreateTextPostApiRequest = CreatePostApiRequest & {
    content?: string;
}

export type CreateLinkPostApiRequest = CreatePostApiRequest & {
    url: string;
}


export type UpdateTextPostApiRequest = {
    content: string;
}


export type GetPostApiRequest = {
    postId: Guid;
    commentsSort: SortOption;
}



export type PostApiResponse = CommunityApiRequest & VoteScores & {
    postId?: Guid;
    postCommunityId?: number;
    postTitle?: string;
    postType?: PostType;
    postAuthorId?: number;
    postCreatedOn?: DateTimeString;
    postIsArchived?: boolean;
    postIsRemoved?: boolean;
    postIsDeleted?: boolean;
    postIsLocked?: boolean;
    postUriWeb?: string;
    postUriApi?: string;
    postCountComments?: number;
}

export type TextPostApiResponse = PostApiResponse & {
    postContent?: string;
}

export type LinkPostApiResponse = PostApiResponse & {
    postUrl?: string;
}

export type PostContentApiResponse = PostApiResponse & {
    postBodyContent?: string;

}



export type GetPostPageApiResponse = {
    isLoggedIn: boolean;
    isCommunityModerator: boolean;
    isPostAuthor: boolean;
    postIsDeleted: boolean;
    isPostModRemoved: boolean;
    postIsLocked: boolean;
    clientPostVote: VoteType;
    post: PostContentApiResponse;
    comments: CommentApiResponse[];
}



export type ModeratePostForm = {
    locked?: boolean;
    removed?: boolean;
}















