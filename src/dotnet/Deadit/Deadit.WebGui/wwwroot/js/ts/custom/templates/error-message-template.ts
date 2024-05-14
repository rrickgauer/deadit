import { ErrorMessage } from "../domain/model/api-response";
import { HtmlTemplate } from "./html-template";



export class ErrorMessageTemplate extends HtmlTemplate<ErrorMessage>
{
    public toHtml(model: ErrorMessage)
    {
        let result = `<li class="error-message-list-item" data-error-message-id="${model.id}">${model.message}</li>`;

        return result;
    }
}