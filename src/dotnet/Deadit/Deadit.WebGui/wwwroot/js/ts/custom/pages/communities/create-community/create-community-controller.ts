import { NativeEvents } from "../../../domain/constants/native-events";
import { InputFeebackState } from "../../../domain/enum/input-feedback-state";
import { ApiErrorCode } from "../../../domain/enum/api-error-codes";
import { CommunityApiRequest, CreateCommunityApiRequest } from "../../../domain/model/api-community-models";
import { ApiResponse, ErrorMessage } from "../../../domain/model/api-response";
import { CommunityService } from "../../../services/community-service";
import { ErrorService } from "../../../services/error-service";
import { CreateCommunityElements } from "./create-community-elements";

export class CreateCommunityController
{

    private _elements = new CreateCommunityElements();
    private _communityService = new CommunityService();


    public control()
    {
        this.addListeners();
    }


    private addListeners = () =>
    {
        this._elements.form.addEventListener(NativeEvents.Submit, async (e) =>
        {
            e.preventDefault();
            await this.onFormSubmit();
        });

        this._elements.inputName.addEventListener(NativeEvents.KeyDown, () =>
        {
            this._elements.inputFeedbackName.state = InputFeebackState.None;
        });
    }


    private async onFormSubmit()
    {
        this._elements.submitButtonSpinner.spin();

        try
        {
            const formData = this.getCreateCommunityApiRequest();
            const response = await this._communityService.createCommunity(formData);

            if (response.successful)
            {
                window.location.href = `/${response.response.data.communityUrlGui}`;
            }
            else
            {
                this.onSubmitBadRequest(response.response);
            }
        }
        catch (error)
        {
            ErrorService.handleApiValidationException(error, {
                onApiValidationException: () =>
                {
                    alert('validation exception');
                },

                onStandardError: () =>
                {
                    this._elements.submitButtonSpinner.reset();
                    throw error;
                }
            });
        }
        finally
        {
            this._elements.submitButtonSpinner.reset();
        }
    }


    private getCreateCommunityApiRequest(): CreateCommunityApiRequest
    {
        const result = new CreateCommunityApiRequest(this._elements.inputName.value, this._elements.inputTitle.value, this._elements.inputDescription.value);

        return result;
    }


    private onSubmitBadRequest(apiResponse: ApiResponse<CommunityApiRequest>)
    {
        const nameErrorDisplayText = this.createDisplayApiErrorMessageForName(apiResponse.errors);
        this._elements.inputFeedbackName.showInvalid(nameErrorDisplayText);
    }

    private createDisplayApiErrorMessageForName(errors: ErrorMessage[])
    {
        let nameErrorMessage: string = "";

        for (const errorMessage of errors)
        {
            switch (errorMessage.id)
            {
                case ApiErrorCode.CreateCommunityNameTaken:
                    nameErrorMessage += errorMessage.message;
                    break;

                case ApiErrorCode.CreateCommunityInvalidNameCharacter:
                    nameErrorMessage += errorMessage.message;
                    break;

                case ApiErrorCode.CreateCommunityNameBanned:
                    nameErrorMessage += errorMessage.message;
                    break;
            }
        }

        return nameErrorMessage;
    }


}

