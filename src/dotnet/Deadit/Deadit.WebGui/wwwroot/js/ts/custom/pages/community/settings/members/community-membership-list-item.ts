import { NativeEvents } from "../../../../domain/constants/native-events";
import { IController } from "../../../../domain/contracts/i-controller";
import { CommunityMembershipDropdownAction } from "../../../../domain/enum/community-membership-dropdown-action";
import { CommunityMembershipDropdownClickedEvent } from "../../../../domain/events/events";



const Selectors = {
    usernameAttr: 'data-username',
    containerClass: 'community-members-list-item',
    dropdownActionAttr: 'data-js-action',
    dropdownMenuClass: 'community-members-item-list-item-dropdown-menu',
}


export class CommunityMembershipListItem implements IController
{
    private _container: HTMLLIElement;


    public get username(): string
    {
        return this._container.getAttribute(`${Selectors.usernameAttr}`);
    }


    constructor(e: Element)
    {
        this._container = e.closest<HTMLLIElement>(`.${Selectors.containerClass}`);
    }

    public control = () =>
    {
        this.addListeners();
    }

    public remove()
    {
        this._container.remove();
    }


    private addListeners = () =>
    {

        this._container.querySelectorAll<HTMLButtonElement>(`.dropdown-item`).forEach((button) =>
        {
            button.addEventListener(NativeEvents.Click, (e) =>
            {
                let target = e.target as HTMLButtonElement;
                const action = target.getAttribute(`${Selectors.dropdownActionAttr}`) as CommunityMembershipDropdownAction;
                this.handleDropdownButtonClick(action);
            });
        });
    }


    private handleDropdownButtonClick(action: CommunityMembershipDropdownAction)
    {
        CommunityMembershipDropdownClickedEvent.invoke(this, {
            action: action,
            username: this.username,
        });

    }



    public static initListItems(container: Element)
    {
        const listItems = container.querySelectorAll<HTMLLIElement>(`.${Selectors.containerClass}`);

        listItems.forEach((li) =>
        {
            const listItem = new CommunityMembershipListItem(li);
            listItem.control();
        });
    }


    public static getListItem(container: Element, username: string)
    {
        const element = container.querySelector<HTMLLIElement>(`.${Selectors.containerClass}[${Selectors.usernameAttr}="${username}"]`);

        return new CommunityMembershipListItem(element);
    }
}