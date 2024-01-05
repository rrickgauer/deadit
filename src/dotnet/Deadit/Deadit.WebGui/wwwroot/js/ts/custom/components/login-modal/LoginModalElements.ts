import { InputFeedback } from "../../domain/helpers/input-feedback";
import { SpinnerButton } from "../../domain/helpers/spinner-button";
import { LoginModalSelectors } from "./LoginModalSelectors";


export class LoginModalElements
{
    public modalContainer: HTMLDivElement = document.querySelector(LoginModalSelectors.CONTAINER);

    // login form
    public formLogin: HTMLFormElement = document.querySelector(LoginModalSelectors.FORM_LOGIN);
    public loginInputUsername: HTMLInputElement = document.querySelector(LoginModalSelectors.LOGIN_INPUT_USERNAME);
    public loginInputPassword: HTMLInputElement = document.querySelector(LoginModalSelectors.LOGIN_INPUT_PASSWORD);

    public feedbackLoginUsername: InputFeedback = new InputFeedback(this.loginInputUsername);
    public feedbackLoginPassword: InputFeedback = new InputFeedback(this.loginInputPassword);

    public loginSubmitButton: HTMLButtonElement = this.formLogin.querySelector('.btn-submit');
    public loginSpinnerButton: SpinnerButton = new SpinnerButton(this.loginSubmitButton);

    // signup form
    public formSignup: HTMLFormElement = document.querySelector(LoginModalSelectors.FORM_SIGNUP);
    public signupInputEmail: HTMLInputElement = document.querySelector(LoginModalSelectors.SIGNUP_INPUT_EMAIL);
    public signupInputUsername: HTMLInputElement = document.querySelector(LoginModalSelectors.SIGNUP_INPUT_USERNAME);
    public signupInputPassword: HTMLInputElement = document.querySelector(LoginModalSelectors.SIGNUP_INPUT_PASSWORD);

    public feedbackSignupEmail: InputFeedback = new InputFeedback(this.signupInputEmail);
    public feedbackSignupUsername: InputFeedback = new InputFeedback(this.signupInputUsername);
    public feedbackSignupPassword: InputFeedback = new InputFeedback(this.signupInputPassword);

    public signupSubmitButton: HTMLButtonElement = this.formSignup.querySelector('.btn-submit');
    public signupSpinnerButton: SpinnerButton = new SpinnerButton(this.signupSubmitButton);
}





