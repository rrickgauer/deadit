import { LoginForm } from "../../components/login-form/login-form"
import { SignupForm } from "../../components/signup-form/signup-form";
import { BaseEventDetail } from "../../domain/events/custom-events";
import { SuccessfulLoginEvent, SuccessfulSignupEvent } from "../../domain/events/events";


export class LoginController
{
    private _loginForm = new LoginForm(document.querySelector('.main-content .login-form-login'));
    private _signupForm = new SignupForm(document.querySelector('.main-content .signup-form'));

    constructor()
    {

    }

    public control = () =>
    {
        this.addListeners();
    }

    private addListeners = () =>
    {
        SuccessfulLoginEvent.addListener(this.redirectPage);
        SuccessfulSignupEvent.addListener(this.redirectPage);
    }

    private redirectPage = (e: BaseEventDetail) =>
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

}