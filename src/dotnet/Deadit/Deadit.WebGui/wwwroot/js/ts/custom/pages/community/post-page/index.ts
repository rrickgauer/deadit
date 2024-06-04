import { MessageBoxUtility } from "../../../utilities/message-box-utility";
import { PageUtility } from "../../../utilities/page-utility";
import { UrlUtility } from "../../../utilities/url-utility";
import { PostPageController } from "./post-page-controller";


PageUtility.pageReady(async () =>
{
    try
    {
        const controller = new PostPageController({
            communityName: UrlUtility.getCurrentPathValue(1),
            postId: UrlUtility.getCurrentPathValue(3),
        });

        await controller.control();
    }
    catch (error)
    {
        MessageBoxUtility.showError({
            message: 'Unknown error. Please try again later',
        });

        throw error;
    }
});