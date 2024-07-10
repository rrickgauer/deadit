import { BootstrapModalEvents, BootstrapUtilityClasses } from "../../../../domain/constants/bootstrap-constants";
import { EditFlairPostEventData, EditFlairPostEvent, FlairPostFormSubmittedEvent, FlairPostFormSubmittedData, NewFlairPostEvent, FlairPostSuccessfullySavedEvent } from "../../../../domain/events/events";
import { ServiceResponse } from "../../../../domain/model/api-response";
import { FlairPostApiRequestBody, GetFlairPostApiResponse } from "../../../../domain/model/flair-models";
import { FlairPostService } from "../../../../services/flair-post-service";
import { AlertUtility } from "../../../../utilities/alert-utility";
import { BootstrapUtility } from "../../../../utilities/bootstrap-utility";
import { ErrorUtility } from "../../../../utilities/error-utility";
import { Nullable } from "../../../../utilities/nullable";
import { FlairForm } from "./flair-form";


const selectors = {
    containerClass: 'flair-form-modal',
    alertsContainerClass: 'form-feedback',
    spinnerContainerClass: 'spinner-container',
    formContainerClass: 'flair-post-form-container',
    modalTitleClass: 'modal-title',
}

const ModalTitleTexts = {
    new: 'New Flair',
    edit: 'Edit Flair',
}

export class FlairFormModal
{

    private static readonly _flairService = new FlairPostService();

    private static readonly _modalElement = document.querySelector<HTMLDivElement>(`.${selectors.containerClass}`);
    private static readonly _alertsContaienr = this._modalElement.querySelector<HTMLDivElement>(`.${selectors.alertsContainerClass}`);
    private static readonly _spinnerContainer = this._modalElement.querySelector<HTMLDivElement>(`.${selectors.spinnerContainerClass}`);
    private static readonly _formContainer = this._modalElement.querySelector<HTMLDivElement>(`.${selectors.formContainerClass}`);
    private static readonly _form = new FlairForm(this._formContainer);
    private static _currentFlairId: number | null = null;

    private static _isNewFlair = false;
    static _communityName: string;

    public static get modal()
    {
        return BootstrapUtility.getModal(this._modalElement);
    }

    private static get _modalTitleText(): string
    {
        return this._modalElement.querySelector<HTMLHeadingElement>(`.${selectors.modalTitleClass}`).innerText;
    }

    private static set _modalTitleText(value: string)
    {
        this._modalElement.querySelector<HTMLHeadingElement>(`.${selectors.modalTitleClass}`).innerText = value;
    }

    public static init(communityName: string)
    {
        this._communityName = communityName;
        this.addListeners();
    }

    

    private static addListeners = () =>
    {
        EditFlairPostEvent.addListener(async (message) =>
        {
            this.onEditFlairPostEvent(message.data);
        });

        NewFlairPostEvent.addListener((message) =>
        {
            this.onNewFlairPostEvent();
        });

        FlairPostFormSubmittedEvent.addListener(async (message) =>
        {
            await this.onFlairPostFormSubmittedEvent(message.data);
        });

        // clear alerts on modal close
        this._modalElement.addEventListener(BootstrapModalEvents.Hidden, (e) =>
        {
            this._alertsContaienr.innerHTML = '';
        });
    }


    

    private static async onEditFlairPostEvent(message: EditFlairPostEventData)
    {
        this.setModalState(false);

        this._currentFlairId = message.flairId;

        this.showSpinner();
        this.showModal();

        const flair = await this.getFlair(message.flairId);

        if (!Nullable.hasValue(flair))
        {
            return;
        }

        this._form.setFlair(flair);

        this.showForm();
    }


    private static async getFlair(flairPostId: number)
    {
        try
        {
            const response = await this._flairService.getFlairPost(flairPostId);

            if (!response.successful)
            {
                AlertUtility.showErrors({
                    container: this._alertsContaienr,
                    errors: response.response.errors,
                });

                return null;
            }

            return response.response.data;
        }
        catch (error)
        {
            ErrorUtility.onException(error, {
                onApiNotFoundException: (e) =>
                {
                    this.showErrorAlert('We could not find this flair. Try refreshing the page.');
                },
                onOther: (e) =>
                {
                    this.showErrorAlert('Unexpected error. Please try again later.');
                },
            });

            return null;
        }
    }

    private static onNewFlairPostEvent()
    {
        this.setModalState(true);
        this._currentFlairId = null;

        this.showSpinner();
        this.showModal();

        this._form.setFlair();

        this.showForm();
    }


    private static async onFlairPostFormSubmittedEvent(message: FlairPostFormSubmittedData)
    {

        this._form.submitButton.spin();

        try
        {

            let result: ServiceResponse<GetFlairPostApiResponse> | null = null;

            if (this._isNewFlair)
            {
                result = await this.createFlair();
            }
            else
            {
                result = await this.updateFlair();
            }

            if (result.successful)
            {
                FlairPostSuccessfullySavedEvent.invoke(this);
                this.modal.hide();
            }
            else
            {
                AlertUtility.showErrors({
                    container: this._alertsContaienr,
                    errors: result.response.errors,
                });
            }
        }
        catch (error)
        {
            ErrorUtility.onException(error, {
                onApiForbiddenException: (e) =>
                {
                    this.showErrorAlert(`You do not have permission to update flairs in this community.`);
                },
                onApiNotFoundException: (e) =>
                {
                    this.showErrorAlert(`Not found. Please try again later`);
                },
                onOther: (e) =>
                {
                    this.showErrorAlert(`Unexpected error. Please try again later.`);
                },
            });
        }
        finally
        {
            this._form.submitButton.reset();
        }


    }

    private static async createFlair(): Promise<ServiceResponse<GetFlairPostApiResponse>>
    {

        const data = this.getApiRequestData();

        const response = await this._flairService.createFlair(data);

        return response;
    }

    private static async updateFlair(): Promise<ServiceResponse<GetFlairPostApiResponse>>
    {
        const data = this.getApiRequestData();

        const response = await this._flairService.updateFlair(this._currentFlairId, data);

        return response;
    }

    private static getApiRequestData(): FlairPostApiRequestBody
    {
        return {
            color: this._form.colorInputValue,
            communityName: this._communityName,
            name: this._form.nameInputValue,
        }
    }

    private static showModal()
    {
        this.modal.show();
    }

    private static showSpinner()
    {
        this._spinnerContainer.classList.remove(BootstrapUtilityClasses.DisplayNone);
        this._formContainer.classList.add(BootstrapUtilityClasses.DisplayNone);
    }

    private static showForm()
    {
        this._spinnerContainer.classList.add(BootstrapUtilityClasses.DisplayNone);
        this._formContainer.classList.remove(BootstrapUtilityClasses.DisplayNone);
    }


    private static setModalState(isNew: boolean)
    {
        this._isNewFlair = isNew;
        this._modalTitleText = isNew ? ModalTitleTexts.new : ModalTitleTexts.edit;
    } 

    private static showErrorAlert(message: string)
    {
        AlertUtility.showDanger({
            container: this._alertsContaienr,
            message: message,
        });
    }
}


