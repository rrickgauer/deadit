import { NativeEvents } from "../../../domain/constants/native-events";
import { CommentFormSubmittedEvent } from "../../../domain/events/events";
import { InputFeedbackTextArea } from "../../../domain/helpers/input-feedback";
import { SpinnerButton } from "../../../domain/helpers/spinner-button";
import { Guid } from "../../../domain/types/aliases";
import { Nullable } from "../../../utilities/nullable";




export class CommentForm
{
    public form: HTMLFormElement;
    public inputContent: InputFeedbackTextArea;
    public btnSubmit: SpinnerButton;
    public messagesContainer: HTMLDivElement;
    public fieldSet: HTMLFieldSetElement;
    
    public get isRootComment(): boolean
    {
        return Nullable.hasValue(this.parentId);
    } 

    public get commentId(): Guid | null
    {
        return this.form.getAttribute('data-comment-id') ?? null;
    }

    public set commentId(value: Guid | null)
    {
        this.form.setAttribute('data-comment-id', value);
    }

    public get parentId(): Guid | null
    {
        return this.form.getAttribute('data-comment-parent-id') ?? null;
    }


    constructor(formElement: HTMLFormElement)
    {
        this.form = formElement;
        this.inputContent = new InputFeedbackTextArea(this.form.querySelector('textarea[name="content"]'));
        this.btnSubmit = new SpinnerButton(this.form.querySelector('.btn-submit'));
        this.messagesContainer = this.form.querySelector('.form-feedback') as HTMLDivElement;

        this.fieldSet = this.form.querySelector('fieldset') as HTMLFieldSetElement;
    }

    public clearFormInputs = () =>
    {
        this.inputContent.inputElement.value = '';
    }


    public show()
    {
        this.form.classList.remove('d-none');
    }

    public hide()
    {
        this.form.classList.add('d-none');
    }



    // #region
    public static addSubmitListeners = () =>
    {
        document.querySelector('body').addEventListener(NativeEvents.Submit, (e) =>
        {
            const target = e.target as HTMLFormElement;

            if (target.classList.contains('form-post-comment'))
            {
                e.preventDefault();

                CommentFormSubmittedEvent.invoke(this, {
                    form: new CommentForm(target),
                });
            }
        });
    }

    // #endregion

}


