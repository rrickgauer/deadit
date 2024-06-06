import { CommentModerationAction } from "../enum/comment-moderation-action";
import { PostDropdownAction } from "../enum/post-dropdown-action";
import { PostModerationDropdownAction } from "../enum/post-moderation-dropdown-action";
import { SortOption } from "../enum/sort-option";
import { TopPostSort } from "../enum/top-post-sort";
import { UpdateCommunityApiRequest } from "../model/api-community-models";
import { CommentApiResponse } from "../model/comment-models";
import { Guid } from "../types/aliases";
import { CustomEmptyMessage, CustomMessage } from "./custom-events";


export const SuccessfulLoginEvent = new CustomEmptyMessage();
export const SuccessfulSignupEvent = new CustomEmptyMessage();


export type RootCommentFormSubmittedData = {
    comment: CommentApiResponse;
}

export const RootCommentFormSubmittedEvent = new CustomMessage<RootCommentFormSubmittedData>();




export type ItemsSortInputChangedData = {
    selectedValue: SortOption,
    itemsSortId?: string;
}

export const ItemsSortInputChangedEvent = new CustomMessage<ItemsSortInputChangedData>();




export type TopPostSortOptionChangedData = {
    sort: TopPostSort;
}

export const TopPostSortOptionChangedEvent = new CustomMessage<TopPostSortOptionChangedData>();




export type PostDropdownItemClickData = {
    action: PostDropdownAction,
}

export const PostDropdownItemClickEvent = new CustomMessage<PostDropdownItemClickData>();



export type PostModerationDropdownItemClickData = {
    action: PostModerationDropdownAction,
}

export const PostModerationDropdownItemClickEvent = new CustomMessage<PostModerationDropdownItemClickData>();


export type OpenModerateCommentModalData = {
    commentId: Guid;
}

export const OpenModerateCommentModalEvent = new CustomMessage<OpenModerateCommentModalData>();




export type CommentLockedData = {
    comment: CommentApiResponse,
}

export const CommentLockedEvent = new CustomMessage<CommentLockedData>();
