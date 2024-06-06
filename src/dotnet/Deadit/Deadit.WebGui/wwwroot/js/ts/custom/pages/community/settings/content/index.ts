import { PageUtility } from "../../../../utilities/page-utility";
import { ContentCommunitySettingsPageController } from "./content-settings-page-controller";


PageUtility.pageReady(() =>
{
    const controller = new ContentCommunitySettingsPageController();
    controller.control();
});