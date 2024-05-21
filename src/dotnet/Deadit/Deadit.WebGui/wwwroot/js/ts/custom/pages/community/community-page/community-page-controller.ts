import { LoginModal } from "../../../components/login-modal/login-modal";
import { IControllerAsync } from "../../../domain/contracts/i-controller";
import { TopPostSort } from "../../../domain/enum/top-post-sort";
import { TopPostSortOptionChangedEvent } from "../../../domain/events/events";
import { TopPostSortOptions } from "../../../domain/helpers/top-post-sort/top-post-sort-options";
import { UrlUtility } from "../../../utilities/url-utility";
import { PostListController } from "../../../components/posts-list/post-list-controller";
import { ToggleMembershipController } from "./toggle-membership-controller";


export class CommunityPageController implements IControllerAsync
{
    private readonly _modal = new LoginModal(true);
    private readonly _toggleMembershipController = new ToggleMembershipController();
    private _postListController: PostListController;
    private readonly _communityName = UrlUtility.getCurrentPathValue(1);

    constructor()
    {
        this._postListController = new PostListController();
    }

    public async control()
    {
        await this._postListController.control();
        this._toggleMembershipController.control();

        TopPostSortOptions.initOptions(document.querySelector('.community-page-top-sort-options'));

        this.addListeners();
    }

    private addListeners = () =>
    {
        TopPostSortOptionChangedEvent.addListener((message) =>
        {
            this.sortTopPosts(message.data.sort);
        });
    }

    private sortTopPosts(sort: TopPostSort)
    {
        window.location.href = TopPostSortOptions.getNewUrl(sort).toString();
    }
}
