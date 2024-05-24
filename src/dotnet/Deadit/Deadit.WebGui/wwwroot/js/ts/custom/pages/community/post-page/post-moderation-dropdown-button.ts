import { NativeEvents } from "../../../domain/constants/native-events";
import { IController } from "../../../domain/contracts/i-controller";
import { PostModerationDropdownAction } from "../../../domain/enum/post-moderation-dropdown-action";
import { PostModerationDropdownItemClickEvent } from "../../../domain/events/events";


export class PostModerationDropdownButton implements IController
{
    private _button: HTMLButtonElement;


    private get _action(): PostModerationDropdownAction
    {
        return this._button.getAttribute('data-js-action') as PostModerationDropdownAction;
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
            PostModerationDropdownItemClickEvent.invoke(this, {
                action: this._action,
            });
        });
    }

    public static initDropdownMenu(dropdown?: Element)
    {
        dropdown?.querySelectorAll('.dropdown-item').forEach(e =>
        {
            const dropdownItem = new PostModerationDropdownButton(e);
            dropdownItem.control();
        });
    }
}