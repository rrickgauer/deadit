

export class SelectInput<T>
{
    public readonly selectElement: HTMLSelectElement;

    public get selectedValue(): T
    {
        return this.selectElement.selectedOptions[0].value as T;
    }

    public set selectedValue(value: T)
    {
        const option = this.selectElement.querySelector<HTMLOptionElement>(`option[value="${value}"]`);
        option.selected = true;
    }

    constructor(selectElement: HTMLSelectElement)
    {
        this.selectElement = selectElement;
    }
}