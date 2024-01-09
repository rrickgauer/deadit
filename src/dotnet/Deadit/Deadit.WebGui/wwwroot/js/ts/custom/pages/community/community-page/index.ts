
import { PageUtilities } from "../../../utilities/page-utilities";
import { CommunityPageController } from "./community-page-controller";


PageUtilities.pageReady(async () =>
{

    const controller = new CommunityPageController();
    controller.control();
    
});
