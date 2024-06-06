import { NativeEvents } from "../../../../domain/constants/native-events";
import { IController } from "../../../../domain/contracts/i-controller";
import { CommunityType } from "../../../../domain/enum/community-type";
import { TextPostBodyRule } from "../../../../domain/enum/text-post-body-rule";
import { InputFeedbackText, InputFeedbackTextArea } from "../../../../domain/helpers/input-feedback";
import { RadioGroup } from "../../../../domain/helpers/radio-group/radio-group";
import { SpinnerButton } from "../../../../domain/helpers/spinner-button";
import { CommunityApiRequest, UpdateCommunityApiRequest, UpdateCommunityRequest } from "../../../../domain/model/api-community-models";
import { ServiceResponse } from "../../../../domain/model/api-response";
import { CommunityService } from "../../../../services/community-service";
import { AlertUtility } from "../../../../utilities/alert-utility";
import { ErrorUtility } from "../../../../utilities/error-utility";
import { Nullable } from "../../../../utilities/nullable";

const FormSelectors = {
    formClass: 'community-settings-form',
    titleInputName: 'title',
    descriptionInputName: 'description',
    acceptingNewMembersId: 'community-settings-form-membership-checkbox',
    alertsContainerClass: 'form-feedback',
    textPostContentRadiosClass: 'text-post-content-input-radios',
    communityTypeRadiosClass: 'community-type-input-radios',
}


export class GeneralSettingsForm implements IController
{
    private readonly _communityName: string;
    private readonly _form: HTMLFormElement;
    private readonly _inputTitle: InputFeedbackText;
    private readonly _inputDescription: InputFeedbackTextArea;
    private readonly _inputAcceptingNewMembers: HTMLInputElement;
    private readonly _submitButton: SpinnerButton;
    private readonly _alertsContainer: HTMLDivElement;
    private readonly _communityTypeInput: RadioGroup<CommunityType>;
    private readonly _textPostRuleInput: RadioGroup<TextPostBodyRule>;
    private _communityService: CommunityService;
    private _fieldSet: HTMLFieldSetElement;

    private get isAcceptingNewMembers(): boolean
    {
        return this._inputAcceptingNewMembers.checked;
    }

    private set isAcceptingNewMembers(value: boolean)
    {
        this._inputAcceptingNewMembers.checked = value;
    }

    constructor(communityName: string)
    {
        this._communityName = communityName;

        this._form                     = document.querySelector<HTMLFormElement>(`.${FormSelectors.formClass}`);
        this._inputTitle               = new InputFeedbackText(this._form.querySelector(`input[name="${FormSelectors.titleInputName}"]`));
        this._inputDescription         = new InputFeedbackTextArea(this._form.querySelector(`textarea[name="${FormSelectors.descriptionInputName}"]`));
        this._inputAcceptingNewMembers = this._form.querySelector<HTMLInputElement>(`#${FormSelectors.acceptingNewMembersId}`);
        this._submitButton             = new SpinnerButton(this._form.querySelector(`.btn-submit`));
        this._alertsContainer          = this._form.querySelector<HTMLDivElement>(`.${FormSelectors.alertsContainerClass}`);
        this._communityTypeInput       = new RadioGroup<CommunityType>(this._form.querySelector(`.${FormSelectors.communityTypeRadiosClass}`));
        this._textPostRuleInput        = new RadioGroup<TextPostBodyRule>(this._form.querySelector(`.${FormSelectors.textPostContentRadiosClass}`));

        this._fieldSet = this._form.querySelector<HTMLFieldSetElement>('fieldset');

        this._communityService = new CommunityService();

    }


    public control()
    {
        this.addListeners();
    }

    private addListeners = () =>
    {
        // Enable/disable the submit button if the title input value is empty
        this._inputTitle.inputElement.addEventListener(NativeEvents.KeyUp, (e) =>
        {
            this.toggleSubmitButtonDisable();
        });


        // listen for form submit event
        this._form.addEventListener(NativeEvents.Submit, async (e) =>
        {
            e.preventDefault();
            await this.onFormSubmit();
        });
    }

    /**
     * Enable/disable the submit button if the title input value is empty
     */
    private toggleSubmitButtonDisable()
    {
        const titleLength = this._inputTitle.inputElement.value.length;

        this._submitButton.button.disabled = true;

        if (titleLength > 0)
        {
            this._submitButton.button.disabled = false;
        }
    }


    private async onFormSubmit()
    {
        try
        {
            // disable the form
            this._submitButton.spin();
            this._fieldSet.disabled = true;

            // send api request
            const formData = this.getFormData();
            const updateCommunityData = new UpdateCommunityRequest(this._communityName, formData);
            const response = await this._communityService.updateCommunity(updateCommunityData);

            // enable the form
            this._submitButton.reset();
            this._fieldSet.disabled = false;

            // handle the response
            this.handleUpdateCommunityResponse(response);

        }
        catch (error)
        {
            this._submitButton.reset();
            this._fieldSet.disabled = false;
            this.handleUpdateCommunityException(error);
        }
    }

    private handleUpdateCommunityResponse(response: ServiceResponse<CommunityApiRequest>)
    {
        if (!response.successful)
        {
            AlertUtility.showErrors({
                container: this._alertsContainer,
                errors: response.response.errors,
            });
        }
        else
        {
            AlertUtility.showSuccess({
                container: this._alertsContainer,
                message: 'Settings saved successfully.',
            });
        }
    }

    private handleUpdateCommunityException(error)
    {
        ErrorUtility.onException(error, {
            onApiNotFoundException: (e) =>
            {
                AlertUtility.showDanger({
                    container: this._alertsContainer,
                    message: 'The community was not found. Please try again later.',
                });
            },

            onApiValidationException: (e) =>
            {
                AlertUtility.showDanger({
                    container: this._alertsContainer,
                    message: 'Validation error. Please try again later.',
                });
            },

            onOther: (e) =>
            {
                AlertUtility.showDanger({
                    container: this._alertsContainer,
                    message: 'Unknown error. Please try again later.',
                });
            },
        });
    }


    private getFormData(): UpdateCommunityApiRequest
    {
        const result: UpdateCommunityApiRequest = {
            acceptingNewMembers: this.isAcceptingNewMembers,
            communityType: this._communityTypeInput.selectedValue,
            description: Nullable.getValue<string>(this._inputDescription.inputElement.value, null),
            textPostBodyRule: this._textPostRuleInput.selectedValue,
            title: this._inputTitle.inputElement.value,
        }

        return result;
    }
}