export class MessageEventDetail<T>
{
    public caller?: any;
    public data?: T;

    constructor(caller?: any, data?: T)
    {
        this.caller = caller;
        this.data = data;
    }
}


export class CustomMessage<T>
{
    private readonly _body = document.querySelector('body') as HTMLBodyElement;

    public invoke(caller?: any, data?: T)
    {
        const eventName = this.constructor.name;
        const detail = new MessageEventDetail<T>(caller, data);

        const customEvent = new CustomEvent(eventName, {
            detail: detail,
            bubbles: true,
            cancelable: true,
        });

        this._body.dispatchEvent(customEvent);
    }

    public addListener(callback: (event: MessageEventDetail<T>) => void)
    {
        const body = document.querySelector('body');
        const eventName = this.constructor.name;

        this._body.addEventListener(eventName, (e: CustomEvent) =>
        {
            callback(e.detail);
        });
    }

    public removeListener = (callback: (event: MessageEventDetail<T>) => void) =>
    {
        const eventName = this.constructor.name;

        this._body.removeEventListener(eventName, (e: CustomEvent) =>
        {
            callback(e.detail);
        });
    }
}






