import { BootstrapUtilityClasses } from "../../constants/bootstrap-constants";



export const ContentLoaderElements = {
    containerClass: 'content-loader',
    spinnerClass: 'content-loader-spinner',
    contentClass: 'content-loader-content',
}

export class ContentLoader
{
    private readonly _container: HTMLDivElement;
    private readonly _spinner: HTMLDivElement;
    private readonly _content: HTMLDivElement;

    constructor(element: Element)
    {
        this._container = element.closest(`.${ContentLoaderElements.containerClass}`) as HTMLDivElement;
        this._spinner = this._container.querySelector(`.${ContentLoaderElements.spinnerClass}`) as HTMLDivElement;
        this._content = this._container.querySelector(`.${ContentLoaderElements.contentClass}`) as HTMLDivElement;
    }

    public showSpinner()
    {
        this._spinner.classList.remove(BootstrapUtilityClasses.DisplayNone);
        this._content.classList.add(BootstrapUtilityClasses.DisplayNone);
    }

    public showContent()
    {
        this._content.classList.remove(BootstrapUtilityClasses.DisplayNone);
        this._spinner.classList.add(BootstrapUtilityClasses.DisplayNone);
    }
}