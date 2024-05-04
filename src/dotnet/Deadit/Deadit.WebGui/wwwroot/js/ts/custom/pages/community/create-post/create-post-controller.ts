import { IController } from "../../../domain/contracts/i-controller";
import { CreatePostLinkForm, CreatePostTextForm } from "./create-post-text-form";
import { UrlUtilities } from "../../../utilities/url-utilities";


export class CreatePostController implements IController
{
    private readonly _communityName: string = UrlUtilities.getCurrentPathValue(1);

    private readonly _textPostForm = new CreatePostTextForm({
        form: document.querySelector('.create-post-text-form'),
        communityName: this._communityName,
    });

    private readonly _linkPostForm = new CreatePostLinkForm({
        form: document.querySelector('.create-post-link-form'),
        communityName: this._communityName,
    });

    public control = () =>
    {
        this._textPostForm.control();
        this._linkPostForm.control();
    }
}

