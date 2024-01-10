import { LoginModal } from "../../../components/login-modal/login-modal";
import { NativeEvents } from "../../../domain/constants/native-events";
import { SpinnerButton } from "../../../domain/helpers/spinner-button";
import { CommunityMembershipService } from "../../../services/community-membership-service";
import { ErrorService } from "../../../services/error-service";
import { UrlUtilities } from "../../../utilities/url-utilities";



export class CommunityPageElements
{
    public btnToggleMembership = new SpinnerButton(document.querySelector('.btn-toggle-membership'));
}

export class CommunityPageController
{
    private static readonly IsMemberDataAttribute = 'data-js-is-member';

    private readonly _modal = new LoginModal(true);
    private readonly _elements = new CommunityPageElements();
    private readonly _communityMembershipService = new CommunityMembershipService();
    private readonly _communityName = UrlUtilities.getCurrentPathValue(1) as string;
    

    constructor()
    {
        
    }

    public control = () =>
    {
        this.addListeners();
    }

    private addListeners = () =>
    {
        this._elements.btnToggleMembership.button.addEventListener(NativeEvents.Click, this.onToggleMembershipButtonClick);
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
                    this._elements.btnToggleMembership.reset();
                    throw error;
                }
            });
        }
        finally
        {
            this._elements.btnToggleMembership.reset();
        }

    }

    private joinCommunity = async () =>
    {

    }


    /**
     * Checks if the the user has joined the current community
     * @returns
     */
    private isMember = () =>
    {
        return this._elements.btnToggleMembership.button.hasAttribute(CommunityPageController.IsMemberDataAttribute);
    }
}

