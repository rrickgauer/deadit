import { LoginForm } from "../login-form/login-form";
import { SignupForm } from "../signup-form/signup-form";
import { LoginModalElements } from "./login-modal-elements";

export class LoginModal
{
    private _loginForm: LoginForm;
    private _signupForm: SignupForm;

    private _elements = new LoginModalElements();

    constructor()
    {
        this._loginForm = new LoginForm(this._elements.loginForm);
        this._signupForm = new SignupForm(this._elements.signupForm);
    }
}