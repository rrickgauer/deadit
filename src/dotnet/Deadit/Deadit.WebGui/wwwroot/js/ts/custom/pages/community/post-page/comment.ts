import { NativeEvents } from "../../../domain/constants/native-events";
import { IController } from "../../../domain/contracts/i-controller";
import { GuidUtility } from "../../../utilities/guid-utility";
import { CommentForm } from "./comment-form";

export class CommentElement implements IController
{
    public element: HTMLLIElement;
    public btnToggle: HTMLAnchorElement;
    public btnReply: HTMLAnchorElement;
    public replyForm: CommentForm;
    replyFormContainer: HTMLDivElement;

    constructor(element: Element)
    {
        this.element = element as HTMLLIElement;
        this.btnToggle = this.element.querySelector('.btn-comment-action-toggle') as HTMLAnchorElement;
        this.btnReply = this.element.querySelector('.btn-comment-action-reply') as HTMLAnchorElement;

        this.replyFormContainer = this.element.querySelector('.form-post-comment-new-container') as HTMLDivElement;
        this.replyForm = new CommentForm(this.replyFormContainer.querySelector('.form-post-comment'));
    }


    public control()
    {
        this.addListeners();
    }


    protected addListeners = () =>
    {
        this.btnToggle.addEventListener(NativeEvents.Click, (e) =>
        {
            e.preventDefault();
            this.onToggleBtnClick();
        });

        this.btnReply.addEventListener(NativeEvents.Click, (e) =>
        {
            e.preventDefault();
            this.onReplyBtnClick();
        });

    }

    protected onToggleBtnClick = () =>
    {
        alert('toggle');
    }

    protected onReplyBtnClick = () =>
    {
        this.showReplyForm();
    }

    public showReplyForm()
    {
        this.replyFormContainer.classList.remove('d-none');
    }

    public closeReplyForm()
    {
        this.replyFormContainer.classList.add('d-none');
        this.replyForm.inputContent.inputElement.value = '';
        this.replyForm.commentId = GuidUtility.getRandomGuid();
    }
}