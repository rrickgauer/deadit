import { VoteScore } from "../../../domain/helpers/vote-scores/vote-score";
import { Guid } from "../../../domain/types/aliases";


export const PostElements = {
    containerClass: 'list-group-item-post',
    postTitleLinkClass: 'post-title-link',
    postIdAttr: 'data-post-id',
}

export class PostListItem
{
    private readonly _container: HTMLLIElement;
    private readonly _postTitleLinkElement: HTMLAnchorElement;
    private readonly _voting: VoteScore;

    private get postId(): Guid | null
    {
        return this._container.getAttribute(PostElements.postIdAttr);
    }
    

    constructor(element: Element)
    {
        this._container = element.closest(`.${PostElements.containerClass}`) as HTMLLIElement;
        this._postTitleLinkElement = this._container.querySelector(`.${PostElements.postTitleLinkClass}`) as HTMLAnchorElement;
        this._voting = new VoteScore(this._container.querySelector(`.item-voting`));
    }

    public upvoted()
    {
        return this._voting.upvoted();
    }

    public downvoted()
    {
        return this._voting.downvoted();
    }
}