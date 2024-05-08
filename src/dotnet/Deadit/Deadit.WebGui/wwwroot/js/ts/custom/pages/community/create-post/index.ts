import { PageUtilities } from "../../../utilities/page-utilities";
import { CreatePostController } from "./create-post-controller";

/**
 * Main logic
 */
PageUtilities.pageReady(async () =>
{
    const controller = new CreatePostController();
    controller.control();
});



