import { JsonObject } from "../domain/types/aliases";



export class FormDataMapper
{

    static toJsonObject = (data: Object): JsonObject =>
    {
        return JSON.stringify(data);
    }


    static toFormData(data: Object) : FormData  {
        const formData = new FormData();

        for (const key in data) {
            const value = data[key];
            formData.append(key, value);
        }

        return formData;
    }
}