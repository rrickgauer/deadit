import { NativeEvents } from "../../../domain/constants/native-events";
import { IController } from "../../../domain/contracts/i-controller";
import { CreateLinkPostApiRequest, CreateTextPostApiRequest } from "../../../domain/model/post-models";
import { PostService } from "../../../services/post-service";
import { ErrorUtility } from "../../../utilities/error-utility";
import { CreatePostFormElements } from "./create-post-form-elements";


export type CreatePostArgs = {
    form: HTMLFormElement,
    communityName: string,
}


export abstract class CreatePostFormBase implements IController
{
    protected abstract saveForm: () => Promise<boolean>

    protected _form: CreatePostFormElements;
    protected _communityName: string;
    protected _postService: PostService;
    
    constructor(args: CreatePostArgs)
    {
        this._form = new CreatePostFormElements({
            form: args.form,
        });

        this._communityName = args.communityName;

        this._postService = new PostService();
    }

    public control = () =>
    {
        this.addListeners();
    }

    protected addListeners = () =>
    {
        this._form.form.addEventListener(NativeEvents.Submit, async (e) =>
        {
            e.preventDefault();
            await this.onFormSubmit();
        });

        this._form.inputTitle.inputElement.addEventListener(NativeEvents.KeyPress, async (e) =>
        {
            await this.onFormInputKey(e, true);
        });

        this._form.inputContent.inputElement.addEventListener(NativeEvents.KeyPress, async (e) =>
        {
            await this.onFormInputKey(e, false);
        });

        this._form.btnClearFlairSelection.addEventListener(NativeEvents.Click, (e) =>
        {
            this._form.flairSelect.selectedValue = "-1";
        });

    }


    private onFormInputKey = async (e: KeyboardEvent, preventDefault: boolean) =>
    {
        if (e.code != 'Enter')
        {
            return;
        }

        if (e.ctrlKey)
        {
            await this.onFormSubmit();
        }
        else if (preventDefault)
        {
            e.preventDefault();
        }
    }



    protected onFormSubmit = async () =>
    {
        this._form.submitBtn.spin();
        this._form.fieldSet.disabled = true;

        const saveResult = await this.saveForm();

        this._form.submitBtn.reset();
        this._form.fieldSet.disabled = false;

        if (!saveResult)
        {
            return;
        }
    }
}


export class CreatePostTextForm extends CreatePostFormBase
{

    protected saveForm = async (): Promise<boolean> =>
    {
        const formData = this.getFormData();

        try
        {
            const response = await this._postService.createTextPost(formData);

            if (!response.successful)
            {
                this._form.showErrors(response.response.errors);
                return false;
            }

            const url = `/c/${this._communityName}/posts/${response.response.data.post.postId}`;
            window.location.href = url;

            return true;
        }
        catch (error)
        {
            ErrorUtility.onException(error, {
                onApiForbiddenException: (e) =>
                {
                    this._form.showErrorAlert('Currently, new posts are not being allowed right now. Please try again later.');
                },
                onOther: (e) =>
                {
                    this._form.showErrorAlert('We encountered an unexpected error, and your post was not created. Please try again later.');
                },
            });

            return false;
        }
    }

    private getFormData = (): CreateTextPostApiRequest =>
    {
        const model = this._form.getModel();

        const formData: CreateTextPostApiRequest = {
            title: model.title,
            content: model.content,
            communityName: this._communityName,
            flairPostId: model.flairPostId,
        };

        return formData;
    }
}


export class CreatePostLinkForm extends CreatePostFormBase
{

    protected saveForm = async (): Promise<boolean> =>
    {
        const formData = this.getFormData();

        try
        {
            const response = await this._postService.createLinkPost(formData);

            const url = `/c/${this._communityName}/posts/${response.response.data.post.postId}`;
            window.location.href = url;

            return true;
        }
        catch (error)
        {
            ErrorUtility.onException(error, {
                onApiNotFoundException: (e) =>
                {
                    alert('not found');
                },
                onApiValidationException: (e) =>
                {
                    alert('validation error');
                },
                onOther: (e) =>
                {
                    alert('unknown error');
                },
            });

            return false;
        }
    }

    private getFormData = (): CreateLinkPostApiRequest =>
    {
        const model = this._form.getModel();

        const formData: CreateLinkPostApiRequest = {
            title: model.title,
            url: model.content,
            communityName: this._communityName,
            flairPostId: model.flairPostId,
        };

        return formData;
    }
}
