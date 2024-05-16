import { NativeEvents } from "../../../domain/constants/native-events";
import { IControllerAsync } from "../../../domain/contracts/i-controller";
import { VoteType } from "../../../domain/enum/vote-type";
import { VoteButton, VoteButtonType } from "../../../domain/helpers/vote-scores/vote-button";
import { VoteService } from "../../../services/vote-service";
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
        document.querySelector('body').addEventListener(NativeEvents.Click, async (e) =>
        {
            try
            {
                let target = e.target as Element;
                const button = VoteButton.getVoteButton(target);
                await this.onVoteButtonClicked(button);
            }
            catch (error)
            {
                return;
            }
        });
    }


    private async onVoteButtonClicked(button: VoteButton)
    {
        const postItem = new PostListItem(button.buttonElement);

        let castVote: VoteType = null;

        switch (button.voteButtonType)
        {
            case VoteButtonType.Upvote:
                castVote = postItem.upvoted();
                break;

            case VoteButtonType.Downvote:
                castVote = postItem.downvoted();
                break;

            default:
                throw new Error(`Unknown vote button type: ${button.voteButtonType}`);
        }

        const voteService = new VoteService();

        try
        {
            const response = await voteService.votePost({
                postId: postItem.postId,
                voteType: castVote,
            });

            console.log({ response });
        }
        catch (error)
        {
            alert('error saving vote');
            console.error({ error });
        }

    }

}