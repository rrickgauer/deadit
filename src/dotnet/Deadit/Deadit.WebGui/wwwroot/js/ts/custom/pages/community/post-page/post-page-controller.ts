import { LoginModal } from "../../../components/login-modal/login-modal";
import { IController } from "../../../domain/contracts/i-controller";
import { ILoginModalPage } from "../../../domain/contracts/ilogin-modal";
import { ItemsSortInput } from "../../../domain/helpers/items-sort/items-sort";
import { GetPostPageApiResponse, PostPageParms } from "../../../domain/model/post-models";
import { PostService } from "../../../services/post-service";
import { ErrorUtility } from "../../../utilities/error-utility";
import { MessageBoxUtility } from "../../../utilities/message-box-utility";
import { PageLoadingUtility } from "../../../utilities/page-loading-utility";
import { CommentsController } from "./comments-controller";
import { PostContent } from "./post-content";
import { RootCommentFormController } from "./root-comment-form-controller";

export class PostPageController implements IController, ILoginModalPage
{
    public readonly _modal = new LoginModal(true);

    private _args: PostPageParms;
    private _commentsController: CommentsController | null;
    private _rootCommentController: RootCommentFormController;
    private readonly _sortInput: ItemsSortInput;
    private _postContentController: PostContent;
    private _postService: PostService;
    

    constructor(args: PostPageParms)
    {
        this._args = args;
        this._sortInput = new ItemsSortInput(document.querySelector('.items-sort'));

        this._postService = new PostService();
    }

    public control = async () =>
    {
        const getPostDataResponse = await this.fetchPostData();

        if (!getPostDataResponse.successful)
        {
            MessageBoxUtility.showServiceResponseErrors(getPostDataResponse);
            return;
        }

        const postData = getPostDataResponse.response.data;

        this.initComments(postData);
        this.initRootCommentController(postData);
        this.initPostContent(postData);

        this.showPageContent();
    }



    private async fetchPostData()
    {

        try
        {
            const response = await this._postService.getPost({
                commentsSort: this._sortInput.selectedOption,
                postId: this._args.postId,
            });

            return response;
        }
        catch (error)
        {
            console.error({ error });

            ErrorUtility.onException(error, {
                onApiNotFoundException: (e) =>
                {
                    MessageBoxUtility.showError({
                        message: 'Post not found. Try refreshing the page.',
                    });
                },

                onOther: (e) =>
                {
                    MessageBoxUtility.showError({
                        message: 'There was an unexpected error loading the post data. Try refreshing the page',
                    });
                }
            });

            throw error;
        }


    }



    private initComments(data: GetPostPageApiResponse)
    {
        this._commentsController = new CommentsController({
            data: data,
            pageArgs: this._args,
        });

        this._commentsController.control();
    }

    private initRootCommentController(data: GetPostPageApiResponse)
    {
        this._rootCommentController = new RootCommentFormController({
            isLoggedIn: data.isLoggedIn,
            postPageArgs: this._args,
        });

        this._rootCommentController.control();
    }


    private initPostContent(data: GetPostPageApiResponse)
    {
        this._postContentController = new PostContent({
            communityName: this._args.communityName,
            postId: this._args.postId,
            isLoggedIn: data.isLoggedIn,
            postIsDeleted: data.postIsDeleted,
        });

        this._postContentController.control();
    }

    private showPageContent()
    {
        PageLoadingUtility.hideLoader();   
    }

}