import { PageUtility } from "../../../../utilities/page-utility";
import { GeneralCommunitySettingsPageController } from "./general-settings-page-controller";



PageUtility.pageReady(() =>
{
    const controller = new GeneralCommunitySettingsPageController();
    controller.control();
});