import { InputFeedback } from "../../domain/helpers/input-feedback";
import { SpinnerButton } from "../../domain/helpers/spinner-button";

export class LoginFormElements {
    public static readonly FORM_LOGIN = '.login-form-login';

    // login form
    public formLogin: HTMLFormElement;
    public loginInputUsername: HTMLInputElement;
    public loginInputPassword: HTMLInputElement;
    public feedbackLoginUsername: InputFeedback;
    public feedbackLoginPassword: InputFeedback;
    public loginSubmitButton: HTMLButtonElement;
    public loginSpinnerButton: SpinnerButton;


    constructor(form: HTMLFormElement) {
        this.formLogin = form;

        this.loginInputUsername = this.formLogin.querySelector('.login-form-input-section.username input');
        this.loginInputPassword = this.formLogin.querySelector('.login-form-input-section.password input');
        this.loginSubmitButton = this.formLogin.querySelector('.btn-submit');

        this.feedbackLoginUsername = new InputFeedback(this.loginInputUsername);
        this.feedbackLoginPassword = new InputFeedback(this.loginInputPassword);

        this.loginSpinnerButton = new SpinnerButton(this.loginSubmitButton);
    }


}
