
import { NativeEvents } from "../../domain/constants/native-events";
import { InputFeebackState } from "../../domain/enum/InputFeebackState";
import { ApiErrorCode } from "../../domain/enum/api-error-codes";
import { InputFeedback } from "../../domain/helpers/input-feedback";
import { LoginApiRequest, SignupApiRequest } from "../../domain/model/api-auth-models";
import { ErrorMessage, ServiceResponse } from "../../domain/model/api-response";
import { AuthService } from "../../services/auth-service";
import { LoginModalElements } from "./LoginModalElements";


export class LoginModalController
{
    private _elements = new LoginModalElements();
    private _authService = new AuthService();

    public init = () =>
    {
        this.addListeners();
    }

    /**
     * Add the event listeners to the DOM
     */
    private addListeners = () =>
    {

        this._elements.formLogin.addEventListener(NativeEvents.Submit, async (e) =>
        {
            e.preventDefault();
            await this.onLoginFormSubmit();
        });

        this._elements.formSignup.addEventListener(NativeEvents.Submit, async (e) =>
        {
            e.preventDefault();
            await this.onSignupFormSubmit();
        });


        this.clearFeedbackOnKeydown(this._elements.feedbackSignupEmail);
        this.clearFeedbackOnKeydown(this._elements.feedbackSignupUsername);
        this.clearFeedbackOnKeydown(this._elements.feedbackSignupPassword);
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
            window.location.href = window.location.href;
        }
        else
        {
            this.badLogin();
        }

    }

    /**
     * Create a new LoginApiRequest object
     * @returns
     */
    private getLoginApiRequestModel = () =>
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

    /**
     * Event handler for a signup form submission event
     */
    private onSignupFormSubmit = async () =>
    {
        // clear the current input feedbacks
        this._elements.feedbackLoginUsername.state = InputFeebackState.None;
        this._elements.feedbackLoginPassword.state = InputFeebackState.None;

        this._elements.signupSpinnerButton.spin();

        // gather the form input data
        const userData = new SignupApiRequest(this._elements.signupInputEmail.value, this._elements.signupInputUsername.value, this._elements.signupInputPassword.value);

        // send it over to the API to create their account
        const result = await this._authService.signup(userData);

        this._elements.signupSpinnerButton.reset();

        // handle the api response
        if (result.successful)
        {
            window.location.href = window.location.href;
        }
        else
        {
            this.handleBadSignup(result);
        }
    }

    /**
     * Something is fucked up
     * @param response
     */
    private handleBadSignup = (response: ServiceResponse<any>) =>
    {
        this.clearSignupFeedbackStates();

        for (const error of response.response.errors)
        {
            this.displaySignupFormError(error);
        }
    }

    /**
     * Display the given ErrorMessage beneath the corresponding input.
     * @param error
     */
    private displaySignupFormError = (error: ErrorMessage) =>
    {
        switch (error.id)
        {
            case ApiErrorCode.SignUpEmailTaken:
                this._elements.feedbackSignupEmail.showInvalid(error.message);
                break;

            case ApiErrorCode.SignupUsernameTaken:
                this._elements.feedbackSignupUsername.showInvalid(error.message);
                break;

            case ApiErrorCode.SignupInvalidPassword:
                this._elements.feedbackSignupPassword.showInvalid(error.message);
                break;
        }
    }

    /**
     * Clear all the feedback states for the signup form
     */
    private clearSignupFeedbackStates = () =>
    {
        this._elements.feedbackSignupEmail.state = InputFeebackState.None;
        this._elements.feedbackSignupUsername.state = InputFeebackState.None;
        this._elements.feedbackSignupPassword.state = InputFeebackState.None;
    }


    /**
     * Setup a new LoginModalController instance for the current webpage
     * @returns an instantiated LoginModalController
     */
    static setupPage = () : LoginModalController =>
    {
        const modal = new LoginModalController();
        modal.init();

        return modal;
    }
}


