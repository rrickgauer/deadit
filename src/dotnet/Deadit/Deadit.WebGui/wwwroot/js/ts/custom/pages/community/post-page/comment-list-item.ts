import { Guid } from "../../../domain/types/aliases";
import { CommentFormTemplate } from "../../../templates/comment-form-template";
import { CommentTemplate } from "../../../templates/comment-template";
import { GuidUtility } from "../../../utilities/guid-utility";
import { MarkdownUtility } from "../../../utilities/markdown-utility";


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
}

export class CommentListItem
{
    private _container: HTMLLIElement;
    private _editFormContainer: HTMLDivElement;
    private _newFormContainer: HTMLDivElement;
    private _contentMdDisplay: HTMLDivElement;
    private _repliesList: HTMLUListElement;

    public get commentId(): Guid | null
    {
        return this._container.getAttribute(CommentSelectors.commentIdAttr);
    }

    public set commentId(value: Guid)
    {
        this._container.setAttribute(CommentSelectors.commentIdAttr, value);
    } 

    public get parentCommentId(): Guid | null
    {
        return this._container.getAttribute(CommentSelectors.parentIdAttr);
    }

    public set parentCommentId(value: Guid)
    {
        this._container.setAttribute(CommentSelectors.parentIdAttr, value);
    } 


    public get isReplying(): boolean
    {
        return this._container.classList.contains(CommentSelectors.replyingClass);
    }

    public set isReplying(value: boolean)
    {
        if (value)
        {
            this._container.classList.add(CommentSelectors.replyingClass);
        }
        else
        {
            this._container.classList.remove(CommentSelectors.replyingClass);
        }
    }

    public get authorUsername(): string
    {
        return (this._container.querySelector(`.${CommentSelectors.authorUsernameClass}`) as HTMLSpanElement).innerText;
    }



    constructor(element: Element)
    {
        this._container = element.closest(`.${CommentSelectors.listItemClass}`) as HTMLLIElement;
        this._editFormContainer = this._container.querySelector(`.${CommentSelectors.editFormContainerClass}`) as HTMLDivElement;
        this._newFormContainer = this._container.querySelector(`.${CommentSelectors.newFormContainerClass}`) as HTMLDivElement;
        this._contentMdDisplay = this._container.querySelector(`.${CommentSelectors.contentMdClass}`) as HTMLDivElement;
        this._repliesList = this._container.querySelector(`.comment-list[data-parent-comment-id="${this.commentId}"]`) as HTMLUListElement;
    }


    public showEditForm()
    {
        this._container.classList.add(CommentSelectors.editingClass);
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
        this._container.classList.remove(CommentSelectors.editingClass);
    }


    public addReply(commentId: Guid, content: string)
    {
        const template = new CommentTemplate();

        const html = template.toHtml({
            commentId: commentId,
            commentParentId: this.commentId,
            commentContent: content,
            createdOnDifferenceDisplay: 'seconds',
            commentIsAuthor: true,
            commentAuthorUsername: this.authorUsername,
        });


        this._repliesList.insertAdjacentHTML("beforeend", html);
        this._newFormContainer.innerHTML = '';
    }

    public contentUpdated(newContent: string)
    {
        const html = `<div class="md">${MarkdownUtility.toHtml(newContent)}</div>`;
        this._contentMdDisplay.innerHTML = html;

        this.cancelEdit();


        const formTemplate = new CommentFormTemplate();

        this._editFormContainer.innerHTML = formTemplate.toHtml({
            commentId: this.commentId,
            isNew: false,
            content: newContent,
            parentId: this.parentCommentId,
        });
    }




}