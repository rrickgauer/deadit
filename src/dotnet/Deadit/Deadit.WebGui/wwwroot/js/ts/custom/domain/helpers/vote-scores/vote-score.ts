import { VoteButtonIcons, VoteButtonIconsSolid, VoteButtonIconsType } from "../../constants/vote-button-icons";
import { VoteType } from "../../enum/vote-type";
import { UserVoteScores } from "../../model/vote-scores";
import { DownvoteButton, UpvoteButton } from "./vote-button";

export class VoteScore
{
    private _container: HTMLDivElement;
    private _upvoteButton: UpvoteButton;
    private _downvoteButton: DownvoteButton;
    private _scoreContainer: HTMLDivElement;

    public get currentVote(): VoteType
    {
        return this._container.getAttribute('data-current-vote') as VoteType;
    }

    public set currentVote(value: VoteType)
    {
        this._container.setAttribute('data-current-vote', value);
    }

    public get score(): number
    {
        const scoreText = this._scoreContainer.innerText;
        return parseInt(scoreText);
    }

    public set score(value: number)
    {
        this._scoreContainer.innerText = `${value}`;
    }

    constructor(container: Element)
    {
        this._container = container.closest('.item-voting') as HTMLDivElement;
        this._upvoteButton = new UpvoteButton(this._container.querySelector('.btn-vote-up'));
        this._downvoteButton = new DownvoteButton(this._container.querySelector('.btn-vote-down'));
        this._scoreContainer = this._container.querySelector('.item-voting-score') as HTMLDivElement;
    }


    public upvoted()
    {

        this.clearVoteButtonSelections();


        switch (this.currentVote)
        {
            case VoteType.Upvote:
                this.currentVote = VoteType.Novote;
                this.score--;
                break;

            case VoteType.Downvote:
                this.score += 2;
                this.currentVote = VoteType.Upvote;
                this._upvoteButton.selected = true;
                break;

            case VoteType.Novote:
                this.currentVote = VoteType.Upvote;
                this.score++;
                this._upvoteButton.selected = true;
                break;
        }
    }

    public downvoted()
    {
        this.clearVoteButtonSelections();

        switch (this.currentVote)
        {
            case VoteType.Downvote:
                this.score++;
                this.currentVote = VoteType.Novote;
                break;

            case VoteType.Upvote:
                this.score -= 2;
                this.currentVote = VoteType.Downvote;
                this._downvoteButton.selected = true;
                break;

            case VoteType.Novote:
                this.currentVote = VoteType.Downvote;
                this.score--;
                this._downvoteButton.selected = true;
                break;
        }

    }

    private clearVoteButtonSelections()
    {
        this._downvoteButton.selected = false;
        this._upvoteButton.selected = false;
    }

    public static getVoteIcons(score: UserVoteScores): VoteButtonIconsType
    {
        const result: VoteButtonIconsType = {
            Downvote: score.userVoteSelection === VoteType.Downvote ? VoteButtonIconsSolid.Downvote : VoteButtonIcons.Downvote,
            Upvote: score.userVoteSelection === VoteType.Upvote ? VoteButtonIconsSolid.Upvote : VoteButtonIcons.Upvote,
        };

        return result;
    }
}


