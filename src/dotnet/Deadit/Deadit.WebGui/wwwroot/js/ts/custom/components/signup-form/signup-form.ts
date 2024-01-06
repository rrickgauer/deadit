import { NativeEvents } from "../../domain/constants/native-events";
import { ApiErrorCode } from "../../domain/enum/api-error-codes";
import { InputFeebackState } from "../../domain/enum/input-feedback-state";
import { SuccessfulSignupEvent } from "../../domain/events/events";
import { InputFeedback } from "../../domain/helpers/input-feedback";
import { SignupApiRequest } from "../../domain/model/api-auth-models";
import { ErrorMessage, ServiceResponse } from "../../domain/model/api-response";
import { AuthService } from "../../services/auth-service";
import { SignupFormElements } from "./signup-form-elements";

export class SignupForm
{
    private _elements: SignupFormElements;
    private _authService = new AuthService();

    constructor(form: HTMLFormElement)
    {
        this._elements = new SignupFormElements(form);
        this.addListeners();
    }


    private addListeners = () =>
    {
        this._elements.formSignup.addEventListener(NativeEvents.Submit, async (e) =>
        {
            e.preventDefault();
            await this.onSignupFormSubmit();
        });


        this.clearFeedbackOnKeydown(this._elements.feedbackSignupEmail);
        this.clearFeedbackOnKeydown(this._elements.feedbackSignupUsername);
        this.clearFeedbackOnKeydown(this._elements.feedbackSignupPassword);
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
     * Event handler for a signup form submission event
     */
    private onSignupFormSubmit = async () =>
    {
        // disable the signup submit button
        this._elements.signupSpinnerButton.spin();

        // gather the form input data
        const userData = new SignupApiRequest(this._elements.signupInputEmail.value, this._elements.signupInputUsername.value, this._elements.signupInputPassword.value);

        // send it over to the API to create their account
        const result = await this._authService.signup(userData);

        this._elements.signupSpinnerButton.reset();

        // handle the api response
        if (result.successful)
        {
            SuccessfulSignupEvent.invoke(this);
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

}