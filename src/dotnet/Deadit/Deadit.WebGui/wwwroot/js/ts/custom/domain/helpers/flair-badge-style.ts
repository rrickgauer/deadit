import { ColorUtility } from "../../utilities/color-utility";



export class FlairBadgeStyle
{
    private _flairBackgroundColor: string;
    private _textColor: string;

    constructor(flairHexColor: string)
    {
        this._flairBackgroundColor = flairHexColor;
        this._textColor = ColorUtility.getTextColorBasedOnBackground(this._flairBackgroundColor);
    }

    public getInlineCssStyle()
    {
        const style = `background-color: ${this._flairBackgroundColor}; color: ${this._textColor};`;

        return style;
    }
}