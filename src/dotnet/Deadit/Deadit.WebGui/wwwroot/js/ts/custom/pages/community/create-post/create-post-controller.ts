import { IController } from "../../../domain/contracts/i-controller";
import { CreatePostLinkForm, CreatePostTextForm } from "./create-post-text-form";
import { UrlUtility } from "../../../utilities/url-utility";
import { LoginModal } from "../../../components/login-modal/login-modal";


export class CreatePostController implements IController
{
    private readonly _modal = new LoginModal(true);

    private readonly _communityName: string = UrlUtility.getCurrentPathValue(1);

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

