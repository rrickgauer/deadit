import { LoginModal } from "../../../components/login-modal/login-modal";
import { IController } from "../../../domain/contracts/i-controller";
import { ILoginModalPage } from "../../../domain/contracts/ilogin-modal";
import { ItemsSortInput } from "../../../domain/helpers/items-sort/items-sort";
import { PostPageParms } from "../../../domain/model/post-models";
import { CommentsService } from "../../../services/comments-service";
import { MessageBoxUtility } from "../../../utilities/message-box-utility";
import { PageLoadingUtility } from "../../../utilities/page-loading-utility";
import { CommentsController } from "./comments-controller";
import { PostContent } from "./post-content";
import { RootCommentFormController } from "./root-comment-form-controller";

export class PostPageController implements IController, ILoginModalPage
{
    public readonly _modal = new LoginModal(true);

    private _args: PostPageParms;
    private _isLoggedIn = false;
    private _commentsController: CommentsController | null;
    private _rootCommentController: RootCommentFormController;
    private readonly _sortInput: ItemsSortInput;
    private _postContentController: PostContent;
    private _postIsDeleted: boolean;

    constructor(args: PostPageParms)
    {
        this._args = args;
        this._sortInput = new ItemsSortInput(document.querySelector('.items-sort'));
    }

    public control = async () =>
    {
        await this.initCommentsController();
        
        await this.initRootCommentFormController();

        await this.initPostContentController();

        this.addListeners();

        this.showPageContent();
    }




    private addListeners = () =>
    {

    }


    private async initCommentsController()
    {
        try
        {
            const service = new CommentsService(this._args);
            const response = await service.getAllComments({
                sort: this._sortInput.selectedOption,
            });

            if (!response.successful)
            {
                MessageBoxUtility.showErrorList(response.response.errors);
                return;
            }

            this._commentsController = new CommentsController({
                getCommentsResponse: response.response.data,
                postPageArgs: this._args,
            });

            this._isLoggedIn = response.response.data.isLoggedIn;
            this._postIsDeleted = response.response.data.postIsDeleted;

            this._commentsController.control();

        }
        catch (error)
        {
            this.onGetCommentsDataError(error);
            return;
        }
    }

    private async initRootCommentFormController()
    {
        this._rootCommentController = new RootCommentFormController({
            isLoggedIn: this._isLoggedIn,
            postPageArgs: this._args,
        });

        await this._rootCommentController.control();
    }

    private async initPostContentController()
    {
        this._postContentController = new PostContent({
            communityName: this._args.communityName,
            postId: this._args.postId,
            isLoggedIn: this._isLoggedIn,
            postIsDeleted: this._postIsDeleted,
        });

        await this._postContentController.control();
    }

    private onGetCommentsDataError(error: Error)
    {
        console.error(error);

        MessageBoxUtility.showError({
            message: 'There was an unexpected error fetching post comments',
        });
    }

    private showPageContent()
    {
        PageLoadingUtility.hideLoader();   
    }

}