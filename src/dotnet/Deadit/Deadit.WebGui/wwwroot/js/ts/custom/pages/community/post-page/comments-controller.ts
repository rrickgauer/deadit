import { NativeEvents } from "../../../domain/constants/native-events";
import { IControllerAsync } from "../../../domain/contracts/i-controller";
import { RootCommentFormSubmittedEvent } from "../../../domain/events/events";
import { MessageBoxConfirm } from "../../../domain/helpers/message-box/MessageBoxConfirm";
import { CommentApiResponse, GetCommentsApiResponse, SaveCommentRequest } from "../../../domain/model/comment-models";
import { PostPageParms } from "../../../domain/model/post-models";
import { CommentsService } from "../../../services/comments-service";
import { CommentTemplate } from "../../../templates/comment-template";
import { ErrorUtility } from "../../../utilities/error-utility";
import { MessageBoxUtility } from "../../../utilities/message-box-utility";
import { CommentForm } from "./comment-form";
import { CommentListItem } from "./comment-list-item";


export type CommentsControllerArgs = {
    getCommentsResponse: GetCommentsApiResponse;
    postPageArgs: PostPageParms;
}


export enum CommentActionButtons
{
    EDIT = 'btn-comment-action-edit',
    DELETE = 'btn-comment-action-delete',
    TOGGLE = 'btn-comment-action-toggle',
    REPLY = 'btn-comment-action-reply',
}


export class CommentsController implements IControllerAsync
{
    private readonly _args: PostPageParms;

    private readonly _isLoggedIn: boolean;

    private readonly _templateEngine: CommentTemplate;
    private readonly _rootListElement: HTMLUListElement;

    private readonly _commentService: CommentsService;

    private _comments: CommentApiResponse[];

    constructor(args: CommentsControllerArgs)
    {
        this._args = args.postPageArgs;
        this._comments = args.getCommentsResponse.comments;
        this._isLoggedIn = args.getCommentsResponse.isLoggedIn;
        this._commentService = new CommentsService(this._args);
        this._templateEngine = new CommentTemplate();
        this._rootListElement = document.querySelector('.comment-list.root') as HTMLUListElement;
    }


    public async control()
    {
        this.initCommentsListHtml();
        this.addListeners();
    }

    private initCommentsListHtml()
    {
        const html = this._templateEngine.toHtmls(this._comments);
        this._rootListElement.innerHTML = html;
    }

    private addListeners = () =>
    {
        // handle action buttons
        this._rootListElement.addEventListener(NativeEvents.Click, async (e) =>
        {
            let target = e.target as Element;

            if (target.classList.contains('btn-comment-action'))
            {
                e.preventDefault();
                await this.onBtnCommentActionClick(target as HTMLAnchorElement);
            }
        });


        this._rootListElement.addEventListener(NativeEvents.Click, (e) =>
        {
            let target = e.target as Element;

            if (target.classList.contains('btn-form-post-comment-cancel'))
            {
                this.onFormPostCommentBtnCancelClick(target);
            }

        });

        this._rootListElement.addEventListener(NativeEvents.Submit, async (e) =>
        {
            let target = e.target as Element;

            if (target.classList.contains('form-post-comment'))
            {
                e.preventDefault();
                await this.onCommentFormSubmitted(target);
            }
        });


        RootCommentFormSubmittedEvent.addListener((message) =>
        {
            const newComment = message.data.comment;
            const html = this._templateEngine.toHtml(newComment);
            this._rootListElement.insertAdjacentHTML("afterbegin", html);
        });
    }

    private onBtnCommentActionClick = async (button: HTMLAnchorElement) =>
    {
        const listItem = new CommentListItem(button);

        if (button.classList.contains(CommentActionButtons.EDIT))
        {
            if (this.isAuth())
            {
                listItem.showEditForm();
            }
        }

        else if (button.classList.contains(CommentActionButtons.DELETE))
        {
            if (this.isAuth())
            {
                await this.confirmDeleteComment(listItem);
            }
        }

        else if (button.classList.contains(CommentActionButtons.TOGGLE))
        {
            listItem.toggleCollapse();
        }

        else if (button.classList.contains(CommentActionButtons.REPLY))
        {
            if (this.isAuth())
            {
                listItem.showReplyForm();
            }
        }
        else
        {
            alert('unknown action button');
        }
    }


    private onFormPostCommentBtnCancelClick(cancelButton: Element)
    {
        const listItem = new CommentListItem(cancelButton);

        const parentForm = cancelButton.closest('.form-post-comment') as HTMLFormElement;

        if (parentForm.classList.contains('form-post-comment-new'))
        {
            listItem.cancelReply();
        }
        else
        {
            listItem.cancelEdit();
        }
    }


    private async onCommentFormSubmitted(target)
    {
        const form = new CommentForm(target);
        const listItem = form.getParentListItem();

        const savedSuccessfully = await this.saveComment(form);

        if (!savedSuccessfully)
        {
            return;
        }

        if (form.isNew)
        {
            listItem.addReply(form.commentId, form.contentValue);
        }
        else 
        {
            listItem.contentUpdated(form.contentValue);
        }

    }


    private async saveComment(form: CommentForm): Promise<boolean>
    {
        try
        {
            const saveResult = await this._commentService.saveComment(new SaveCommentRequest(form.commentId, {
                content: form.contentValue,
                parentId: form.parentCommentId,
            }));


            console.log({ d: saveResult.response.data });

            return saveResult.successful;

        }
        catch (error)
        {
            return false;
        }
    }


    private confirmDeleteComment = async (listItem: CommentListItem) =>
    {
        const confirmMessageBox = new MessageBoxConfirm('Are you sure you want to permanently delete this comment?', 'Delete Comment');

        confirmMessageBox.confirm({
            onSuccess: async () =>
            {
                await this.deleteComment(listItem);
            },
        });
    }

    private async deleteComment(listItem: CommentListItem)
    {
        const commentId = listItem.commentId;

        try
        {
            const response = await this._commentService.deleteComment(commentId);

            if (!response.successful)
            {
                MessageBoxUtility.showErrorList(response.response.errors);
                return;
            }

            listItem.markDeleted();
            return;
        }
        catch (error)
        {
            ErrorUtility.onException(error, {
                onApiNotFoundException: () =>
                {
                    MessageBoxUtility.showError({
                        message: 'Comment does not exist',
                    });
                },
                onOther: (e) =>
                {
                    console.error(e);
                    MessageBoxUtility.showError({
                        message: 'Unexpected error deleting your comment.',
                    });
                },
            });
        }

    }




    private isAuth(): boolean
    {
        if (!this._isLoggedIn)
        {
            MessageBoxUtility.showStandard({
                message: 'Please sign in or create an account to continue',
                title: 'Log in required',
            });

            return false;
        }

        return true;
    }







}