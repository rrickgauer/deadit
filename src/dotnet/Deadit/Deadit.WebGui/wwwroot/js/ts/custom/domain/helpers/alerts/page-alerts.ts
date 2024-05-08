import { AlertTypes } from "./alert-types";



export type AlertParms = {
    container: HTMLDivElement;
    message: string;
}

//#region Base class


export abstract class AlertBase
{
    protected abstract _alertType: AlertTypes;

    protected _message: string;
    protected _container: HTMLDivElement;

    constructor(args: AlertParms)
    {
        this._message = args.message;
        this._container = args.container;
    }

    public static getPageTopContainer = (): HTMLDivElement =>
    {
        return document.querySelector('#alerts-page-top') as HTMLDivElement;
    }

    public show = () =>
    {
        this._container.innerHTML = this._getHtml();
    }

    protected _getHtml = (): string =>
    {

        let html = `

            <div class="alert ${this._alertType} alert-dismissible show" role="alert">
                <div>${this._message}</div>
                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
            </div>
        `;

        return html;
    }
}

//#endregion


export class AlertSuccess extends AlertBase
{
    protected _alertType = AlertTypes.SUCCESS;   
}

export class AlertDanger extends AlertBase
{
    protected _alertType = AlertTypes.DANGER;
}



