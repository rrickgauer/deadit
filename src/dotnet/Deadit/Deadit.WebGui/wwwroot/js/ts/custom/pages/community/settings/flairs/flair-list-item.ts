import { EditFlairPostEvent } from "../../../../domain/events/events";
import { FlairFormModal } from "./flair-form-modal";

const selectors = {
    containerClass: 'flair-post-list-item',
    flairIdAttr: 'data-js-flair-id',
}

export class FlairListItem
{
    private _container: HTMLLIElement;

    public get flairId(): number
    {
        const attrValue = this._container.getAttribute(selectors.flairIdAttr);

        return parseInt(attrValue);
    }

    constructor(e: Element)
    {
        this._container = e.closest<HTMLLIElement>(`.${selectors.containerClass}`);
    }

    public edit()
    {
        EditFlairPostEvent.invoke(this, {
            flairId: this.flairId,
        });
    }

    public remove()
    {
        this._container.remove();
    }
}