import { PageUtility } from "../../../utilities/page-utility";
import { PrivateCommunityPageController } from "./private-community-page-controller";

PageUtility.pageReady(() =>
{
    const controller = new PrivateCommunityPageController();
    controller.control();
});