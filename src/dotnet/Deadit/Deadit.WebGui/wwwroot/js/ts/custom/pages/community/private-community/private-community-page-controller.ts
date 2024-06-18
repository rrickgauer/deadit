import { LoginModal } from "../../../components/login-modal/login-modal";
import { IController } from "../../../domain/contracts/i-controller";


export class PrivateCommunityPageController implements IController
{

    private _loginModal = new LoginModal();

    public control()
    {
        
    }
}