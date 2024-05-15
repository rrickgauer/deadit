import { PageUtility } from "../../../utilities/page-utility";
import { CreatePostController } from "./create-post-controller";

/**
 * Main logic
 */
PageUtility.pageReady(async () =>
{
    const controller = new CreatePostController();
    controller.control();
});



