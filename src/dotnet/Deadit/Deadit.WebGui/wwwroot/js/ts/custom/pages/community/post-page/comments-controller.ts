import { IController } from "../../../domain/contracts/i-controller";
import { CommentFormSubmittedData, CommentFormSubmittedEvent } from "../../../domain/events/events";
import { CommentApiResponse, GetCommentsApiResponse, SaveCommentRequest } from "../../../domain/model/comment-models";
import { PostPageParms } from "../../../domain/model/post-models";
import { CommentsService } from "../../../services/comments-service";
import { CommentTemplate } from "../../../templates/comment-template";
import { ErrorUtility } from "../../../utilities/error-utility";
import { GuidUtility } from "../../../utilities/guid-utility";
import { MessageBoxUtility } from "../../../utilities/message-box-utility";
import { Nullable } from "../../../utilities/nullable";
import { CommentElement } from "./comment";
import { AuthoredCommentElement } from "./comment-author";
import { CommentForm } from "./comment-form";




export class CommentsController implements IController
{
    private readonly _communityName: string;
    private readonly _postId: string;
    private readonly _commentsService: CommentsService;

    constructor(args: PostPageParms)
    {
        this._communityName = args.communityName;
        this._postId = args.postId;
        this._commentsService = new CommentsService(args);
    }


    public control = async () =>
    {
        await this.addListeners();
        await this.fetchComments();
    }

    private addListeners = async () =>
    {
        CommentForm.addSubmitListeners();

        CommentFormSubmittedEvent.addListener(async (message) =>
        {
            await this.onCommentFormSubmittedEvent(message.data);
        });
    }

    //#region - Fetch All Comments -

    private fetchComments = async () =>
    {
        try
        {
            const serviceResult = await this._commentsService.getAllComments();

            if (!serviceResult.successful)
            {
                MessageBoxUtility.showError({
                    title: 'Api Error - Bad Request',
                    message: 'Could not fetch the post comments',
                });

                return;
            }

            this.displayComments(serviceResult.response.data);
            
        }
        catch (error)
        {
            ErrorUtility.onException(error, {
                onApiNotFoundException: () =>
                {
                    MessageBoxUtility.showError({
                        message: 'Not found api error',
                    });
                },
                onApiValidationException: (e) =>
                {
                    MessageBoxUtility.showError({
                        message: `Validation error`,
                    });
                },
            });

            console.error(error);
            throw error;
        }
    }


    private displayComments = (comments: GetCommentsApiResponse) =>
    {
        const templateEngine = new CommentTemplate();
        const rootList = document.querySelector('.comment-list.root');
        rootList.innerHTML = templateEngine.toHtmls(comments.comments);

        const elements = rootList.querySelectorAll('.comment-list-item');
        this.initCommentElements(elements);
    }


    private initCommentElements = (elements: NodeListOf<Element>) =>
    {
        elements.forEach(e =>
        {

            if (!e.classList.contains('comment-list-item-authored'))
            {
                const comment = new CommentElement(e);
                comment.control();
            }
            else
            {
                const comment = new AuthoredCommentElement(e);
                comment.control();
            }
        });
    }



    //#endregion


    private onCommentFormSubmittedEvent = async (message: CommentFormSubmittedData) =>
    {

        message.form.btnSubmit.spin();

        const commentId = GuidUtility.getRandomGuid();

        const request = new SaveCommentRequest(commentId, {
            content: message.form.inputContent.inputElement.value,
            parentId: message.form.parentId,
        });

        const result = await this.saveComment(request);

        message.form.btnSubmit.reset();

        if (!result.successful)
        {
            return;
        }

        message.form.clearFormInputs();

        this.updateComment(result.response.data);
    }

    private saveComment = async (request: SaveCommentRequest) =>
    {

        try
        {
            const response = await this._commentsService.saveComment(request);

            if (!response.successful)
            {
                MessageBoxUtility.showErrorList(response.response.errors);
            }

            return response;
            
        }
        catch (error)
        {
            MessageBoxUtility.showError({
                message: 'There was an unexpected error saving your comment.',
            });

            console.error(error);

            throw error;
        }
    }

    private updateComment = (comment: CommentApiResponse) =>
    {
        const templateEngine = new CommentTemplate();
        const html = templateEngine.toHtml(comment);

        const isTopLevelComment = !Nullable.hasValue(comment.commentParentId);

        const existingCommentElement = document.querySelector(`.comment-list-item[data-comment-id="${comment.commentId}"]`);



        // insert the comment
        if (!Nullable.hasValue(existingCommentElement))
        {
            let rootListElement = document.querySelector('.comment-list.root') as HTMLUListElement;

            if (!isTopLevelComment)
            {
                const parentElement = new CommentElement(document.querySelector(`.comment-list-item[data-comment-id="${comment.commentParentId}"]`));

                rootListElement = parentElement.element.querySelector('.comment-list');
                rootListElement.insertAdjacentHTML("afterbegin", html);


                parentElement.closeReplyForm();
            }
            else
            {
                rootListElement.insertAdjacentHTML("afterbegin", html);    
            }

            

            const insertedHtml = document.querySelector(`.comment-list-item[data-comment-id="${comment.commentId}"]`);

            const commentElement = new AuthoredCommentElement(insertedHtml);
            commentElement.control();
        }
        else
        {
            MessageBoxUtility.showStandard({
                message: 'Updated a comment',
            });
        }

    }







}