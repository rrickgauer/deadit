import { NativeEvents } from "../../../domain/constants/native-events";
import { IControllerAsync } from "../../../domain/contracts/i-controller";
import { IVoted } from "../../../domain/contracts/ivoted";
import { VoteType } from "../../../domain/enum/vote-type";
import { VoteButton, VoteButtonType } from "../../../domain/helpers/vote-scores/vote-button";
import { VoteScore } from "../../../domain/helpers/vote-scores/vote-score";
import { PostPageParms } from "../../../domain/model/post-models";
import { Guid } from "../../../domain/types/aliases";
import { VoteService } from "../../../services/vote-service";
import { ErrorUtility } from "../../../utilities/error-utility";
import { MessageBoxUtility } from "../../../utilities/message-box-utility";




export type PostContentArgs = PostPageParms & {
    isLoggedIn: boolean;
}

export const PostContentElements = {
    ContainerClass: 'card-post-content',
}

export class PostContent implements IControllerAsync, IVoted
{
    private readonly _args: PostContentArgs;
    private readonly _container: HTMLDivElement;
    private readonly _voting: VoteScore;
    private readonly _voteService: VoteService;

    //#region - Getters/Setters -

    private get _postId(): Guid
    {
        return this._args.postId;
    }

    private get _isLoggedIn(): boolean
    {
        return this._args.isLoggedIn;
    }

    private get _communityName(): string
    {
        return this._args.communityName;
    }

    //#endregion

    constructor(args: PostContentArgs)
    {
        this._args = args;
        this._container = document.querySelector(`.${PostContentElements.ContainerClass}`) as HTMLDivElement;
        this._voting = new VoteScore(this._container.querySelector(`.item-voting`));

        this._voteService = new VoteService();
    }

    public async control()
    {
        this.addListeners();
    }

    private addListeners = () =>
    {
        this._container.addEventListener(NativeEvents.Click, async (e) =>
        {
            let target = e.target as Element;
            let buttonElement = target.closest('.btn-vote') as HTMLButtonElement;

            if (!buttonElement)
            {
                return;
            }

            await this.onVoteButtonClick(buttonElement);
        });
    }

    private async onVoteButtonClick(buttonElement: HTMLButtonElement)
    {
        const buttonType = VoteButton.getVoteButton(buttonElement).voteButtonType;
        const newVoteType = this.getNewVoteValue(buttonType);

        // save the new vote type to the api
        try
        {
            await this.saveVote(newVoteType);
        }
        catch (error)
        {
            this.onSaveVoteError(error);
            return;
        }

    }

    private getNewVoteValue(buttonType: VoteButtonType): VoteType
    {
        let newVoteType: VoteType = null;

        switch (buttonType)
        {
            case VoteButtonType.Upvote:
                newVoteType = this.upvoted();
                break;

            case VoteButtonType.Downvote:
                newVoteType = this.downvoted();
                break;

            default:
                throw new Error(`${buttonType} not implemented`);
        }

        return newVoteType;
    }

    private async saveVote(voteType: VoteType)
    {
        const response = await this._voteService.votePost({
            postId: this._postId,
            voteType: voteType,
        });

        if (!response.successful)
        {
            MessageBoxUtility.showErrorList(response.response.errors);
        }
    }

    private onSaveVoteError(error: Error)
    {
        console.error(error);

        ErrorUtility.onException(error, {
            onApiNotFoundException: (e) =>
            {
                MessageBoxUtility.showError({ message: 'Could not find this post' });
            },
            onOther: (e) =>
            {
                MessageBoxUtility.showError({
                    message: 'Unexpected error',
                });
            },
        });
    }


    public upvoted(): VoteType
    {
        this._voting.upvoted();
        return this._voting.currentVote;
    }

    public downvoted(): VoteType
    {
        this._voting.downvoted();
        return this._voting.currentVote;
    }
}