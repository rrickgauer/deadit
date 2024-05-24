import { NativeEvents } from "../../../domain/constants/native-events";
import { IController, IControllerAsync } from "../../../domain/contracts/i-controller";
import { InputFeedbackTextArea } from "../../../domain/helpers/input-feedback";
import { SpinnerButton } from "../../../domain/helpers/spinner-button";
import { Guid } from "../../../domain/types/aliases";
import { PostService } from "../../../services/post-service";
import { MarkdownUtility } from "../../../utilities/markdown-utility";
import { MessageBoxUtility } from "../../../utilities/message-box-utility";
import { Nullable } from "../../../utilities/nullable";


export const PostContentFormElements = {
    ContainerClass: 'post-content',
    DisplayContainerClass: 'post-content-display',
    EditContainerClass: 'post-content-edit',
    EditFormClass: 'edit-post-form',
    EditingClass: 'editing',
}

export class PostContentForm implements IController
{
    private _container: HTMLDivElement;
    private _form: HTMLFormElement | null;
    private _submitButton: SpinnerButton;
    private _cancelButton: HTMLButtonElement;
    private _contentInput: InputFeedbackTextArea;
    private _displayContainer: HTMLDivElement;
    private _communityName: string;
    private _postService: PostService;
    private _postId: string;

    public get editing(): boolean
    {
        return this._container.classList.contains(PostContentFormElements.EditingClass);
    }

    public set editing(value: boolean)
    {
        if (value)
        {
            this._container.classList.add(PostContentFormElements.EditingClass);
        }
        else
        {
            this._container.classList.remove(PostContentFormElements.EditingClass);
        }
    }


    constructor(communityName: string, postId: Guid)
    {
        this._communityName = communityName;
        this._postId = postId;
        this._container = document.querySelector(`.card-post-content .${PostContentFormElements.ContainerClass}`) as HTMLDivElement;
        this._displayContainer = this._container.querySelector(`.${PostContentFormElements.DisplayContainerClass}`) as HTMLDivElement;
        this._form = this._container.querySelector(`.${PostContentFormElements.EditFormClass}`) as HTMLFormElement;
        this._postService = new PostService(this._communityName);

        if (Nullable.hasValue(this._form))
        {
            this._submitButton = new SpinnerButton(this._form.querySelector('.btn-submit'));
            this._cancelButton = this._form.querySelector('.btn-cancel') as HTMLButtonElement;
            this._contentInput = new InputFeedbackTextArea(this._form.querySelector('textarea[name="content"]'), true);
        }

    }



    public control()
    {
        this.addListeners();
    }

    private addListeners = () =>
    {
        this._form?.addEventListener(NativeEvents.Submit, async (e) =>
        {
            e.preventDefault();
            await this.onFormSubmit();
        });

        this._cancelButton?.addEventListener(NativeEvents.Click, (e) =>
        {
            this.editing = false;
        });

        this._contentInput?.inputElement.addEventListener(NativeEvents.KeyUp, (e) =>
        {
            const length = this._contentInput?.inputElement.value.length ?? 0;
            this._submitButton.button.disabled = length < 1;
        });
    }


    private async onFormSubmit()
    {
        this._submitButton?.spin();

        // send api save request
        await this.savePost();

        // update the display content to the new post content
        this.updateDisplayContent(this._contentInput?.inputElement.value ?? "");
        this.editing = false;

        this._submitButton?.reset();
    }

    private async savePost()
    {
        const content = this._contentInput?.inputElement.value ?? "";

        if (!Nullable.hasValue(content))
        {
            throw new Error(`Post content is null`);
        }

        try
        {
            const response = await this._postService.updateTextPost(this._postId, {
                content: content,
            });

            if (!response.successful)
            {
                MessageBoxUtility.showErrorList(response.response.errors);
                return;
            }

            MessageBoxUtility.showSuccess({
                message: `Your post was updated successfully.`,
            });
        }
        catch (error)
        {
            MessageBoxUtility.showError({
                message: `There was an error saving your post.`,
            });

            console.error({ error });
        }
    }



    private updateDisplayContent(content: string)
    {
        const html = MarkdownUtility.toHtml(content);
        this._displayContainer.innerHTML = `<div class="md">${html}</div>`;
    }

}