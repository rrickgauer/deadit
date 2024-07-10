import { PageUtility } from "../../../../utilities/page-utility";
import { CommunitySettingsFlairPageController } from "./community-settings-flair-page-controller";



PageUtility.pageReady(async () =>
{
    const controller = new CommunitySettingsFlairPageController();
    await controller.control();
});