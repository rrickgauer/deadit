import { BootstrapUtilityClasses } from "../../../../domain/constants/bootstrap-constants";
import { NativeEvents } from "../../../../domain/constants/native-events";
import { IController, IControllerAsync } from "../../../../domain/contracts/i-controller";
import { FlairPostListItemDropdownAction } from "../../../../domain/enum/flair-post-list-item-dropdown-action";
import { NewFlairPostEvent, FlairPostSuccessfullySavedEvent } from "../../../../domain/events/events";
import { MessageBoxConfirm } from "../../../../domain/helpers/message-box/MessageBoxConfirm";
import { GetFlairPostApiResponse } from "../../../../domain/model/flair-models";
import { FlairPostService } from "../../../../services/flair-post-service";
import { FlairPostTemplate } from "../../../../templates/flair-post-template";
import { AlertUtility } from "../../../../utilities/alert-utility";
import { ErrorUtility } from "../../../../utilities/error-utility";
import { MessageBoxUtility } from "../../../../utilities/message-box-utility";
import { UrlUtility } from "../../../../utilities/url-utility";
import { FlairFormModal } from "./flair-form-modal";
import { FlairListItem } from "./flair-list-item";


const FlairPageSelectors = {
    spinnerClass: 'spinner-spinner',
    pageContentClass: 'page-content',
    flairListClass: 'flair-post-list',
    listItemDropdownButtonClass: 'flair-post-list-item-dropdown-button',
    listItemDropdownButtonActionAttr: 'data-js-action',
    newFlairButtonClass: 'btn-new-flair-post',
    flairListAlertsClass: 'flair-list-alerts',
}

export class CommunitySettingsFlairPageController implements IControllerAsync
{
    private readonly _communityName = UrlUtility.getCurrentPathValue(1);
    private readonly _flairService = new FlairPostService();

    private _spinnerContainer: HTMLDivElement;
    private _pageContent: HTMLDivElement;
    private _flairList: HTMLUListElement;
    private _alertsContainer: HTMLDivElement;


    constructor()
    {
        this._spinnerContainer = document.querySelector<HTMLDivElement>(`.${FlairPageSelectors.spinnerClass}`);
        this._pageContent = document.querySelector<HTMLDivElement>(`.${FlairPageSelectors.pageContentClass}`);
        this._flairList = document.querySelector<HTMLUListElement>(`.${FlairPageSelectors.flairListClass}`);
        this._alertsContainer = document.querySelector<HTMLDivElement>(`.${FlairPageSelectors.flairListAlertsClass}`);

        FlairFormModal.init(this._communityName);
    }


    public async control()
    {
        const flairsLoaded = await this.fetchFlairs();

        if (!flairsLoaded)
        {
            return;
        }

        await this.addListeners();
        this.showPageContent();

    }

    private addListeners = async () =>
    {
        document.addEventListener(NativeEvents.Click, async (e) =>
        {
            const target = e.target as HTMLElement;

            if (target?.classList.contains(FlairPageSelectors.listItemDropdownButtonClass))
            {
                await this.onFlairDropdownButtonClick(target);
            }

            if (target?.classList.contains(FlairPageSelectors.newFlairButtonClass))
            {
                NewFlairPostEvent.invoke(this, {});
            }
        });


        FlairPostSuccessfullySavedEvent.addListener(async (message) =>
        {
            this.fetchFlairs();
        });
    }


    private async fetchFlairs(): Promise<boolean>
    {
        try
        {
            const response = await this._flairService.getCommunityFlairPosts(this._communityName);

            if (!response.successful)
            {
                MessageBoxUtility.showErrorList(response.response.errors);
                return false;
            }

            this.showFlairs(response.response.data);

            return true;
        }
        catch (error)
        {
            ErrorUtility.onException(error, {
                onOther: (e) =>
                {
                    console.error(error)

                    MessageBoxUtility.showError({
                        message: 'Could not fetch the post flairs for this community. Please try again later.',
                        title: 'Unknown Error',
                    });
                },
            });

            return false;
        }
    }


    private showFlairs(flairs: GetFlairPostApiResponse[])
    {
        const templateEngine = new FlairPostTemplate();
        const html = templateEngine.toHtmls(flairs);

        this._flairList.innerHTML = this.getTopRowFlairListItemHtml();
        this._flairList.innerHTML += html;
    }

    private getTopRowFlairListItemHtml(): string
    {
        return `
            <li class="list-group-item list-group-item-secondary">
                <div class="d-flex justify-content-between align-items-center">
                    <div>Current Post Flairs</div>
                    <button class="btn btn-sm btn-success btn-new-flair-post" type="button">New</button>
                </div>
            </li>

        `;
    }



    private showPageContent()
    {
        this._spinnerContainer.classList.add(BootstrapUtilityClasses.DisplayNone);
        this._pageContent.classList.remove(BootstrapUtilityClasses.DisplayNone);
    }


    private async onFlairDropdownButtonClick(e: Element)
    {
        const attrValue = e.getAttribute(FlairPageSelectors.listItemDropdownButtonActionAttr) as FlairPostListItemDropdownAction;

        switch (attrValue)
        {
            case FlairPostListItemDropdownAction.Delete:
                await this.deleteFlair(e);
                break;

            case FlairPostListItemDropdownAction.Edit:
                await this.editFlair(e);
                break;

            default:
                throw new Error(`Unknown action: ${attrValue}`);
        }
    }

    private async editFlair(e: Element)
    {
        const listItem = new FlairListItem(e);
        listItem.edit();
    }

    private async deleteFlair(e: Element)
    {
        const listItem = new FlairListItem(e);

        const confirmMessageBox = new MessageBoxConfirm('Are you sure you want to delete this flair?');

        confirmMessageBox.confirm({
            onSuccess: async () =>
            {
                await this.sendDeleteRequest(listItem);
            },
        });

    }


    private async sendDeleteRequest(listItem: FlairListItem): Promise<boolean>
    {
        try
        {
            const flairId = listItem.flairId;

            const response = await this._flairService.deleteFlair(flairId);

            if (!response.successful)
            {
                AlertUtility.showErrors({
                    container: this._alertsContainer,
                    errors: response.response.errors,
                });

                return false;
            }

            listItem.remove();

            this.showSuccessfulAlert('Flair successfully deleted.');

            return true;
        }
        catch (error)
        {
            ErrorUtility.onException(error, {
                onApiForbiddenException: (e) => this.showErrorAlert('You do not have permission to delete this flair.'),
                onApiNotFoundException: (e)  => this.showErrorAlert('We could not find the specified flair. Please refresh the page and try again.'),
                onOther: (e)                 => this.showErrorAlert('Unexpected error. Please try again later.'),
            });

            return false;
        }
    }


    private showErrorAlert(message: string)
    {
        AlertUtility.showDanger({
            container: this._alertsContainer,
            message: message,
        });
    }

    private showSuccessfulAlert(message: string)
    {
        AlertUtility.showSuccess({
            container: this._alertsContainer,
            message: message,
        });
    }



}