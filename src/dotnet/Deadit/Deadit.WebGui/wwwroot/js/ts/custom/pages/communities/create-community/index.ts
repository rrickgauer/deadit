import { LoginModal } from "../../../components/login-modal/login-modal";
import { PageUtilities } from "../../../utilities/page-utilities";
import { CreateCommunityController } from "./create-community-controller";



/**
 * Main page logic
 */
PageUtilities.pageReady(async () =>
{
    const loginModal = new LoginModal();
    run();
});

function run()
{
    const controller = new CreateCommunityController();
    controller.control();
}



