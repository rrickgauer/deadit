import { FlairBadgeStyle } from "../domain/helpers/flair-badge-style";
import { GetFlairPostApiResponse } from "../domain/model/flair-models";
import { HtmlTemplate } from "./html-template";


export class FlairPostFilterRadioTemplate extends HtmlTemplate<GetFlairPostApiResponse>
{
    public toHtml(data: GetFlairPostApiResponse)
    {

        const id = `flair-post-filter-input-${data.flairPostId}`;

        const badgeStyle = new FlairBadgeStyle(data.flairPostColor);
        const badge = `<span class="badge" style="${badgeStyle.getInlineCssStyle()}">${data.flairPostName}</span>`;

        let html = `
            <div class="form-check my-1">
                <input type="radio" class="form-check-input" id="${id}" name="flair-post-filter-input" value="${data.flairPostId}" />
                <label class="form-check-label" for="${id}">
                    ${badge}
                </label>
            </div>
        `;

        return html;
    }
}