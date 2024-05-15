import { HttpMethods } from "../domain/constants/api-constants";
import { ApplicationTypes } from "../domain/constants/application-types";
import { JsonObject } from "../domain/types/aliases";





export type FetchParms = {
    method?: HttpMethods;
    body?: JsonObject,
}

export class ApiUtility
{
    public static fetchJson = async (url: string, parms?: FetchParms): Promise<Response> =>
    {
        const method = parms?.method ?? HttpMethods.GET;
        const body = parms?.body ?? null;

        return await fetch(url, {
            body: body,
            method: method,
            headers: ApplicationTypes.GetJsonHeaders(),
        });
    }
}



