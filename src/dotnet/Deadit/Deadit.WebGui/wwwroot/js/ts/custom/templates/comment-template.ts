import { CommentApiResponse } from "../domain/model/comment-models";
import { HtmlString } from "../domain/types/aliases";
import { GuidUtility } from "../utilities/guid-utility";
import { MarkdownUtility } from "../utilities/markdown-utility";
import { CommentFormTemplate } from "./comment-form-template";
import { HtmlTemplate } from "./html-template";


export class CommentTemplate extends HtmlTemplate<CommentApiResponse>
{

    public toHtml = (model: CommentApiResponse): HtmlString =>
    {
        let replies = '';

        if (model.commentReplies.length > 0)
        {
            replies = this.toHtmls(model.commentReplies);
        }

        const editActionButtons = `
            &#183;
            <a href="#" class="text-reset btn-comment-action btn-comment-action-edit">Edit</a>
            &#183;
            <a href="#" class="text-reset btn-comment-action btn-comment-action-delete">Delete</a>
        `;


        const formTemplate = new CommentFormTemplate();
        const editCommentForm = formTemplate.toHtml(model);


        const newCommentForm = formTemplate.toHtml({
            commentId: GuidUtility.getRandomGuid(),
            commentParentId: model.commentId,
            commentIsAuthor: true,
            
        });






        let html = `

            <hr />
            <li class="comment-list-item ${model.commentIsAuthor ? 'comment-list-item-authored' : ''}" data-comment-id="${model.commentId}">

                <div class="comment-actions">
                    <small class="text-muted">
                        <span>${model.commentAuthorUsername}</span> &#183;
                        <span>${model.createdOnDifferenceDisplay} ago</span> &#183;
                        <a href="#" class="text-reset btn-comment-action btn-comment-action-toggle">Hide</a> &#183;
                        <a href="#" class="text-reset btn-comment-action btn-comment-action-reply">Reply</a>
                        ${model.commentIsAuthor ? editActionButtons : ''}
                    </small>
                </div>

                <div class="form-post-comment-edit-container">
                    ${model.commentIsAuthor ? editCommentForm : ''}
                </div>
                
                <div class="comment-thread">
                    <div class="comment-content">
                        <div class="comment-content-md">
                            <div class="md">
                                ${MarkdownUtility.toHtml(model.commentContent)}
                            </div>
                        </div>

                        <div class="form-post-comment-new-container d-none">
                            ${newCommentForm}
                        </div>
                    </div>


                    <ul class="comment-list" data-parent-comment-id="${model.commentParentId}">
                        ${replies}
                    </ul>
                </div>

            </li>
        `;





        return html;
    }
}