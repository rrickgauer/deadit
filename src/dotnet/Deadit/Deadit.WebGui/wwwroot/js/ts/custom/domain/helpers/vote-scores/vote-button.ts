import { VoteButtonIcons, VoteButtonIconsSolid } from "../../constants/vote-button-icons";

export enum VoteButtonType 
{
    Upvote,
    Downvote
}

export abstract class VoteButton
{
    public abstract readonly voteButtonType: VoteButtonType;
    protected abstract readonly _selectedIcon: string;
    protected abstract readonly _normalIcon: string;

    private _button: HTMLButtonElement;

    public get selected(): boolean
    {
        return this._icon.hasAttribute('data-js-selected')
    }

    public set selected(isSelected: boolean)
    {
        this._button.innerHTML = isSelected ? this._selectedIcon : this._normalIcon;
    }

    protected get _icon(): HTMLElement
    {
        return this._button.querySelector('i.bx') as HTMLElement;
    }


    constructor(button: HTMLButtonElement)
    {
        this._button = button;

    }

    public toggle()
    {
        this.selected = !this.selected;
    }

    public static getVoteButton(button: HTMLButtonElement)
    {
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
            console.error(`Invalid vote button`);
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