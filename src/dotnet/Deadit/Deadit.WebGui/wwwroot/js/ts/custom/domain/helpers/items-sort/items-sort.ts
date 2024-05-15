import { NativeEvents } from "../../constants/native-events";
import { SortOption } from "../../enum/sort-option";
import { ItemsSortInputChangedEvent } from "../../events/events";



export const ItemsSortElements = {
    selectClass: 'items-sort',
    sortIdAttr: 'data-items-sort-id',
}


export class ItemsSortInput
{
    private readonly _select: HTMLSelectElement;

    public get selectedOption(): SortOption
    {
        return this._select.selectedOptions[0].value as SortOption;
    }

    public get sortId(): string | null
    {
        return this._select.getAttribute(ItemsSortElements.sortIdAttr);
    }

    constructor(element: Element)
    {
        this._select = element.closest(`.${ItemsSortElements.selectClass}`) as HTMLSelectElement;

        this.addListeners();
    }

    private addListeners = () =>
    {
        this._select.addEventListener(NativeEvents.Change, (e) =>
        {
            ItemsSortInputChangedEvent.invoke(this, {
                selectedValue: this.selectedOption,
                itemsSortId: this.sortId ?? null,
            });
        });
    }




}


