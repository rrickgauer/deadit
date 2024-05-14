import { LoginModal } from "../../../components/login-modal/login-modal";
import { IController } from "../../../domain/contracts/i-controller";
import { ILoginModalPage } from "../../../domain/contracts/ilogin-modal";
import { PostPageParms } from "../../../domain/model/post-models";
import { CommentsService } from "../../../services/comments-service";
import { MessageBoxUtility } from "../../../utilities/message-box-utility";
import { CommentsController } from "./comments-controller";
import { RootCommentFormController } from "./root-comment-form-controller";

export class PostPageController implements IController, ILoginModalPage
{
    public readonly _modal = new LoginModal(true);

    private _args: PostPageParms;
    private _isLoggedIn = false;
    private _commentsController: CommentsController | null;
    private _rootCommentController: RootCommentFormController;

    constructor(args: PostPageParms)
    {
        this._args = args;
    }

    public control = async () =>
    {
        await this.initCommentsController();
        this._commentsController.control();

        this._rootCommentController = new RootCommentFormController({
            isLoggedIn: this._isLoggedIn,
            postPageArgs: this._args,
        });

        await this._rootCommentController.control();


        this.addListeners();
    }

    private addListeners = () =>
    {

    }


    private async initCommentsController()
    {
        try
        {
            const service = new CommentsService(this._args);
            const response = await service.getAllComments();

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

        }
        catch (error)
        {
            this.onGetCommentsDataError(error);
            return;
        }
    }

    private onGetCommentsDataError(error: Error)
    {
        console.error(error);

        MessageBoxUtility.showError({
            message: 'There was an unexpected error fetching post comments',
        });
    }

}