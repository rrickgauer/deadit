

export class SpinnerButton
{
    private static readonly SpinnerHtml = '<div class="spinner-border spinner-border-sm" role="status"></div>'; 

    private readonly _button: HTMLButtonElement;
    private _displayText: string;
    
    constructor(button: HTMLButtonElement)
    {
        this._button = button;
    }

    spin = () =>
    {
        this._displayText = this._button.innerText;
        const width = this._button.offsetWidth

        this._button.innerHTML = SpinnerButton.SpinnerHtml;
        this._button.style.width = `${width}px`;
        this._button.disabled = true;
    }

    reset = () =>
    {
        const width = this._button.offsetWidth;

        this._button.innerHTML = this._displayText;
        this._button.style.width = `${width}px`;
        this._button.disabled = false;
    }
}
