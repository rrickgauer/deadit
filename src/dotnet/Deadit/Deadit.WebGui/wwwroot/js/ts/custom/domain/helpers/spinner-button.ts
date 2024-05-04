

export class SpinnerButton
{
    private static readonly SpinnerHtml = '<div class="spinner-border spinner-border-sm" role="status"></div>';

    public readonly button: HTMLButtonElement;
    private _displayText: string;

    constructor(button: HTMLButtonElement)
    {
        this.button = button;
    }

    public spin = () =>
    {
        this._displayText = this.button.innerText;

        const width = this.button.offsetWidth;
        this.button.style.minWidth = `${width}px`;

        this.button.innerHTML = SpinnerButton.SpinnerHtml;

        this.button.disabled = true;
    }

    public reset = () =>
    {
        this.button.innerText = this._displayText;
        this.button.disabled = false;
    }
}
