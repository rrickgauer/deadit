import { InputFeedbackTextArea } from "../../../domain/helpers/input-feedback";
import { SpinnerButton } from "../../../domain/helpers/spinner-button";
import { ErrorMessage } from "../../../domain/model/api-response";
import { Guid } from "../../../domain/types/aliases";
import { AlertUtility } from "../../../utilities/alert-utility";
import { ErrorUtility } from "../../../utilities/error-utility";
import { Nullable } from "../../../utilities/nullable";
import { CommentListItem, CommentSelectors } from "./comment-list-item";



export const CommentFormSelectors = {
    formClass: 'form-post-comment',
    newFormClass: 'form-post-comment-new',
    submitButtonClass: 'btn-submit',
    cancelButtonClass: 'btn-form-post-comment-cancel',
    messagesContainerClass: 'form-feedback',
    commentIdAttr: 'data-comment-id',
    parentIdAttr: 'data-comment-parent-id',
}


export class CommentForm
{
    form: HTMLFormElement;
    contentInput: InputFeedbackTextArea;
    submitBtn: SpinnerButton;
    errorMessageContainer: any;

    public get commentId(): Guid
    {
        return this.form.getAttribute(CommentFormSelectors.commentIdAttr);
    }

    public set commentId(value: Guid)
    {
        this.form.setAttribute(CommentFormSelectors.commentIdAttr, value);
    }

    public get parentCommentId(): Guid
    {
        return this.form.getAttribute(CommentFormSelectors.parentIdAttr);
    }

    public set parentCommentId(value: Guid)
    {
        this.form.setAttribute(CommentFormSelectors.parentIdAttr, value);
    }

    public get isNew(): boolean
    {
        return this.form.classList.contains(CommentFormSelectors.newFormClass);
    }

    public get contentValue(): string | null
    {
        const inputValue = this.contentInput.inputElement.value;
        return Nullable.getValue(inputValue, null);
    }

    public set contentValue(value: string | null)
    {
        this.contentInput.inputElement.value = Nullable.getValue(value, '');
    }

    public get submitButtonElement(): HTMLButtonElement
    {
        return this.form.querySelector('.btn-submit') as HTMLButtonElement;
    }


    constructor(element: Element)
    {
        this.form = element.closest(`.${CommentFormSelectors.formClass}`) as HTMLFormElement;
        this.contentInput = new InputFeedbackTextArea(this.form.querySelector('textarea[name="content"]'), true);
        this.submitBtn = new SpinnerButton(this.submitButtonElement);
        this.errorMessageContainer = this.form?.querySelector(`.${CommentFormSelectors.messagesContainerClass}`);
    }


    public getParentListItem(): CommentListItem
    {
        const element = this.form.closest(`.${CommentSelectors.listItemClass}`);
        return new CommentListItem(element);
    }

    public showLoading()
    {
        this.submitBtn.spin();
        this.contentInput.inputElement.disabled = true;
    }

    public showNormal()
    {
        this.submitBtn.reset();
        this.contentInput.inputElement.disabled = false;
    }

    public showErrors(errors: ErrorMessage[])
    {
        const html = ErrorUtility.generateErrorList(errors);

        AlertUtility.showDanger({
            container: this.errorMessageContainer,
            message: html,
        });
    }

    public isInvalid(): boolean
    {
        if (!Nullable.hasValue(this.contentValue))
        {
            this.contentInput.showInvalid('Please provide some text');
            return true;
        }

        return false;
    }

    public displaySuccessfulAlert(message: string)
    {
        AlertUtility.showSuccess({
            container: this.errorMessageContainer,
            message: message,
        });
    }
}