import { Modal } from "bootstrap";
import { BootstrapUtility } from "../../../utilities/bootstrap-utility";
import { NativeEvents } from "../../../domain/constants/native-events";
import { UrlUtility } from "../../../utilities/url-utility";
import { BootstrapUtilityClasses } from "../../../domain/constants/bootstrap-constants";
import { FlairPostService } from "../../../services/flair-post-service";
import { RadioGroup, RadioGroupNumber } from "../../../domain/helpers/radio-group/radio-group";
import { AlertUtility } from "../../../utilities/alert-utility";
import { GetFlairPostApiResponse } from "../../../domain/model/flair-models";
import { ErrorUtility } from "../../../utilities/error-utility";
import { FlairPostFilterRadioTemplate } from "../../../templates/flair-post-filter-radio-template";
import { Nullable } from "../../../utilities/nullable";



const selectors = {
    modalClass: 'modal-flair-post-filter',
    openModalButtonClass: 'btn-open-flair-filter-modal',
    loadingContainerClass: 'modal-flair-post-filter-loading',
    contentContainerClass: 'modal-flair-post-filter-content',
    alertsContainerClass: 'alerts-container',
    radiosContainerClass: 'flair-post-filter-input-group',
    submitButtonClass: 'btn-submit',
    clearSelectionButtonClass: 'btn-clear-selection',
}


export class FlairPostFilterModal
{
    private static readonly FLAIR_URL_QUERY_PARM_KEY = 'flair';

    private static readonly _communityName = UrlUtility.getCurrentPathValue(1);

    private static readonly _openModalButton = document.querySelector<HTMLButtonElement>(`.${selectors.openModalButtonClass}`);

    private static readonly _modalElement = document.querySelector<HTMLDivElement>(`.${selectors.modalClass}`);
    private static readonly _loadingContainer = this._modalElement.querySelector<HTMLDivElement>(`.${selectors.loadingContainerClass}`);
    private static readonly _contentContainer = this._modalElement.querySelector<HTMLDivElement>(`.${selectors.contentContainerClass}`);
    private static readonly _alertsContainer = this._modalElement.querySelector<HTMLDivElement>(`.${selectors.alertsContainerClass}`);
    private static readonly _radiosContainer = this._modalElement.querySelector<HTMLDivElement>(`.${selectors.radiosContainerClass}`);
    private static readonly _btnSubmit = this._modalElement.querySelector<HTMLButtonElement>(`.${selectors.submitButtonClass}`);
    private static readonly _btnClearSelection = this._modalElement.querySelector<HTMLButtonElement>(`.${selectors.clearSelectionButtonClass}`);

    private static readonly _filterService = new FlairPostService();


    private static get _modal(): Modal
    {
        return BootstrapUtility.getModal(this._modalElement);
    }

    private static get _currentUrlFlairQueryParm(): number | null
    {
        return UrlUtility.getQueryParmValueNumber(this.FLAIR_URL_QUERY_PARM_KEY);
    }

    private static get _radioInputGroup(): RadioGroupNumber
    {
        return new RadioGroupNumber(this._radiosContainer);
    }


    public static async init()
    {
        this.showLoading();

        const flairs = await this.fetchFlairs();
        this.displayFlairs(flairs);

        this.showContent();

        this.addListeners();


    }

    private static addListeners()
    {
        this._openModalButton.addEventListener(NativeEvents.Click, (e) =>
        {
            this.showModal();
        });

        this._btnSubmit.addEventListener(NativeEvents.Click, (e) =>
        {
            this.filterPosts();
        });

        this._btnClearSelection.addEventListener(NativeEvents.Click, (e) =>
        {
            this.clearSelection();
        });
    }

    public static showModal()
    {
        this._radioInputGroup.selectedValue = this._currentUrlFlairQueryParm;
        this._modal.show();
    }

    private static async fetchFlairs(): Promise<GetFlairPostApiResponse[]>
    {
        try
        {
            const response = await this._filterService.getCommunityFlairPosts(this._communityName);

            if (!response.successful)
            {
                AlertUtility.showErrors({
                    container: this._alertsContainer,
                    errors: response.response.errors,
                });

                return [];
            }

            return response.response.data;
        }
        catch (error)
        {
            ErrorUtility.onException(error, {
                onOther: (e) =>
                {
                    this.showErrorAlert('There was an unexprected error fetching the post flairs. Please try again later');
                },
            });

            return [];
        }
    }

    private static displayFlairs(flairs: GetFlairPostApiResponse[])
    {
        const htmlTemplate = new FlairPostFilterRadioTemplate();
        this._radiosContainer.innerHTML = htmlTemplate.toHtmls(flairs);

        this._radioInputGroup.selectedValue = this._currentUrlFlairQueryParm;
    }


    private static showErrorAlert(message: string)
    {
        AlertUtility.showDanger({
            container: this._alertsContainer,
            message: message,
        });
    }


    private static showLoading()
    {
        this._loadingContainer.classList.remove(BootstrapUtilityClasses.DisplayNone);
        this._contentContainer.classList.add(BootstrapUtilityClasses.DisplayNone);
    }

    private static showContent()
    {
        this._loadingContainer.classList.add(BootstrapUtilityClasses.DisplayNone);
        this._contentContainer.classList.remove(BootstrapUtilityClasses.DisplayNone);
    }


    private static clearSelection()
    {
        const currentUrl = this.getCurrentUrlNoParms();
        window.location.href = currentUrl.toString();
    }

    private static filterPosts()
    {

        const currentUrl = this.getCurrentUrlNoParms();

        if (Nullable.hasValue(this._radioInputGroup.selectedValue))
        {
            currentUrl.searchParams.set('flair', `${this._radioInputGroup.selectedValue}`);
        }

        window.location.href = currentUrl.toString();
    }


    private static getCurrentUrlNoParms(): URL
    {
        const currentUrl = new URL(window.location.href);

        for (const key of currentUrl.searchParams.keys())
        {
            currentUrl.searchParams.delete(key);
        }

        return currentUrl;
    }
}