import { BaseEventDetail } from "../../domain/events/custom-events";
import { SuccessfulLoginEvent, SuccessfulSignupEvent } from "../../domain/events/events";
import { BootstrapUtility } from "../../utilities/bootstrap-utility";
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
            SuccessfulLoginEvent.addListener(this.handleSuccessfulFormSubmission);
            SuccessfulSignupEvent.addListener(this.handleSuccessfulFormSubmission);
        }   
    }

    private handleSuccessfulFormSubmission = (data: BaseEventDetail) =>
    {
        const url = new URL(window.location.href);

        const destinationUrlParm = url.searchParams.get('destination');

        if (destinationUrlParm === null)
        {
            window.location.href = window.location.href;
        }
        else
        {
            window.location.href = destinationUrlParm;
        }
    }


    public static ShowModal()
    {
        const elements = new LoginModalElements();

        BootstrapUtility.showModal(elements.modalContainer);
    }
}