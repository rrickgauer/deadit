import { BaseEventDetail } from "../../domain/events/custom-events";
import { SuccessfulLoginEvent } from "../../domain/events/events";
import { LoginForm } from "../login-form/login-form";
import { SignupForm } from "../signup-form/signup-form";
import { LoginModalElements } from "./login-modal-elements";

export class LoginModal
{
    private _loginForm: LoginForm;
    private _signupForm: SignupForm;

    private _elements = new LoginModalElements();

    private _useListeners;

    constructor(useListeners=true)
    {
        this._loginForm = new LoginForm(this._elements.loginForm);
        this._signupForm = new SignupForm(this._elements.signupForm);

        this._useListeners = useListeners;

        this.addListeners();
    }

    private addListeners = () =>
    {
        if (this._useListeners)
        {
            SuccessfulLoginEvent.addListener(this.onSuccessfulLogin);
        }
        
    }

    private onSuccessfulLogin = (data: BaseEventDetail) =>
    {
        window.location.href = window.location.href;
    }
}