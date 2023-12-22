import { InputFeebackState } from "../enum/InputFeebackState";


// SEE: https://getbootstrap.com/docs/5.3/forms/validation/#server-side

export class InputFeedback
{
    private static readonly IsValidText = "is-valid";
    private static readonly IsInvalidText = "is-invalid";

    private _container: HTMLDivElement;
    private _validFeedbackElement: HTMLDivElement;
    private _invalidFeedbackElement: HTMLDivElement;
    public inputElement: HTMLElement;

    /**
     * Helper class for interfacing with an input's valid feedback text/display
     * @param container
     */
    constructor(container: HTMLElement)
    {
        this._container = container.closest('.input-feedback');

        this._validFeedbackElement = this._container.querySelector('.valid-feedback');
        this._invalidFeedbackElement = this._container.querySelector('.invalid-feedback');
        this.inputElement = this._container.querySelector('.input-feedback-input');
    }


    /**
     * Set the valid text and show it as valid
     * @param text the text to display
     */
    public showValid = (text: string) =>
    {
        this.validFeedbackText = text;
        this.state = InputFeebackState.Valid;
    }

    /**
     * Show the invalid text
     * @param text the text to display
     */
    public showInvalid = (text: string) =>
    {
        this.invalidFeedbackText = text;
        this.state = InputFeebackState.Invalid;
    }

    /**
     * Get the current InputFeedbackState
     */
    public get state(): InputFeebackState
    {
        if (this.inputElement.classList.contains(InputFeedback.IsInvalidText))
        {
            return InputFeebackState.Invalid;
        }
        else if (this.inputElement.classList.contains(InputFeedback.IsValidText))
        {
            return InputFeebackState.Valid;
        }
        else
        {
            return InputFeebackState.None;
        }
    }

    /**
     * Set the state
     */
    public set state(value: InputFeebackState)
    {
        this.clearInputFeedbackClasses();

        switch (value)
        {
            case InputFeebackState.Valid:
                this.inputElement.classList.add(InputFeedback.IsValidText);
                break;

            case InputFeebackState.Invalid:
                this.inputElement.classList.add(InputFeedback.IsInvalidText);
                break;

            default:
                break;
        }
    }

    /**
     * Get the current valid feedback text
     */
    public get validFeedbackText(): string
    {
        return this._validFeedbackElement.innerText;
    }

    /**
     * Set the valid feedback text
     */
    public set validFeedbackText(value: string)
    {
        this._validFeedbackElement.innerText = value;
    }

    /**
     * Get the invalid feedback text
     */
    public get invalidFeedbackText(): string
    {
        return this._invalidFeedbackElement.innerText;
    }

    /**
     * Set the invalid feedback text
     */
    public set invalidFeedbackText(value: string)
    {
        this._invalidFeedbackElement.innerText = value;
    }


    /**
     * Clear the input feedback classes, or set it None
     */
    private clearInputFeedbackClasses()
    {
        this.inputElement.classList.remove(InputFeedback.IsInvalidText, InputFeedback.IsValidText);
    }
}

