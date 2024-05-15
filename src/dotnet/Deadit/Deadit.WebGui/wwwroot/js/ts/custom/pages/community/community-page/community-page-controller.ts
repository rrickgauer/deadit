import { LoginModal } from "../../../components/login-modal/login-modal";
import { IControllerAsync } from "../../../domain/contracts/i-controller";
import { PostListController } from "./post-list-controller";
import { ToggleMembershipController } from "./toggle-membership-controller";


export class CommunityPageController implements IControllerAsync
{
    private readonly _modal = new LoginModal(true);
    private readonly _toggleMembershipController = new ToggleMembershipController();
    private _postListController: PostListController;


    constructor()
    {
        this._postListController = new PostListController({

        });

    }

    public async control()
    {
        await this._postListController.control();
        this._toggleMembershipController.control();
    }
}
