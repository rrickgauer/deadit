import { PageUtility } from "../../../utilities/page-utility";
import { UrlUtility } from "../../../utilities/url-utility";
import { PostPageController } from "./post-page-controller";


PageUtility.pageReady(() =>
{
    const controller = new PostPageController({
        communityName: UrlUtility.getCurrentPathValue(1),
        postId: UrlUtility.getCurrentPathValue(3),
    });

    controller.control();

});