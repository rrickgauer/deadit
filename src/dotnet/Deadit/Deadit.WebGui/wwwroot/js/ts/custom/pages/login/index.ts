
import { PageUtility } from "../../utilities/page-utility";
import { LoginController } from "./login-controller";


PageUtility.pageReady(() =>
{
    const controller = new LoginController();
    controller.control();
});





