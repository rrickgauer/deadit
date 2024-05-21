import { IVoted } from "../../domain/contracts/ivoted";
import { VoteType } from "../../domain/enum/vote-type";
import { VoteScore } from "../../domain/helpers/vote-scores/vote-score";
import { Guid } from "../../domain/types/aliases";


export const PostElements = {
    containerClass: 'list-group-item-post',
    postTitleLinkClass: 'post-title-link',
    postIdAttr: 'data-post-id',
}

export class PostListItem implements IVoted
{
    private readonly _container: HTMLLIElement;
    private readonly _voting: VoteScore;

    public get postId(): Guid | null
    {
        return this._container.getAttribute(PostElements.postIdAttr);
    }
    

    constructor(element: Element)
    {
        this._container = element.closest(`.${PostElements.containerClass}`) as HTMLLIElement;
        this._voting = new VoteScore(this._container.querySelector(`.item-voting`));
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