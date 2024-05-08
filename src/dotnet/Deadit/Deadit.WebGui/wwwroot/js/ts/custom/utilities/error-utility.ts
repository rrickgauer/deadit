import { ApiNotFoundException, ApiValidationException } from "../domain/model/api-response";

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
}