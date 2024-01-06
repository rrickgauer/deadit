import { InputFeedback } from "../../domain/helpers/input-feedback";
import { SpinnerButton } from "../../domain/helpers/spinner-button";


export class SignupFormElements {
    public formSignup: HTMLFormElement;

    public signupInputEmail: HTMLInputElement;
    public signupInputUsername: HTMLInputElement;
    public signupInputPassword: HTMLInputElement;

    public signupSubmitButton: HTMLButtonElement;

    public feedbackSignupEmail: InputFeedback;
    public feedbackSignupUsername: InputFeedback;
    public feedbackSignupPassword: InputFeedback;

    public signupSpinnerButton: SpinnerButton;


    public constructor(form: HTMLFormElement) {
        this.formSignup = form;

        this.signupInputEmail = this.formSignup.querySelector('.signup-form-input-section.email input');
        this.signupInputUsername = this.formSignup.querySelector('.signup-form-input-section.username input');
        this.signupInputPassword = this.formSignup.querySelector('.signup-form-input-section.password input');

        this.signupSubmitButton = this.formSignup.querySelector('.btn-submit');

        this.feedbackSignupEmail = new InputFeedback(this.signupInputEmail);
        this.feedbackSignupUsername = new InputFeedback(this.signupInputUsername);
        this.feedbackSignupPassword = new InputFeedback(this.signupInputPassword);

        this.signupSpinnerButton = new SpinnerButton(this.signupSubmitButton);
    }
}
