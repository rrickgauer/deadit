import { NativeEvents } from "../../../domain/constants/native-events";
import { IController } from "../../../domain/contracts/i-controller";
import { CommunityMembershipService } from "../../../services/community-membership-service";
import { UrlUtility } from "../../../utilities/url-utility";
import { CommunityPageElements } from "./community-page-elements";


export class ToggleMembershipController implements IController
{
    private static readonly IsMemberDataAttribute = 'data-js-is-member';

    private readonly _elements = new CommunityPageElements();
    private readonly _communityMembershipService = new CommunityMembershipService();
    private readonly _communityName = UrlUtility.getCurrentPathValue(1) as string;

    public control = () =>
    {
        this.addListeners();
    }

    private addListeners = () =>
    {
        this._elements.btnToggleMembership.button?.addEventListener(NativeEvents.Click, this.onToggleMembershipButtonClick);
    }


    private onToggleMembershipButtonClick = async (e: MouseEvent) =>
    {
        if (this.isMember())
        {
            await this.leaveCommunity();
        }
        else
        {
            await this.joinCommunity();
        }
    }

    /**
     * Leave the community
     */
    private leaveCommunity = async () =>
    {
        this._elements.btnToggleMembership.spin();

        try
        {
            const serviceResponse = await this._communityMembershipService.leaveCommunity(this._communityName);

            if (serviceResponse.successful)
            {
                window.location.href = window.location.href;
            }
            else
            {
                alert('not successful');
            }

            this._elements.btnToggleMembership.reset();
        }
        catch (error)
        {
            alert('Unknown error');
            console.error(error);
            this._elements.btnToggleMembership.reset();
            throw error;
        }
    }

    /**
     * Join the community
     */
    private joinCommunity = async () =>
    {
        this._elements.btnToggleMembership.spin();

        try
        {
            const serviceResponse = await this._communityMembershipService.joinCommunity(this._communityName);

            if (serviceResponse.successful)
            {
                window.location.href = window.location.href;
            }
            else
            {
                alert('not successful');
            }

            this._elements.btnToggleMembership.reset();
        }
        catch (error)
        {
            this._elements.btnToggleMembership.reset();
            console.log({ error });
            alert('Unknow error');
        }
        finally
        {
            this._elements.btnToggleMembership.reset();
        }
    }

    /**
     * Checks if the the user has joined the current community
     * @returns
     */
    private isMember = () =>
    {
        return this._elements.btnToggleMembership.button.hasAttribute(ToggleMembershipController.IsMemberDataAttribute);
    }
}

