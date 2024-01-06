import { ApiValidationException } from "../domain/model/api-response";


export class ErrorService
{
    public static handleApiValidationException = (error: any, callbacks: { onApiValidationException?: () => void, onStandardError?: () => void }) =>
    {
        if (error instanceof ApiValidationException)
        {
            if (callbacks.onApiValidationException !== null)
            {
                callbacks.onApiValidationException();
            }
            return true;
        }

        if (callbacks.onStandardError !== null)
        {
            callbacks.onStandardError();
        }

        return false;
    }
}