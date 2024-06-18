import { IModelForm } from "../../../domain/contracts/imodel-form";
import { InputFeedbackText } from "../../../domain/helpers/input-feedback";
import { SpinnerButton } from "../../../domain/helpers/spinner-button";
import { ErrorMessage } from "../../../domain/model/api-response";
import { AlertUtility } from "../../../utilities/alert-utility";
import { Nullable } from "../../../utilities/nullable";


export type CreatePostFormModel = {
    title: string | null;
    content: string | null;
}

export class CreatePostFormElements implements IModelForm<CreatePostFormModel>
{
    public form: HTMLFormElement;
    public inputTitle: InputFeedbackText;
    public inputContent: InputFeedbackText;
    public formFeedback: HTMLDivElement;
    public submitBtn: SpinnerButton;
    public fieldSet: HTMLFieldSetElement;

    public get titleInputValue(): string
    {
        return this.inputTitle.inputElement.value;
    }

    public get contentInputValue()
    {
        return this.inputContent.inputElement.value;
    }

    constructor(args: { form: HTMLFormElement; }) 
    {
        this.form = args.form;
        this.inputTitle = new InputFeedbackText(this.form.querySelector('.input-feedback [name="title"]'));
        this.inputContent = new InputFeedbackText(this.form.querySelector('.input-feedback [name="content"]'));
        this.formFeedback = this.form.querySelector('.form-feedback') as HTMLDivElement;
        this.submitBtn = new SpinnerButton(this.form.querySelector('.btn-submit'));
        this.fieldSet = this.form.querySelector('fieldset') as HTMLFieldSetElement;
    }

    public getModel = () =>
    {
        const result: CreatePostFormModel = {
            title: Nullable.getValue(this.titleInputValue),
            content: Nullable.getValue(this.contentInputValue),
        };

        return result;
    }


    public disableInputs = () => {
        this.fieldSet.disabled = true;
    };


    public showErrors(errors: ErrorMessage[])
    {
        AlertUtility.showErrors({
            container: this.formFeedback,
            errors: errors,
        });
    }

    public showErrorAlert(message: string)
    {
        AlertUtility.showDanger({
            container: this.formFeedback,
            message: message,
        });
    }

    public showSuccessfulAlert(message: string)
    {
        AlertUtility.showSuccess({
            container: this.formFeedback,
            message: message,
        });
    }
}
