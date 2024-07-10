import { Icons } from "../domain/constants/icons";
import { FlairPostListItemDropdownAction } from "../domain/enum/flair-post-list-item-dropdown-action";
import { FlairBadgeStyle } from "../domain/helpers/flair-badge-style";
import { GetFlairPostApiResponse } from "../domain/model/flair-models";
import { HtmlTemplate } from "./html-template";



export class FlairPostTemplate extends HtmlTemplate<GetFlairPostApiResponse>
{
    public toHtml(model: GetFlairPostApiResponse): string
    { 
        const flairBadgeStyle = new FlairBadgeStyle(model.flairPostColor);

        let html = `

        <li class="list-group-item flair-post-list-item" data-js-flair-id="${model.flairPostId}">

            <div class="d-flex justify-content-between align-items-center">

                <div>
                    <p class="m-0"><span class="badge badge-flair" style="${flairBadgeStyle.getInlineCssStyle()}">${model.flairPostName}</span></p>
                </div>

                <div class="dropdown">
                    <button class="btn btn-sm btn-reset" type="button" data-bs-toggle="dropdown">${Icons.Dots}</button>
                    <ul class="dropdown-menu">
                        <li><button class="dropdown-item flair-post-list-item-dropdown-button" type="button" data-js-action="${FlairPostListItemDropdownAction.Edit}">Edit</button></li>
                        <li><button class="dropdown-item flair-post-list-item-dropdown-button" type="button" data-js-action="${FlairPostListItemDropdownAction.Delete}">Delete</button></li>
                    </ul>
                </div>

            </div>
        </li>




        `;

        return html;
    }

}


