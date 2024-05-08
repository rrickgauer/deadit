import { CommentForm } from "../../pages/community/post-page/comment-form";
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



export type CommentFormSubmittedData = {
    form: CommentForm;
}

export const CommentFormSubmittedEvent = new CustomMessage<CommentFormSubmittedData>();

