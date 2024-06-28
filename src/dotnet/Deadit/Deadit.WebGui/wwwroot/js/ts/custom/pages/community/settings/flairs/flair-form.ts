import { NativeEvents } from "../../../../domain/constants/native-events";
import { FlairPostFormSubmittedEvent } from "../../../../domain/events/events";
import { InputFeedbackText } from "../../../../domain/helpers/input-feedback";
import { SpinnerButton } from "../../../../domain/helpers/spinner-button";
import { GetFlairPostApiResponse, SaveFlairPostData } from "../../../../domain/model/flair-models";
import { Nullable } from "../../../../utilities/nullable";



const fairFormSelectors = {
    formClass: 'flair-post-form',
    
    nameInputId: 'flair-post-form-input-name',
    colorInputId: 'flair-post-form-input-color',
    submitButtonClass: 'btn-submit',

}

export class FlairForm
{
    public readonly form: HTMLFormElement;
    public readonly fieldset: HTMLFieldSetElement;
    public readonly inputName: InputFeedbackText;
    public readonly inputColor: InputFeedbackText;
    public readonly submitButton: SpinnerButton;


    public get nameInputValue(): string | null
    {
        const value = this.inputName.inputElement.value;
        return Nullable.getValue<string>(value, null);
    }

    public set nameInputValue(value: string | null)
    {
        this.inputName.inputElement.value = value ?? "";
    }


    public get colorInputValue(): string | null
    {
        const value = this.inputColor.inputElement.value;
        return Nullable.getValue<string>(value, null);
    }

    public set colorInputValue(value: string | null)
    {
        this.inputColor.inputElement.value = value ?? "";
    }


    constructor(e: Element, flair?: GetFlairPostApiResponse)
    {
        this.form = e.querySelector<HTMLFormElement>(`.${fairFormSelectors.formClass}`);
        this.fieldset = this.form.querySelector<HTMLFieldSetElement>(`fieldset`);
        this.inputName = new InputFeedbackText(this.form.querySelector<HTMLInputElement>(`#${fairFormSelectors.nameInputId}`));
        this.inputColor = new InputFeedbackText(this.form.querySelector<HTMLInputElement>(`#${fairFormSelectors.colorInputId}`));
        this.submitButton = new SpinnerButton(this.form.querySelector<HTMLButtonElement>(`.${fairFormSelectors.submitButtonClass}`));

        this.setFlair(flair);

        this.addListeners();
    }

    public setFlair(flair?: GetFlairPostApiResponse)
    {
        if (Nullable.hasValue(flair))
        {
            this.colorInputValue = flair.flairPostColor;
            this.nameInputValue = flair.flairPostName;
        }
        else
        {
            this.colorInputValue = '#000000';
            this.nameInputValue = "";
        }
    }


    private addListeners = () =>
    {
        this.form.addEventListener(NativeEvents.Submit, (e) =>
        {
            e.preventDefault();

            const flair = this.getData();

            FlairPostFormSubmittedEvent.invoke(this, {
                flair: flair,
            });
        });
    }

    private getData(): SaveFlairPostData
    {
        return {
            color: this.colorInputValue,
            name: this.nameInputValue,
        }
    }


}