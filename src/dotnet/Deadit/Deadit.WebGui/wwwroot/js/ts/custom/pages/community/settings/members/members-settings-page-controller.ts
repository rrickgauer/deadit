import { IController } from "../../../../domain/contracts/i-controller";
import { CommunityMembershipDropdownClickedData, CommunityMembershipDropdownClickedEvent } from "../../../../domain/events/events";
import { UrlUtility } from "../../../../utilities/url-utility";
import { CommunityMembershipListItem } from "./community-membership-list-item";
import { CommunityMembershipDropdownAction } from "../../../../domain/enum/community-membership-dropdown-action";
import { MessageBoxConfirm } from "../../../../domain/helpers/message-box/MessageBoxConfirm";
import { use } from "marked";
import { AlertUtility } from "../../../../utilities/alert-utility";
import { CommunityMembershipService } from "../../../../services/community-membership-service";
import { ErrorUtility } from "../../../../utilities/error-utility";
import { CommunityMembershipSorting } from "./community-membership-sorting";


export class MembersCommunitySettingsPageController implements IController
{
    private readonly _communityName = UrlUtility.getCurrentPathValue(1);
    private _container: HTMLDivElement;
    private _alertsContainer: HTMLDivElement;
    private _list: HTMLUListElement;
    private _membershipService: CommunityMembershipService;

    constructor()
    {
        this._container = document.querySelector<HTMLDivElement>(`.community-members-container`);
        this._alertsContainer = this._container.querySelector<HTMLDivElement>(`.messages-container`);
        this._list = this._container.querySelector<HTMLUListElement>(`.community-members-list`);
        this._membershipService = new CommunityMembershipService();
    }


    public control = () =>
    {
        CommunityMembershipListItem.initListItems(this._list);
        CommunityMembershipSorting.init();

        this.addListeners();
    }

    private addListeners = () =>
    {
        CommunityMembershipDropdownClickedEvent.addListener((message) =>
        {
            this.onCommunityMembershipDropdownClickedEvent(message.data);
        });
    }

    private onCommunityMembershipDropdownClickedEvent(message: CommunityMembershipDropdownClickedData)
    {
        switch (message.action)
        {
            case CommunityMembershipDropdownAction.Remove:
                this.tryRemove(message.username);
                break;

            default:
                alert(`Unknow action: ${message.action}`);
                break;
        }
    }

    private tryRemove(username: string)
    {
        const messageBoxConfirm = new MessageBoxConfirm('Are you sure you want to remove this user?');

        messageBoxConfirm.confirm({
            onSuccess: () =>
            {
                this.removeUser(username);
            },
        });

    }

    private async removeUser(username: string)
    {

        try
        {
            // send delete request to API
            const response = await this._membershipService.removeMember(this._communityName, username);

            // handle any errors
            if (!response.successful)
            {
                AlertUtility.showErrors({
                    container: this._alertsContainer,
                    errors: response.response.errors,
                });

                return;
            }

            // remove the list item from the page
            const listItem = this.getListItem(username);
            listItem.remove();

            // show successful alert
            this.showSuccessfulAlert('User removed successfully.');
        }
        catch (error)
        {
            ErrorUtility.onException((error), {
                onApiNotFoundException: (e) =>
                {
                    this.showErrorAlert('Could not find the user. Try refreshing the page');
                },
                onOther: (e) =>
                {
                    this.showErrorAlert('An unexpected error occured. Please try again later.');
                },
            });

            return;
        }
    }


    private getListItem(username: string)
    {
        return CommunityMembershipListItem.getListItem(this._list, username);
    }

    private showSuccessfulAlert(message: string)
    {
        AlertUtility.showSuccess({
            container: this._alertsContainer,
            message: message,
        });
    }

    private showErrorAlert(message: string)
    {
        AlertUtility.showDanger({
            container: this._alertsContainer,
            message: message,
        });
    }
}