import { NativeEvents } from "../../../domain/constants/native-events";
import { IController } from "../../../domain/contracts/i-controller";
import { LinkPostApiRequest, TextPostApiRequest } from "../../../domain/model/post-models";
import { PostService } from "../../../services/post-service";
import { ErrorUtilities } from "../../../utilities/error-utilities";
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

        this._postService = new PostService(this._communityName);
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

            const url = `/c/${this._communityName}/posts/${response.response.data.postId}`;
            window.location.href = url;

            return true;
        }
        catch (error)
        {
            ErrorUtilities.onException(error, {
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

    private getFormData = (): TextPostApiRequest =>
    {
        const model = this._form.getModel();

        const formData: TextPostApiRequest = {
            title: model.title,
            content: model.content,
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

            const url = `/c/${this._communityName}/posts/${response.response.data.postId}`;
            window.location.href = url;

            return true;
        }
        catch (error)
        {
            ErrorUtilities.onException(error, {
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

    private getFormData = (): LinkPostApiRequest =>
    {
        const model = this._form.getModel();

        const formData: LinkPostApiRequest = {
            title: model.title,
            url: model.content,
        };

        return formData;
    }
}
