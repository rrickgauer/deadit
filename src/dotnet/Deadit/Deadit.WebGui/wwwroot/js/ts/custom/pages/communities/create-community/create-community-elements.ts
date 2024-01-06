import { InputFeedback } from "../../../domain/helpers/input-feedback";
import { SpinnerButton } from "../../../domain/helpers/spinner-button";


export class CreateCommunityElements {
    public container: HTMLDivElement = document.querySelector('.create-community-form-container');
    public form: HTMLFormElement = this.container.querySelector('.create-community-form');

    public inputName: HTMLInputElement = this.form.querySelector('#create-community-form-input-name');
    public inputTitle: HTMLInputElement = this.form.querySelector('#create-community-form-input-title');
    public inputDescription: HTMLTextAreaElement = this.form.querySelector('#create-community-form-input-description');

    public inputFeedbackName: InputFeedback = new InputFeedback(this.inputName);
    public inputFeedbackTitle: InputFeedback = new InputFeedback(this.inputTitle);
    public inputFeedbackDescription: InputFeedback = new InputFeedback(this.inputDescription);


    public submitButton: HTMLButtonElement = this.container.querySelector('.btn-submit');
    public submitButtonSpinner: SpinnerButton = new SpinnerButton(this.submitButton);
}






