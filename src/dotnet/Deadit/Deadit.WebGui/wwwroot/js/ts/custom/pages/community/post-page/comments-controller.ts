import { NativeEvents } from "../../../domain/constants/native-events";
import { IController, IControllerAsync } from "../../../domain/contracts/i-controller";
import { SortOption } from "../../../domain/enum/sort-option";
import { CommentLockedData, CommentLockedEvent, ItemsSortInputChangedEvent, OpenModerateCommentModalEvent, RootCommentFormSubmittedEvent } from "../../../domain/events/events";
import { MessageBoxConfirm } from "../../../domain/helpers/message-box/MessageBoxConfirm";
import { ServiceResponse } from "../../../domain/model/api-response";
import { CommentApiResponse, SaveCommentRequest } from "../../../domain/model/comment-models";
import { GetPostPageApiResponse, PostPageParms } from "../../../domain/model/post-models";
import { Guid } from "../../../domain/types/aliases";
import { CommentsService } from "../../../services/comments-service";
import { VoteService } from "../../../services/vote-service";
import { CommentTemplate } from "../../../templates/comment-template";
import { ErrorUtility } from "../../../utilities/error-utility";
import { MessageBoxUtility } from "../../../utilities/message-box-utility";
import { Nullable } from "../../../utilities/nullable";
import { CommentForm } from "./comment-form";
import { CommentListItem } from "./comment-list-item";
import { CommentModModal } from "./comment-moderation-modal";


export type CommentsControllerArgs = {
    pageArgs: PostPageParms;
    data: GetPostPageApiResponse;
}


export enum CommentActionButtons
{
    EDIT = 'btn-comment-action-edit',
    DELETE = 'btn-comment-action-delete',
    TOGGLE = 'btn-comment-action-toggle',
    REPLY = 'btn-comment-action-reply',
    MODERATE = 'btn-comment-action-moderate',
}


export class CommentsController implements IController
{
    private readonly _args: PostPageParms;
    private readonly _isLoggedIn: boolean;
    private readonly _templateEngine: CommentTemplate;
    private readonly _rootListElement: HTMLUListElement;
    private readonly _commentService: CommentsService;
    private _comments: CommentApiResponse[];
    private _postIsDeleted: boolean;
    private _postPageData: GetPostPageApiResponse;
    private _postIsLocked: boolean;

    constructor(args: CommentsControllerArgs)
    {
        this._args = args.pageArgs;
        this._postPageData = args.data;

        this._comments = this._postPageData.comments;

        this._isLoggedIn = this._postPageData.isLoggedIn;

        this._commentService = new CommentsService();
        this._templateEngine = new CommentTemplate();
        this._rootListElement = document.querySelector('.comment-list.root') as HTMLUListElement;
        this._postIsDeleted = this._postPageData.postIsDeleted;
        this._postIsLocked = this._postPageData.postIsLocked;

        
    }


    public control()
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

        // comment form cancel button click
        this._rootListElement.addEventListener(NativeEvents.Click, (e) =>
        {
            let target = e.target as Element;

            if (target.classList.contains('btn-form-post-comment-cancel'))
            {
                this.onFormPostCommentBtnCancelClick(target);
            }

        });

        // comment form submit
        this._rootListElement.addEventListener(NativeEvents.Submit, async (e) =>
        {
            let target = e.target as Element;

            if (target.classList.contains('form-post-comment'))
            {
                e.preventDefault();
                await this.onCommentFormSubmitted(target);
            }
        });


        // root comment form submitted
        RootCommentFormSubmittedEvent.addListener((message) =>
        {
            const newComment = message.data.comment;
            const html = this._templateEngine.toHtml(newComment);
            this._rootListElement.insertAdjacentHTML("afterbegin", html);
        });


        // comment vote button clicked
        this._rootListElement.addEventListener(NativeEvents.Click, async (e) =>
        {
            let target = e.target as Element;
            let buttonElement = target.closest('.btn-vote') as HTMLButtonElement;

            if (!buttonElement)
            {
                return;
            }

            if (this.isAuth())
            {
                this.onVoteButtonClick(buttonElement);
            }
        });


        // selected comment sort option changed
        ItemsSortInputChangedEvent.addListener((message) =>
        {
            this.sortComments(message.data.selectedValue);
        });

        CommentModModal.init();


        CommentLockedEvent.addListener((message) =>
        {
            this.onCommentLockedEvent(message.data);
        });
    }

    private async onBtnCommentActionClick(button: HTMLAnchorElement)
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

            if (!this.isAuth())
            {
                return;
            }

            if (this.checkPostLocked())
            {
                return;
            }

            if (this.isAuth() && !this._postIsLocked && !listItem.isLocked)
            {
                listItem.showReplyForm();
            }
        }

        else if (button.classList.contains(CommentActionButtons.MODERATE))
        {
            if (this.isAuth())
            {
                OpenModerateCommentModalEvent.invoke(this, {
                    commentId: listItem.commentId,
                });
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

        form.showLoading();

        if (form.isInvalid())
        {
            form.showNormal();
            return;
        }

        const listItem = form.getParentListItem();

        const saveResponse = await this.saveComment(form);

        form.showNormal();

        if (!saveResponse?.successful ?? false)
        {
            return;
        }

        if (form.isNew)
        {
            listItem.addReply(saveResponse.response.data);
        }
        else 
        {
            listItem.contentUpdated(form.contentValue);
        }

    }


    private async saveComment(form: CommentForm): Promise<ServiceResponse<CommentApiResponse>> | null
    {
        try
        {
            const saveResult = await this._commentService.saveComment(new SaveCommentRequest(form.commentId, {
                content: form.contentValue,
                parentId: form.parentCommentId,
                postId: this._args.postId,
            }));

            if (!saveResult.successful)
            {
                MessageBoxUtility.showErrorList(saveResult.response.errors);
            }

            return saveResult;

        }
        catch (error)
        {
            ErrorUtility.onException(error, {
                onApiNotFoundException: (e) =>
                {
                    MessageBoxUtility.showError({
                        message: 'Comment not found',
                    });
                },

                onOther: (e) =>
                {
                    MessageBoxUtility.showError({
                        message: 'Your comment was not saved. Please try again later.',
                        title: 'Unexpected Error',
                    });
                },
            });

            return null;
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


    private async onVoteButtonClick(clickedVoteButton: HTMLButtonElement)
    {
        const wasUpvoted = clickedVoteButton.classList.contains('btn-vote-up');
        const listItem = new CommentListItem(clickedVoteButton);

        await this.saveVote(listItem, wasUpvoted);
    }

    private async saveVote(listItem: CommentListItem, wasUpvoted: boolean)
    {
        const newVoteType = wasUpvoted ? listItem.upvoted() : listItem.downvoted();
        const service = new VoteService();

        try
        {
            const response = await service.voteComment({
                commentId: listItem.commentId,
                voteType: newVoteType,
            });

            if (!response.successful)
            {
                MessageBoxUtility.showErrorList(response.response.errors);
                return;
            }
        }
        catch (error)
        {
            console.error({ error });

            MessageBoxUtility.showError({
                message: `There was an unexpected error saving your vote.`,
            });
            return;
        }
    }


    private sortComments(sortType: SortOption)
    {
        const url = new URL(window.location.href);
        url.searchParams.set('sort', sortType.toLowerCase());
        window.location.href = url.toString();
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


    private checkPostLocked(): boolean
    {
        if (this._postIsLocked)
        {
            MessageBoxUtility.showStandard({
                title: 'Locked Post',
                message: 'The post has been locked by a moderator.'
            });

            return true;
        }

        return false;
    }

    private onCommentLockedEvent(message: CommentLockedData)
    {
        const comment = message.comment;

        // get the existing children of the comment
        comment.commentReplies = this.getCommentModel(message.comment.commentId)?.commentReplies ?? [];

        const template = new CommentTemplate();
        const html = template.toHtml(comment);

        let listItem = CommentListItem.getItemById(message.comment.commentId);

        if (Nullable.hasValue(listItem))
        {
            listItem.container.outerHTML = html;
        }
    }


    private getCommentModel(commentId: Guid): CommentApiResponse | null
    {
        for (const comment of this._comments)
        {
            const c = this.searchChildren(commentId, comment);

            if (Nullable.hasValue(c))
            {
                return c;
            }
        }

        return null;
    }

    private searchChildren(commentId: Guid, comment: CommentApiResponse): CommentApiResponse | null
    {
        var childComment = comment.commentReplies.find(c => c.commentId === commentId);

        if (Nullable.hasValue(childComment))
        {
            return childComment;
        }

        if (comment.commentId === commentId)
        {
            return comment;
        }
        else
        {
            return null;
        }
    }




}