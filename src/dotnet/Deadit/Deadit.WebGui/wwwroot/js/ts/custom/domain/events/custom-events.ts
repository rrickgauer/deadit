

export class BaseEventDetail 
{
    public caller?: any;
    public data?: any;

    constructor(caller?: any, data?: any) 
    {
        this.caller = caller;
        this.data = data;
    }
}


export class BaseEvent
{

    static get EventName() 
    {
        return this.prototype.constructor.name;
    }

    static invoke(caller?: any, data?: any)
    {
        const eventName = this.prototype.constructor.name;
        const eventDetail = new BaseEventDetail(caller, data);

        const customEvent = new CustomEvent(eventName, {
            detail: eventDetail,
            bubbles: true,
            cancelable: true,
        });

        const body = document.querySelector('body');
        body.dispatchEvent(customEvent);
    }

    static addListener(callback: (event: BaseEventDetail) => void)
    {
        const body = document.querySelector('body');
        const eventName = this.prototype.constructor.name;

        body.addEventListener(eventName, (e: CustomEvent) =>
        {
            callback(e.detail);
        });
    }
}