
import { PageUtilities } from "../../utilities/page-utilities";
import { LoginController } from "./login-controller";


PageUtilities.pageReady(() =>
{
    const controller = new LoginController();
    controller.control();
});





