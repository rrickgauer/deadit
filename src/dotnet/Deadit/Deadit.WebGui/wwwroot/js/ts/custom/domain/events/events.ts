import { PostDropdownAction } from "../enum/post-dropdown-action";
import { SortOption } from "../enum/sort-option";
import { TopPostSort } from "../enum/top-post-sort";
import { CommentApiResponse } from "../model/comment-models";
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

