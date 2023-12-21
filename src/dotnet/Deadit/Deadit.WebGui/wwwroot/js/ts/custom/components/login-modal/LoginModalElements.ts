import { LoginModalSelectors } from "./LoginModalSelectors";


export class LoginModalElements {

    // login form
    public formLogin: HTMLFormElement = document.querySelector(LoginModalSelectors.FORM_LOGIN);
    public loginInputUsername: HTMLInputElement = document.querySelector(LoginModalSelectors.LOGIN_INPUT_USERNAME);
    public loginInputPassword: HTMLInputElement = document.querySelector(LoginModalSelectors.LOGIN_INPUT_PASSWORD);

    // signup form
    public formSignup: HTMLFormElement = document.querySelector(LoginModalSelectors.FORM_SIGNUP);
    public signupInputEmail: HTMLInputElement = document.querySelector(LoginModalSelectors.SIGNUP_INPUT_EMAIL);
    public signupInputUsername: HTMLInputElement = document.querySelector(LoginModalSelectors.SIGNUP_INPUT_USERNAME);
    public signupInputPassword: HTMLInputElement = document.querySelector(LoginModalSelectors.SIGNUP_INPUT_PASSWORD);

}
