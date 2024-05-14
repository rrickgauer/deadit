import { LoginModal } from "../../../components/login-modal/login-modal";
import { NativeEvents } from "../../../domain/constants/native-events";
import { IControllerAsync } from "../../../domain/contracts/i-controller";
import { PostPageParms } from "../../../domain/model/post-models";



export type RootCommentFormControllerArgs = {
    postPageArgs: PostPageParms;
    isLoggedIn: boolean;
}


export class RootCommentFormController implements IControllerAsync
{
    postPageArgs: PostPageParms;
    private readonly _isLoggedIn: boolean;

    constructor(args: RootCommentFormControllerArgs)
    {
        this._isLoggedIn = args.isLoggedIn;
        this.postPageArgs = args.postPageArgs;
    }


    public async control()
    {
        this.addListeners();
    }

    private addListeners = () =>
    {
        document.querySelector('.btn-root-form-login')?.addEventListener(NativeEvents.Click, (e) =>
        {
            LoginModal.ShowModal();
        });
    }


}