import { NativeEvents } from "../../../domain/constants/native-events";
import { IControllerAsync } from "../../../domain/contracts/i-controller";
import { VoteButton, VoteButtonType } from "../../../domain/helpers/vote-scores/vote-button";
import { PostListItem } from "./post-item";



export type PostListControllerArgs = {
    
}


export class PostListController implements IControllerAsync
{
    constructor(args: PostListControllerArgs)
    {

    }

    public async control()
    {
        this.addListeners();
    }

    private addListeners = () =>
    {
        // listen for vote button clicks
        document.querySelector('body').addEventListener(NativeEvents.Click, (e) =>
        {
            try
            {
                let target = e.target as Element;
                const button = VoteButton.getVoteButton(target);
                this.onVoteButtonClicked(button);
            }
            catch (error)
            {
                return;
            }
        });
    }


    private onVoteButtonClicked(button: VoteButton)
    {
        const postItem = new PostListItem(button.buttonElement);

        switch (button.voteButtonType)
        {
            case VoteButtonType.Upvote:
                postItem.upvoted();
                break;

            case VoteButtonType.Downvote:
                postItem.downvoted();
                break;

            default:
                throw new Error(`Unknown vote button type: ${button.voteButtonType}`);
        }

    }

}