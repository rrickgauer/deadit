import { LoginModal } from "../../components/login-modal/login-modal";
import { PostListController } from "../../components/posts-list/post-list-controller";
import { IControllerAsync } from "../../domain/contracts/i-controller";
import { TopPostSort } from "../../domain/enum/top-post-sort";
import { TopPostSortOptionChangedEvent } from "../../domain/events/events";
import { TopPostSortOptions } from "../../domain/helpers/top-post-sort/top-post-sort-options";



export class HomePageController implements IControllerAsync
{
    private readonly _loginModal = new LoginModal();
    private _postsList: PostListController;

    constructor()
    {
        this._postsList = new PostListController();
    }

    public async control()
    {
        TopPostSortOptions.initOptions(document.querySelector('.community-page-top-sort-options'));

        await this._postsList.control();

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
