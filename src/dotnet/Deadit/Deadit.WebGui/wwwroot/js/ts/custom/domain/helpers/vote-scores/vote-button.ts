import { Nullable } from "../../../utilities/nullable";
import { VoteButtonIcons, VoteButtonIconsSolid } from "../../constants/vote-button-icons";

export enum VoteButtonType 
{
     Upvote = "Upvote",
     Downvote = "Downvote",
}

export abstract class VoteButton
{
    public abstract readonly voteButtonType: VoteButtonType;
    protected abstract readonly _selectedIcon: string;
    protected abstract readonly _normalIcon: string;

    public buttonElement: HTMLButtonElement;

    public get selected(): boolean
    {
        return this._icon.hasAttribute('data-js-selected')
    }

    public set selected(isSelected: boolean)
    {
        this.buttonElement.innerHTML = isSelected ? this._selectedIcon : this._normalIcon;
    }

    protected get _icon(): HTMLElement
    {
        return this.buttonElement.querySelector('i.bx') as HTMLElement;
    }


    constructor(button: HTMLButtonElement)
    {
        this.buttonElement = button;

    }

    public toggle()
    {
        this.selected = !this.selected;
    }

    public static getVoteButton(element: Element): VoteButton
    {
        const button = element.closest('.btn-vote') as HTMLButtonElement;

        if (!Nullable.hasValue(button))
        {
            throw new Error('Invalid vote button');
        }

        if (button.classList.contains('btn-vote-up'))
        {
            return new UpvoteButton(button);
        }
        else if (button.classList.contains('btn-vote-down'))
        {
            return new DownvoteButton(button);
        }
        else
        {
            throw new Error('Invalid vote button');
        }
    }
}

export class UpvoteButton extends VoteButton
{
    public readonly voteButtonType = VoteButtonType.Upvote;
    protected readonly _selectedIcon = VoteButtonIconsSolid.Upvote;
    protected readonly _normalIcon = VoteButtonIcons.Upvote;
}

export class DownvoteButton extends VoteButton
{
    public readonly voteButtonType = VoteButtonType.Downvote;
    protected readonly _selectedIcon = VoteButtonIconsSolid.Downvote;
    protected readonly _normalIcon = VoteButtonIcons.Downvote;
}