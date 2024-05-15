
import { PageUtility } from "../../../utilities/page-utility";
import { CommunityPageController } from "./community-page-controller";


PageUtility.pageReady(async () =>
{

    const controller = new CommunityPageController();
    controller.control();
    
});
