import { Nullable } from "../../../utilities/nullable";
import { NativeEvents } from "../../constants/native-events";
import { IController } from "../../contracts/i-controller";
import { TopPostSort } from "../../enum/top-post-sort";
import { TopPostSortOptionChangedEvent } from "../../events/events";



export class TopPostSortOptions implements IController
{
    private _selectElement: HTMLSelectElement;

    public get hasTopPostElement(): boolean
    {
        return this._selectElement != null;
    }

    constructor(e: Element)
    {
        this._selectElement = e.closest('.top-post-sort-options') as HTMLSelectElement;
    }

    public control()
    {
        this.addListeners();
    }

    private addListeners = () =>
    {
        this._selectElement?.addEventListener(NativeEvents.Change, (e) =>
        {
            TopPostSortOptionChangedEvent.invoke(this, {
                sort: this._selectElement.selectedOptions[0].value as TopPostSort,
            });
        });
    }

    public static initOptions(container: Element | null): TopPostSortOptions | null
    {
        const options = container?.querySelector('.top-post-sort-options');

        if (!Nullable.hasValue(options))
        {
            return null;
        }

        const sortOptions = new TopPostSortOptions(options);

        sortOptions.control();

        return sortOptions;
    }


    public static getNewUrl(sort: TopPostSort): string
    {
        const url = new URL(window.location.href);
        const queryParms = url.searchParams;

        queryParms.set('sort', sort.toLowerCase());

        // restart the pagination
        queryParms.delete('page');

        let newUrl = `${url.pathname}?${queryParms.toString()}`;

        return newUrl;
    }
}