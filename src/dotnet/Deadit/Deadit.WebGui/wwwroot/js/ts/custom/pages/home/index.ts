
import { LoginModal } from "../../components/login-modal/login-modal";
import { PageUtility } from "../../utilities/page-utility";
import { HomePageController } from "./home-page-controller";


PageUtility.pageReady(async () => {

    const controller = new HomePageController();
    await controller.control();

});
