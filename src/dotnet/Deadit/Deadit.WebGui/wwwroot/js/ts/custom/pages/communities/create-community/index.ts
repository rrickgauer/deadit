import { LoginModalController } from "../../../components/login-modal/login-modal-controller";
import { NativeEvents } from "../../../domain/constants/native-events";
import { PageUtilities } from "../../../utilities/page-utilities";
import { CreateCommunityController } from "./create-community-controller";



/**
 * Main page logic
 */
PageUtilities.pageReady(async () =>
{
    const loginModal = LoginModalController.setupPage();
    run();
});

function run()
{
    const controller = new CreateCommunityController();
    controller.control();
}



