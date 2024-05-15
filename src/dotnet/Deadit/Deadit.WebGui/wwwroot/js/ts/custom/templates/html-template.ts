import { HtmlString } from "../domain/types/aliases";



export abstract class HtmlTemplate<T>
{
    public abstract toHtml(model: T): HtmlString;

    public toHtmls = (models: T[]): HtmlString => models.map(model => this.toHtml(model)).join('');
}

