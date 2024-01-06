import { NativeEvents } from "../../domain/constants/native-events";
import { InputFeebackState } from "../../domain/enum/input-feedback-state";
import { SuccessfulLoginEvent } from "../../domain/events/events";
import { InputFeedback } from "../../domain/helpers/input-feedback";
import { LoginApiRequest } from "../../domain/model/api-auth-models";
import { AuthService } from "../../services/auth-service";
import { LoginFormElements } from "./login-form-elements";


export class LoginForm
{
    private _elements: LoginFormElements;
    private _authService = new AuthService();

    constructor(form: HTMLFormElement)
    {
        this._elements = new LoginFormElements(form);

        this.addListeners();
    }


    private addListeners = () =>
    {
        this._elements.formLogin.addEventListener(NativeEvents.Submit, async (e) =>
        {
            e.preventDefault();
            await this.onLoginFormSubmit();
        });

        this.clearFeedbackOnKeydown(this._elements.feedbackLoginUsername);
        this.clearFeedbackOnKeydown(this._elements.feedbackLoginPassword);
    }

    /**
     * Helper to add event listeners to InputFeedback objects to clear the feedback when a keydown event is fired
     * @param inputFeedback
     */
    private clearFeedbackOnKeydown = (inputFeedback: InputFeedback) =>
    {
        inputFeedback.inputElement.addEventListener(NativeEvents.KeyDown, (e) =>
        {
            inputFeedback.state = InputFeebackState.None;
        });
    }

    /**
     * User tried to login.
     * Event handler for login form submission event
     */
    private onLoginFormSubmit = async () =>
    {
        this._elements.loginSpinnerButton.spin();

        const loginModel = this.getLoginApiRequestModel();
        const response = await this._authService.login(loginModel);

        this._elements.loginSpinnerButton.reset();

        if (response.successful)
        {
            /*            window.location.href = window.location.href;*/

            SuccessfulLoginEvent.invoke(this);
        }
        else
        {
            this.badLogin();
        }
    }

    /**
     * Create a new LoginApiRequest object
     */
    private getLoginApiRequestModel = (): LoginApiRequest =>
    {
        return new LoginApiRequest(this._elements.loginInputUsername.value, this._elements.loginInputPassword.value);
    }

    /**
     * Display error message to user
     */
    private badLogin = () =>
    {
        this._elements.feedbackLoginUsername.showInvalid('Username and password do not match');
        this._elements.feedbackLoginPassword.showInvalid('');
    }
}


