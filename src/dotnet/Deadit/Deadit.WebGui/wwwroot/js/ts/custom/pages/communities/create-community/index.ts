import { LoginModal } from "../../../components/login-modal/login-modal";
import { PageUtility } from "../../../utilities/page-utility";
import { CreateCommunityController } from "./create-community-controller";



/**
 * Main page logic
 */
PageUtility.pageReady(async () =>
{
    const loginModal = new LoginModal();
    run();
});

function run()
{
    const controller = new CreateCommunityController();
    controller.control();
}



