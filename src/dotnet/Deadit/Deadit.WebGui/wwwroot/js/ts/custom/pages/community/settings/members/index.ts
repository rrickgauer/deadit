import { PageUtility } from "../../../../utilities/page-utility";
import { MembersCommunitySettingsPageController } from "./members-settings-page-controller";

PageUtility.pageReady(() =>
{
    const controller = new MembersCommunitySettingsPageController();
    controller.control();
});