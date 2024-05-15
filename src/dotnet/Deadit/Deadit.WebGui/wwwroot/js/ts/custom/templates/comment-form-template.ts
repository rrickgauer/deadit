import { Guid } from "../domain/types/aliases"
import { Nullable } from "../utilities/nullable";
import { HtmlTemplate } from "./html-template";



export type CreateCommentFormArgs = {
    commentId: Guid;
    isNew: boolean;
    parentId?: Guid;
    content?: string;
}


export class CommentFormTemplate extends HtmlTemplate<CreateCommentFormArgs>
{
    public toHtml(model: CreateCommentFormArgs)
    {
        const commentIdAttr = `data-comment-id="${model.commentId}"`;
        const parentIdAttr = Nullable.hasValue(model.parentId) ? `data-comment-parent-id="${model.parentId}"` : '';
        const isNewClass = model.isNew ? 'form-post-comment-new' : '';

        let commentForm = `
            <form class="form-post-comment ${isNewClass}" ${commentIdAttr} ${parentIdAttr}>
                <div class="form-feedback"></div>

                <fieldset>                        
                    <div class="input-feedback">
                        <label class="form-label-container">
                            <textarea class="form-control input-feedback-input" name="content" rows="3">${model.content ?? ''}</textarea>
                            <div class="valid-feedback"></div>
                            <div class="invalid-feedback"></div>
                        </label>
                    </div>

                    <div class="d-flex">
                        <button class="btn btn-sm btn-success btn-submit me-1" type="submit">Save</button>
                        <button class="btn btn-sm btn-outline-danger btn-form-post-comment-cancel" type="button">Cancel</button>
                    </div>
                </fieldset>

            </form>  
        `;

        return commentForm;
    }
}