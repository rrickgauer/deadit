import { NativeEvents } from "../../../domain/constants/native-events";
import { IController } from "../../../domain/contracts/i-controller";
import { IVoted } from "../../../domain/contracts/ivoted";
import { PostDropdownAction } from "../../../domain/enum/post-dropdown-action";
import { PostModerationDropdownAction } from "../../../domain/enum/post-moderation-dropdown-action";
import { VoteType } from "../../../domain/enum/vote-type";
import { PostDropdownItemClickData, PostDropdownItemClickEvent, PostModerationDropdownItemClickData, PostModerationDropdownItemClickEvent } from "../../../domain/events/events";
import { MessageBoxConfirm } from "../../../domain/helpers/message-box/MessageBoxConfirm";
import { VoteButton, VoteButtonType } from "../../../domain/helpers/vote-scores/vote-button";
import { VoteScore } from "../../../domain/helpers/vote-scores/vote-score";
import { ModeratePostForm, PostPageParms } from "../../../domain/model/post-models";
import { Guid } from "../../../domain/types/aliases";
import { PostService } from "../../../services/post-service";
import { VoteService } from "../../../services/vote-service";
import { ErrorUtility } from "../../../utilities/error-utility";
import { MessageBoxUtility } from "../../../utilities/message-box-utility";
import { PageLoadingUtility } from "../../../utilities/page-loading-utility";
import { PageUtility } from "../../../utilities/page-utility";
import { PostContentForm } from "./post-content-form";
import { PostDropdownButton } from "./post-dropdown-button";
import { PostModerationDropdownButton } from "./post-moderation-dropdown-button";




export type PostContentArgs = PostPageParms & {
    isLoggedIn: boolean;
    postIsDeleted: boolean;
}

export const PostContentElements = {
    ContainerClass: 'card-post-content',
    DropdownMenuClass: 'dropdown-post',
}

export class PostContent implements IController, IVoted
{
    private readonly _args: PostContentArgs;
    private readonly _container: HTMLDivElement;
    private readonly _voting: VoteScore;
    private readonly _voteService: VoteService;
    private _dropdownMenu: HTMLDivElement;
    private _editForm: PostContentForm;
    private _postService: PostService;
    private _postModerationDropdownMenu: HTMLDivElement | null;

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
        this._dropdownMenu = this._container.querySelector(`.${PostContentElements.DropdownMenuClass}`) as HTMLDivElement;
        this._editForm = new PostContentForm(this._communityName, this._postId);
        this._voteService = new VoteService();
        this._postService = new PostService(this._communityName);
        this._postModerationDropdownMenu = document.querySelector(`.dropdown-post-moderation`) as HTMLDivElement;
    }

    public control()
    {
        this.addListeners();
        this._editForm.control();
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


        PostDropdownButton.addListenersToDropdown(this._dropdownMenu);

        PostDropdownItemClickEvent.addListener(async (message) =>
        {
            await this.onPostDropdownItemClickEvent(message.data);
        });



        PostModerationDropdownButton.initDropdownMenu(this._postModerationDropdownMenu);

        PostModerationDropdownItemClickEvent.addListener(async (message) =>
        {
            await this.onPostModerationDropdownButtonClick(message.data);
        });


    }

    private async onVoteButtonClick(buttonElement: HTMLButtonElement)
    {

        if (this._args.postIsDeleted)
        {
            return;
        }

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


    private async onPostDropdownItemClickEvent(message: PostDropdownItemClickData)
    {

        switch (message.action)
        {
            case PostDropdownAction.Edit:
                this._editForm.editing = true;
                break;

            case PostDropdownAction.Delete:
                await this.confirmDeletePost();
                break;

            default:
                alert(message.action);
                break;
        }
    }

    private async confirmDeletePost()
    {
        const message = new MessageBoxConfirm('Are you sure you want to delete permanently this post?');

        message.confirm({
            onSuccess: async () =>
            {
                await this.deletePost();
            }
        });
    }

    private async deletePost()
    {
        try
        {
            const response = await this._postService.deletePost(this._postId);

            if (!response.successful)
            {
                MessageBoxUtility.showErrorList(response.response.errors);
                return;
            }

            window.location.href = window.location.href;
        }
        catch (error)
        {
            MessageBoxUtility.showError({
                message: 'There was an error deleting the post.',
            });

            console.error({ error });

            throw error;
        }
    }


    private async onPostModerationDropdownButtonClick(message: PostModerationDropdownItemClickData)
    {
        PageLoadingUtility.showLoader();

        switch (message.action)
        {
            case PostModerationDropdownAction.Lock:

                const wasLocked = await this.lockPost(true);

                if (wasLocked)
                {
                    PageUtility.refreshPage();
                }
                else
                {
                    PageLoadingUtility.hideLoader();
                }

                break;

            case PostModerationDropdownAction.Unlock:
                const wasUnlocked = await this.lockPost(false);

                if (wasUnlocked)
                {
                    PageUtility.refreshPage();
                }
                else
                {
                    PageLoadingUtility.hideLoader();
                }

                break;


            case PostModerationDropdownAction.Remove:

                const result = await this.removePost(true);

                if (result)
                {
                    PageUtility.refreshPage();
                }
                else
                {
                    PageLoadingUtility.hideLoader();
                }

                break;

            case PostModerationDropdownAction.Restore:
                const restoreRestul = await this.removePost(false);

                if (restoreRestul)
                {
                    PageUtility.refreshPage();
                }
                else
                {
                    PageLoadingUtility.hideLoader();
                }
                
                break;

            default:
                PageLoadingUtility.hideLoader();
                alert(`Unknown PostModerationDropdownAction: ${message.action}`);
                break;
        }
    }






    private async lockPost(isLocked: boolean): Promise<boolean>
    {
        return await this.moderatePost({
            locked: isLocked,
        });
    }

    private async removePost(isRemoved: boolean): Promise<boolean>
    {
        return await this.moderatePost({
            removed: isRemoved,
        });
    }


    private async moderatePost(form: ModeratePostForm): Promise<boolean>
    {
        try
        {
            const response = await this._postService.moderatePost(this._postId, form);

            if (!response.successful)
            {
                MessageBoxUtility.showErrorList(response.response.errors);
                return false;
            }

            return true;
        }
        catch (error)
        {
            const title = `Could not ${form.locked? 'lock' : 'unlock'} post`;

            ErrorUtility.onException(error, {
                onApiNotFoundException: (e) =>
                {
                    MessageBoxUtility.showError({
                        message: 'Post not found. Please try again later.',
                        title: title,
                    });

                    return false;
                },
                onOther: (e) =>
                {
                    MessageBoxUtility.showError({
                        message: `Unknown error. Please try again later.`,
                        title: title,
                    });

                    return false;
                }
            });

            return false;
        }

        
    }

}