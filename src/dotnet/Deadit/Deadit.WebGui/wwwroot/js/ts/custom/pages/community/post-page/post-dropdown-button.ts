import { NativeEvents } from "../../../domain/constants/native-events";
import { IController } from "../../../domain/contracts/i-controller";
import { PostDropdownAction } from "../../../domain/enum/post-dropdown-action";
import { PostDropdownItemClickEvent } from "../../../domain/events/events";


export const PostDropdownButtonElements = {
    ActionAttr: 'data-js-action',
}

export class PostDropdownButton implements IController
{
    private _button: HTMLButtonElement;

    private get _action(): PostDropdownAction
    {
        return this._button.getAttribute(PostDropdownButtonElements.ActionAttr) as PostDropdownAction;
    }

    constructor(e: Element)
    {
        this._button = e.closest('.dropdown-item') as HTMLButtonElement;
    }

    public control()
    {
        this.addListeners();
    }

    private addListeners = () =>
    {
        this._button.addEventListener(NativeEvents.Click, (e) =>
        {
            PostDropdownItemClickEvent.invoke(this, {
                action: this._action,
            });
        });
    }



    public static addListenersToDropdown(dropdown?: Element)
    {
        dropdown?.querySelectorAll('.dropdown-item').forEach(buttonElement =>
        {
            const item = new PostDropdownButton(buttonElement);
            item.control();
        });
    }

}