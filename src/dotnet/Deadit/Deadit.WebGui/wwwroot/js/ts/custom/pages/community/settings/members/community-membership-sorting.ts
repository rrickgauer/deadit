import { NativeEvents } from "../../../../domain/constants/native-events";
import { IController } from "../../../../domain/contracts/i-controller";
import { CommunityMembershipSort } from "../../../../domain/enum/community-membership-sort";
import { CommunityMembershipSortOptionValue } from "../../../../domain/enum/community-membership-sort-option-value";
import { SortDirection } from "../../../../domain/enum/sort-direction";
import { SelectInput } from "../../../../domain/helpers/select-input";



const Selectors = {
    selectInputClass: 'community-members-sorting-options',
}

export class CommunityMembershipSorting implements IController
{
    private readonly _input: SelectInput<CommunityMembershipSortOptionValue>;

    private get _selectedValue(): CommunityMembershipSortOptionValue
    {
        return this._input.selectedValue;
    }

    private set _selectedValue(value: CommunityMembershipSortOptionValue)
    {
        this._input.selectedValue = value;
    }


    constructor(e: Element)
    {
        const input = e.closest<HTMLSelectElement>(`.${Selectors.selectInputClass}`);
        this._input = new SelectInput<CommunityMembershipSortOptionValue>(input);
    }

    public control = () =>
    {
        this.addListeners();
    }

    private addListeners = () =>
    {
        this._input.selectElement.addEventListener(NativeEvents.Change, (e) =>
        {
            this.refreshUrl();
        });
    }


    private refreshUrl()
    {
        const url = new URL(window.location.href);
        const queryParms = url.searchParams;

        const sort = this.getSort();
        const direction = this.getSortDirection();

        queryParms.set('sort', sort);
        queryParms.set('sortDirection', direction);

        // restart the pagination
        queryParms.delete('page');

        let newUrl = `${url.pathname}?${queryParms.toString()}`;

        window.location.href = newUrl;

    }


    private getSortDirection(): SortDirection
    {
        let sortDirection = null;

        switch (this._selectedValue)
        {
            case CommunityMembershipSortOptionValue.JoinedOnAsc:
            case CommunityMembershipSortOptionValue.UsernameAsc:
                sortDirection = SortDirection.Ascending;
                break;

            case CommunityMembershipSortOptionValue.JoinedOnDesc:
            case CommunityMembershipSortOptionValue.UsernameDesc:
                sortDirection = SortDirection.Descending;
                break;

            default:
                throw new Error(`Unknown sort option: ${this._selectedValue}`);
        }

        return sortDirection;
    }

    private getSort(): CommunityMembershipSort
    {
        let sortDirection = null;

        switch (this._selectedValue)
        {
            case CommunityMembershipSortOptionValue.JoinedOnAsc:
            case CommunityMembershipSortOptionValue.JoinedOnDesc:
                sortDirection = CommunityMembershipSort.JoinedOn;
                break;

            case CommunityMembershipSortOptionValue.UsernameAsc:
            case CommunityMembershipSortOptionValue.UsernameDesc:
                sortDirection = CommunityMembershipSort.Username;
                break;

            default:
                throw new Error(`Unknown sort option: ${this._selectedValue}`);
        }

        return sortDirection;
    }


    public static init(): CommunityMembershipSorting
    {
        const element = document.querySelector(`.${Selectors.selectInputClass}`);

        const options = new CommunityMembershipSorting(element);
        options.control();

        return options;
    }


}