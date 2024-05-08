import { BootstrapModalEvents } from "../../constants/bootstrap-constants";
import { NativeEvents } from "../../constants/native-events";
import { MessageBoxBase } from "./MessageBoxBase";
import { MessageBoxType } from "./MessageBoxType";

export type MessageBoxConfirmArgs = {
    onSuccess?: () => void;
    //onCancel?: () => void;
}


export class MessageBoxConfirm extends MessageBoxBase {
    public readonly messageBoxType = MessageBoxType.ERROR;
    protected defaultTitle = "Confirm";

    private readonly _defaultConfirmButtonText = "Confirm";
    private readonly _btnConfirm: HTMLButtonElement;

    private onSuccess?: () => void;

    public get element() {
        return document.querySelector('#message-box-confirm') as HTMLDivElement;
    }

    constructor(message: string, confirmButtonText?: string, title?: string) {
        super(message, title);

        this._btnConfirm = this.element.querySelector('[data-js-confirm]') as HTMLButtonElement;
        this._btnConfirm.innerText = confirmButtonText ?? this._defaultConfirmButtonText;

        this.element.addEventListener(BootstrapModalEvents.Hidden, () => {
            this._btnConfirm.removeEventListener(NativeEvents.Click, this.clickHandler);
        });
    }

    public confirm = (args: MessageBoxConfirmArgs) => {
        this.onSuccess = args.onSuccess;
        this.show();
        this._btnConfirm.addEventListener(NativeEvents.Click, this.clickHandler);
    };

    private clickHandler = (e) => {
        e.preventDefault();
        this.onSuccess();

        // Remove the click event listener
        this._btnConfirm.removeEventListener(NativeEvents.Click, this.clickHandler);

        this.close();
    };


}
