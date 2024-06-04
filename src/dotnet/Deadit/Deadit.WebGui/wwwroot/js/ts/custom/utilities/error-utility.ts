import { ErrorMessage } from "../domain/model/api-response";
import { ApiNotFoundException, ApiValidationException } from "../domain/model/exceptions";
import { HtmlString } from "../domain/types/aliases";
import { ErrorMessageTemplate } from "../templates/error-message-template";

export type OnExceptionCallbacks = {

    onApiNotFoundException?: (error: ApiNotFoundException) => void;
    onApiValidationException?: (error: ApiValidationException) => void;
    onOther?: (error: Error) => void;
}

export class ErrorUtility
{
    public static onException = (error: Error, callbacks: OnExceptionCallbacks) =>
    {
        if (error instanceof ApiNotFoundException && callbacks.onApiNotFoundException != null)
        {
            callbacks.onApiNotFoundException(error);
            return;
        }
        else if (error instanceof ApiValidationException && callbacks.onApiValidationException != null)
        {
            callbacks.onApiValidationException(error);
            return;
        }
        else if (callbacks.onOther != null)
        {
            callbacks.onOther(error);
            return;
        }

        throw error;
    }


    public static generateErrorList(errors: ErrorMessage[]): HtmlString
    {
        const template = new ErrorMessageTemplate();
        const listItems = template.toHtmls(errors);

        const messageBody = `<ul class="error-message-list">${listItems}</ul>`;

        return messageBody;
    }
}