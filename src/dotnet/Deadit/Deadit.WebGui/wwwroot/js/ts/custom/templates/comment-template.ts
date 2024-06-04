import { CommentApiResponse } from "../domain/model/comment-models";
import { HtmlString } from "../domain/types/aliases";
import { MarkdownUtility } from "../utilities/markdown-utility";
import { HtmlTemplate } from "./html-template";
import { CommentFormTemplate } from "./comment-form-template";
import { Nullable } from "../utilities/nullable";
import { VoteScoreTemplate } from "./votes-score-template";
import { Icons } from "../domain/constants/icons";


export class CommentTemplate extends HtmlTemplate<CommentApiResponse>
{

    public toHtml = (model: CommentApiResponse): HtmlString =>
    {
        let replies = '';

        if (Nullable.getValue(model.commentReplies?.length, 0) > 0)
        {
            replies = this.toHtmls(model.commentReplies);
        }

        const actionButtons = this.getActionsSection(model);

        const formTemplate = new CommentFormTemplate();

        const editCommentForm = formTemplate.toHtml({
            commentId: model.commentId,
            isNew: false,
            parentId: model.commentParentId,
            content: model.commentContent,
        });

        const votingTemplate = new VoteScoreTemplate();
        const voting = votingTemplate.toHtml(model);


        let lockedDisplay = `
            <div class="me-1 comment-list-item-locked">
                ${model.commentIsLocked ? Icons.CommentLocked : ''}
            </div>
            `;

        let html = `

            
            <li class="comment-list-item ${model.commentIsLocked ? 'locked' : ''} ${model.commentIsAuthor ? 'comment-list-item-authored' : ''}" data-comment-id="${model.commentId}">
            
                
                    ${voting}
                
                    <div class="w-100">
                        <div class="comment-actions d-flex">
                            ${lockedDisplay}
                            <div>${actionButtons}</div>
                            
                        </div>

                        

                        <div class="form-post-comment-edit-container">
                            ${editCommentForm}
                        </div>
                
                        <div class="comment-thread">
                            <div class="comment-content">
                                <div class="comment-content-md">
                                    <div class="md">
                                        ${MarkdownUtility.toHtml(model.commentContent)}
                                    </div>
                                </div>

                                <div class="form-post-comment-new-container"></div>
                            </div>

                            <ul class="comment-list" data-parent-comment-id="${model.commentId}">
                                ${replies}
                            </ul>
                        </div>
                    </div>
            </li>
        `;

        return html.replace('\n\n', '');
    }


    public getActionsSection(model: CommentApiResponse): HtmlString
    {

        const editActionButtons = `
            &#183;
            <a href="#" class="text-reset btn-comment-action btn-comment-action-edit">Edit</a>
            &#183;
            <a href="#" class="text-reset btn-comment-action btn-comment-action-delete">Delete</a>
        `;

        let moderateButton = `&#183; <a href="#" class="text-reset btn-comment-action btn-comment-action-moderate">Moderate</a>`;

        if (!model.userIsCommunityModerator)
        {
            moderateButton = '';
        }


        let replyButton = `&#183; <a href="#" class="text-reset btn-comment-action btn-comment-action-reply">Reply</a>`;

        if (Nullable.hasValue(model.commentDeletedOn))
        {
            replyButton = '';
        }
        else if (model.commentIsLocked)
        {
            replyButton = '';
        }


        let html = `
            <small class="text-muted">
                <span class="comment-author-username">${model.commentAuthorUsername}</span> &#183;
                <span>${model.createdOnDifferenceDisplay ?? 'seconds'} ago</span> &#183;
                <a href="#" class="text-reset btn-comment-action btn-comment-action-toggle">Hide</a> 
                ${replyButton}
                ${model.commentIsAuthor ? editActionButtons : ''}
                ${moderateButton}
            </small>
        `;

        return html;
    }
}