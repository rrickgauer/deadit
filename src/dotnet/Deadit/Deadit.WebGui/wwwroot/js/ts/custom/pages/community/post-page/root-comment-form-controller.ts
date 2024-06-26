import { LoginModal } from "../../../components/login-modal/login-modal";
import { NativeEvents } from "../../../domain/constants/native-events";
import { IController, IControllerAsync } from "../../../domain/contracts/i-controller";
import { RootCommentFormSubmittedEvent } from "../../../domain/events/events";
import { SaveCommentRequest } from "../../../domain/model/comment-models";
import { PostPageParms } from "../../../domain/model/post-models";
import { CommentsService } from "../../../services/comments-service";
import { ErrorUtility } from "../../../utilities/error-utility";
import { GuidUtility } from "../../../utilities/guid-utility";
import { MessageBoxUtility } from "../../../utilities/message-box-utility";
import { CommentForm } from "./comment-form";



export type RootCommentFormControllerArgs = {
    postPageArgs: PostPageParms;
    isLoggedIn: boolean;
}


export class RootCommentFormController implements IController
{
    postPageArgs: PostPageParms;
    private readonly _isLoggedIn: boolean;
    private _commentService: CommentsService;

    constructor(args: RootCommentFormControllerArgs)
    {
        this._isLoggedIn = args.isLoggedIn;
        this.postPageArgs = args.postPageArgs;

        this._commentService = new CommentsService();
    }

    public control()
    {
        this.addListeners();
    }

    private addListeners = () =>
    {
        document.querySelector('.btn-root-form-login')?.addEventListener(NativeEvents.Click, (e) =>
        {
            LoginModal.ShowModal();
        });


        document.querySelector('.form-post-comment.root')?.addEventListener(NativeEvents.Submit, async (e) =>
        {
            e.preventDefault();

            const form = new CommentForm(e.target as Element);
            form.commentId = GuidUtility.getRandomGuid();

            await this.onFormSubmitted(form);
        });
    }


    private onFormSubmitted = async (form: CommentForm) =>
    {
        if (form.isInvalid())
        {
            return;
        }


        form.showLoading();

        try
        {

            const response = await this._commentService.saveComment(new SaveCommentRequest(form.commentId, {
                content: form.contentValue,
                parentId: null,
                postId: this.postPageArgs.postId,
            }));

            if (!response.successful)
            {
                form.showErrors(response.response.errors);
                form.showNormal();
                return;
            }


            RootCommentFormSubmittedEvent.invoke(this, {
                comment: response.response.data,
            });

            form.showNormal();
            form.contentInput.inputElement.value = '';

            form.displaySuccessfulAlert('Your comment was created!');
        }
        catch (error)
        {

            ErrorUtility.onException(error, {
                onApiValidationException: (e) =>
                {
                    console.log({ e });
                },
                onOther: (e) =>
                {
                    MessageBoxUtility.showError({
                        message: 'There was an error saving your comment',
                    });
                },
            });

            form.showNormal();
            return;
        }
    }


}