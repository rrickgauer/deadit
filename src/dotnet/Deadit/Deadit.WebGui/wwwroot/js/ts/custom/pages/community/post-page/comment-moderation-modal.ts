import { BootstrapModalEvents } from "../../../domain/constants/bootstrap-constants";
import { NativeEvents } from "../../../domain/constants/native-events";
import { CommentLockedEvent, OpenModerateCommentModalEvent } from "../../../domain/events/events";
import { ContentLoader } from "../../../domain/helpers/content-loader/content-loader";
import { SpinnerButton } from "../../../domain/helpers/spinner-button";
import { ModerateCommentRequest } from "../../../domain/model/comment-models";
import { PostPageParms } from "../../../domain/model/post-models";
import { Guid } from "../../../domain/types/aliases";
import { CommentsService } from "../../../services/comments-service";
import { AlertUtility } from "../../../utilities/alert-utility";
import { BootstrapUtility } from "../../../utilities/bootstrap-utility";
import { ErrorUtility } from "../../../utilities/error-utility";
import { MessageBoxUtility } from "../../../utilities/message-box-utility";
import { Nullable } from "../../../utilities/nullable";
import { CommentListItem } from "./comment-list-item";


export const CommentModerationModalElements = {
    containerClass: 'modal-comment-moderation',
    formClass: 'comment-moderation-form',
    checkName: 'comment-moderation-form-check',
    commentIdAttr: 'data-js-comment-id',
    checkInputLockId: 'comment-moderation-form-check-lock',
    checkInputRemoveId: 'comment-moderation-form-check-remove',
    alertsContainerClass: 'comment-moderation-form-feedback',
}


export class CommentModModal
{
    private static _commentsService: CommentsService = null;
    private static readonly _container = document.querySelector(`.${CommentModerationModalElements.containerClass}`) as HTMLDivElement;
    private static readonly _form = this._container?.querySelector(`.${CommentModerationModalElements.formClass}`) as HTMLFormElement;
    private static readonly _submitButton = Nullable.hasValue(this._form) ? new SpinnerButton(this._form.querySelector(`.btn-submit`)) : null;
    private static readonly _contentLoading = new ContentLoader(this._container.querySelector('.content-loader'));
    private static readonly _alertsContainer = this._container?.querySelector(`.${CommentModerationModalElements.alertsContainerClass}`) as HTMLDivElement;

    private static get _modal()
    {
        return BootstrapUtility.getModal(this._container);
    }

    private static get _commentId()
    {
        return this._container.getAttribute(CommentModerationModalElements.commentIdAttr) as Guid;
    }

    private static set _commentId(commentId: Guid)
    {
        this._container.setAttribute(CommentModerationModalElements.commentIdAttr, commentId);
    }


    private static get _isLocked(): boolean
    {
        return (this._form.querySelector(`#${CommentModerationModalElements.checkInputLockId}`) as HTMLInputElement).checked;
    }

    private static set _isLocked(value: boolean)
    {
        (this._form.querySelector(`#${CommentModerationModalElements.checkInputLockId}`) as HTMLInputElement).checked = value;
    }



    private static get _isRemoved(): boolean
    {
        return (this._form.querySelector(`#${CommentModerationModalElements.checkInputRemoveId}`) as HTMLInputElement).checked;
    }

    private static set _isRemoved(value: boolean)
    {
        (this._form.querySelector(`#${CommentModerationModalElements.checkInputRemoveId}`) as HTMLInputElement).checked = value;
    }



    public static init = () =>
    {
        OpenModerateCommentModalEvent.addListener(async (message) =>
        {
            await this.show(message.data.commentId);
        });

        this._commentsService = new CommentsService();

        this._form?.addEventListener(NativeEvents.Submit, async (e) =>
        {
            e.preventDefault();
            await this.onFormSubmit();
        });

        this._container.addEventListener(BootstrapModalEvents.Hidden, (e) =>
        {
            this._alertsContainer.innerHTML = '';
        });
    }



    private static async show(commentId: Guid)
    {
        this._contentLoading.showSpinner();
        this._commentId = commentId;
        this._modal.show();

        try
        {
            const getComment = await this._commentsService.getComment(this._commentId);

            if (!getComment.successful)
            {
                MessageBoxUtility.showErrorList(getComment.response.errors);
                return;
            }

            const comment = getComment.response.data;

            const listItem = CommentListItem.getItemById(this._commentId);
            this._isLocked = listItem.isLocked;
            this._isRemoved = comment.commentIsRemoved;

            this._contentLoading.showContent();
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
                        message: 'Unexpected error. Please try again later',
                    });
                },
            });
            
        }

        
    }

    public static hide()
    {
        this._modal.hide();
    }

    private static async onFormSubmit()
    {
        this._submitButton.spin();
        let listItem = CommentListItem.getItemById(this._commentId);

        if (!Nullable.hasValue(listItem))
        {
            throw new Error(`Could not find comment list item with id: ${this._commentId}`);
        }

        try
        {
            const request = new ModerateCommentRequest(this._commentId, {
                locked: this._isLocked,
                removed: this._isRemoved,
            });

            const response = await this._commentsService.moderateComment(request);

            if (!response.successful)
            {
                AlertUtility.showErrors({
                    container: this._alertsContainer,
                    errors: response.response.errors,
                });

            }
            else
            {
                AlertUtility.showSuccess({
                    container: this._alertsContainer,
                    message: `Changes saved successfully.`,
                });

                CommentLockedEvent.invoke(this, {
                    comment: response.response.data,
                });
            }
        }
        catch (error)
        {
            ErrorUtility.onException(error, {
                onOther: (e) =>
                {
                    AlertUtility.showDanger({
                        container: this._alertsContainer,
                        message: `Unknown error saving your changes. Please try again later.`,
                    });
                },
            });
        }

        this._submitButton.reset();
    }

}