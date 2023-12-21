

export class FormDataMapper
{

    static toFormData(data: Object) : FormData  {
        const formData = new FormData();

        for (const key in data) {
            const value = data[key];
            formData.append(key, value);
        }

        return formData;
    }
}