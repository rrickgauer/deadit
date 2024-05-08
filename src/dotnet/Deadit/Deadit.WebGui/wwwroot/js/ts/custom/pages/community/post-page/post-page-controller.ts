import { LoginModal } from "../../../components/login-modal/login-modal";
import { IController } from "../../../domain/contracts/i-controller";
import { PostPageParms } from "../../../domain/model/post-models";
import { CommentsController } from "./comments-controller";

export class PostPageController implements IController
{
    private readonly _communityName: string;
    private readonly _postId: string;
    private readonly _modal = new LoginModal(true);
    private readonly _commentsController: CommentsController;

    constructor(args: PostPageParms)
    {
        this._communityName = args.communityName;
        this._postId = args.postId;
        this._commentsController = new CommentsController(args);
    }

    public control = async () =>
    {
        await this._commentsController.control();

        this.addListeners();
    }

    private addListeners = () =>
    {



    }

}