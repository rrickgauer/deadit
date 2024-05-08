import { CommentApiResponse } from "../domain/model/comment-models";
import { HtmlString } from "../domain/types/aliases";
import { Nullable } from "../utilities/nullable";
import { HtmlTemplate } from "./html-template";



export class CommentFormTemplate extends HtmlTemplate<CommentApiResponse>
{
    public toHtml(model: CommentApiResponse): HtmlString
    {
        const commentIdAttr = Nullable.hasValue(model.commentId) ? `data-comment-id="${model.commentId}"` : '';
        const parentIdAttr = Nullable.hasValue(model.commentParentId) ? `data-comment-parent-id="${model.commentParentId}"` : '';

        let commentForm = `
            <form class="form-post-comment" ${commentIdAttr} ${parentIdAttr}>
                <div class="form-feedback"></div>

                <fieldset>                        
                    <div class="input-feedback">
                        <label class="form-label-container">
                            <textarea class="form-control input-feedback-input" name="content" rows="3">${model.commentContent ?? ''}</textarea>
                            <div class="valid-feedback"></div>
                            <div class="invalid-feedback"></div>
                        </label>
                    </div>

                    <div class="d-flex">
                        <button class="btn btn-success btn-submit" type="submit">Save</button>
                        <button class="btn btn-outline-danger btn-cancel" type="button">Cancel</button>
                    </div>
                </fieldset>

            </form>  
        `;

        return commentForm;
    }
}