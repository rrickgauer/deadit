

export class FormDataMapper
{
    /**
     * Map the specified object to a FormData object
     * @param {Object} data - the object to map
     * @returns {FormData} the form data
     */
    static toFormData(data) {
        const formData = new FormData();

        for (const key in data) {
            const value = data[key];
            formData.append(key, value);
        }

        return formData;
    }
}