import { LoginModalController } from "../../../components/login-modal/login-modal-controller";
import { PageUtilities } from "../../../utilities/page-utilities";


PageUtilities.pageReady(async () =>
{
    const loginModal = LoginModalController.setupPage();
});



