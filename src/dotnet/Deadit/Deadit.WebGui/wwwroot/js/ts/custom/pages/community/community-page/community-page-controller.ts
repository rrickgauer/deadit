import { LoginModal } from "../../../components/login-modal/login-modal";
import { IController } from "../../../domain/contracts/i-controller";
import { ToggleMembershipController } from "./toggle-membership-controller";


export class CommunityPageController implements IController
{
    private readonly _modal = new LoginModal(true);
    private readonly _toggleMembershipController = new ToggleMembershipController();


    public control = () =>
    {
        this._toggleMembershipController.control();
    }

}
