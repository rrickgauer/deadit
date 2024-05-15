import { SortOption } from "../enum/sort-option";
import { CommentApiResponse } from "../model/comment-models";
import { Guid } from "../types/aliases";
import { BaseEvent } from "./custom-events";
import { CustomMessage } from "./custom-events2";


export class SuccessfulLoginEvent extends BaseEvent { }
export class SuccessfulSignupEvent extends BaseEvent { }


export type ExampleMessageClass = {
    username: string;
    age: number;
}

export const TestingEvent = new CustomMessage<ExampleMessageClass>();



export type RootCommentFormSubmittedData = {
    comment: CommentApiResponse;
}

export const RootCommentFormSubmittedEvent = new CustomMessage<RootCommentFormSubmittedData>();


export type ItemsSortInputChangedData = {
    selectedValue: SortOption,
    itemsSortId?: string;
}

export const ItemsSortInputChangedEvent = new CustomMessage<ItemsSortInputChangedData>();