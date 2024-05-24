import { BootstrapUtilityClasses } from "../../../domain/constants/bootstrap-constants";
import { Icons } from "../../../domain/constants/icons";
import { IVoted } from "../../../domain/contracts/ivoted";
import { VoteType } from "../../../domain/enum/vote-type";
import { VoteScore } from "../../../domain/helpers/vote-scores/vote-score";
import { CommentApiResponse } from "../../../domain/model/comment-models";
import { Guid } from "../../../domain/types/aliases";
import { CommentFormTemplate } from "../../../templates/comment-form-template";
import { CommentTemplate } from "../../../templates/comment-template";
import { GuidUtility } from "../../../utilities/guid-utility";
import { MarkdownUtility } from "../../../utilities/markdown-utility";
import { Nullable } from "../../../utilities/nullable";


export const CommentSelectors = {
    commentIdAttr: 'data-comment-id',
    parentIdAttr: 'data-parent-comment-id',
    replyingClass: 'replying',
    editingClass: 'editing',
    listItemClass: 'comment-list-item',
    editFormContainerClass: 'form-post-comment-edit-container',
    newFormContainerClass: 'form-post-comment-new-container',
    contentMdClass: 'comment-content-md',
    authorUsernameClass: 'comment-author-username',
    actionsContainerClass: 'comment-actions',
    btnToggleClass: 'btn-comment-action-toggle',
    collapsedClass: 'collapsed',
    lockedClass: 'locked',
    lockedDisplay: 'comment-list-item-locked',
}

export class CommentListItem implements IVoted
{
    public container: HTMLLIElement;
    private _editFormContainer: HTMLDivElement;
    private _newFormContainer: HTMLDivElement;
    private _contentMdDisplay: HTMLDivElement;
    private _repliesList: HTMLUListElement;
    private _actionsContainer: HTMLDivElement;
    private _toggleBtn: HTMLAnchorElement;
    private _voting: VoteScore;
    private _lockedDisplay: HTMLDivElement;

    public get commentId(): Guid | null
    {
        return this.container.getAttribute(CommentSelectors.commentIdAttr);
    }

    public set commentId(value: Guid)
    {
        this.container.setAttribute(CommentSelectors.commentIdAttr, value);
    } 

    public get parentCommentId(): Guid | null
    {
        return this.container.getAttribute(CommentSelectors.parentIdAttr);
    }

    public set parentCommentId(value: Guid)
    {
        this.container.setAttribute(CommentSelectors.parentIdAttr, value);
    } 


    public get isReplying(): boolean
    {
        return this.container.classList.contains(CommentSelectors.replyingClass);
    }

    public set isReplying(value: boolean)
    {
        if (value)
        {
            this.container.classList.add(CommentSelectors.replyingClass);
        }
        else
        {
            this.container.classList.remove(CommentSelectors.replyingClass);
        }
    }

    public get authorUsername(): string
    {
        return (this.container.querySelector(`.${CommentSelectors.authorUsernameClass}`) as HTMLSpanElement).innerText;
    }

    public get isCollapsed(): boolean
    {
        return this.container?.classList.contains(CommentSelectors.collapsedClass) ?? false;
    }

    public set isCollapsed(value: boolean)
    {
        if (!Nullable.hasValue(this.container))
        {
            return;
        }

        if (value)
        {
            this.container.classList.add(CommentSelectors.collapsedClass);
            this._toggleBtn.innerText = 'Show';

        }
        else
        {
            this.container.classList.remove(CommentSelectors.collapsedClass);
            this._toggleBtn.innerText = 'Hide';
        }
    }

    public get isLocked(): boolean
    {
        return this.container?.classList.contains(CommentSelectors.lockedClass) ?? false;
    }

    public set isLocked(value: boolean)
    {
        if (value)
        {
            this.container?.classList.add(CommentSelectors.lockedClass);
            this._lockedDisplay.innerHTML = Icons.CommentLocked;
        }
        else
        {
            this.container?.classList.remove(CommentSelectors.lockedClass);
            this._lockedDisplay.innerHTML = '';
        }
    }






    constructor(element: Element)
    {
        this.container = element.closest(`.${CommentSelectors.listItemClass}`) as HTMLLIElement;
        this._editFormContainer = this.container.querySelector(`.${CommentSelectors.editFormContainerClass}`) as HTMLDivElement;
        this._newFormContainer = this.container.querySelector(`.${CommentSelectors.newFormContainerClass}`) as HTMLDivElement;
        this._contentMdDisplay = this.container.querySelector(`.${CommentSelectors.contentMdClass}`) as HTMLDivElement;
        this._repliesList = this.container.querySelector(`.comment-list[data-parent-comment-id="${this.commentId}"]`) as HTMLUListElement;
        this._actionsContainer = this.container?.querySelector(`.${CommentSelectors.actionsContainerClass}`) as HTMLDivElement;
        this._toggleBtn = this.container?.querySelector(`.${CommentSelectors.btnToggleClass}`) as HTMLAnchorElement;
        this._lockedDisplay = this.container.querySelector(`.${CommentSelectors.lockedDisplay}`) as HTMLDivElement;

        this._voting = new VoteScore(this.container.querySelector(`.item-voting`));

    }


    public showEditForm()
    {
        this.container.classList.add(CommentSelectors.editingClass);
    }

    public showReplyForm()
    {
        if (this.isReplying)
        {
            return;
        }

        const templateEngine = new CommentFormTemplate();

        const html = templateEngine.toHtml({
            commentId: GuidUtility.getRandomGuid(),
            isNew: true,
            parentId: this.commentId,
        });


        this._newFormContainer.innerHTML = html;

        this.isReplying = true;

    }

    public cancelReply()
    {
        this._newFormContainer.innerHTML = '';
        this.isReplying = false;
    }

    public cancelEdit()
    {
        this.container.classList.remove(CommentSelectors.editingClass);
    }

    public addReply(reply: CommentApiResponse)
    {
        const template = new CommentTemplate();

        const html = template.toHtml(reply);

        this._repliesList.insertAdjacentHTML("beforeend", html);
        this._newFormContainer.innerHTML = '';
    }

    public contentUpdated(newContent: string)
    {
        this.setContentText(newContent);

        this.cancelEdit();

        const formTemplate = new CommentFormTemplate();

        this._editFormContainer.innerHTML = formTemplate.toHtml({
            commentId: this.commentId,
            isNew: false,
            content: newContent,
            parentId: this.parentCommentId,
        });
    }



    public markDeleted()
    {
        const deletedText = '[deleted]';

        const deletedComment: CommentApiResponse = {
            commentAuthorUsername: deletedText,
            commentContent: deletedText,
            commentIsAuthor: false,
            commentDeletedOn: deletedText,
        };

        const template = new CommentTemplate();
        const buttonsHtml = template.getActionsSection(deletedComment);

        if (Nullable.hasValue(this._actionsContainer))
        {
            this._actionsContainer.innerHTML = buttonsHtml;
        }

        this.setContentText(deletedText);

    }

    public markRemoved()
    {
        const deletedText = '[removed]';

        const deletedComment: CommentApiResponse = {
            commentAuthorUsername: deletedText,
            commentContent: deletedText,
            commentIsAuthor: false,
            commentDeletedOn: deletedText,
        };

        const template = new CommentTemplate();
        const buttonsHtml = template.getActionsSection(deletedComment);

        if (Nullable.hasValue(this._actionsContainer))
        {
            this._actionsContainer.innerHTML = buttonsHtml;
        }

        this.setContentText(deletedText);
    }


    public toggleCollapse()
    {
        this.isCollapsed = !this.isCollapsed;
    }


    public upvoted(): VoteType
    {
        this._voting.upvoted();
        return this._voting.currentVote;

    }

    public downvoted(): VoteType
    {
        this._voting.downvoted();
        return this._voting.currentVote;
    }


    private setContentText(newContent)
    {
        const html = `<div class="md">${MarkdownUtility.toHtml(newContent)}</div>`;
        this._contentMdDisplay.innerHTML = html;
    }





    public static getItemById(commentId: Guid): CommentListItem | null
    {
        let element = document.querySelector(`.${CommentSelectors.listItemClass}[${CommentSelectors.commentIdAttr}="${commentId}"]`) as Element;

        if (!Nullable.hasValue(element))
        {
            return null;
        }

        return new CommentListItem(element);
    }
}