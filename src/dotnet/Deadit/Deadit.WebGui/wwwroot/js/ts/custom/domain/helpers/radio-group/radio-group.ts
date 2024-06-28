

const RadioGroupElements = {
    containerClass: 'radio-group',
    groupNameAttr: 'data-radio-group-name',
}

export class RadioGroup<T>
{
    protected readonly _container: HTMLDivElement;

    protected get _groupName(): string
    {
        return this._container.getAttribute(`${RadioGroupElements.groupNameAttr}`);
    }

    public get selectedValue(): T | null
    {
        const selectedInput = this.getSelectedInput();

        if (selectedInput)
        {
            const value = selectedInput.value;
            return value as T;
        }

        return null;
    }

    public set selectedValue(value: T | null)
    {
        this._container.querySelectorAll<HTMLInputElement>(`input[name="${this._groupName}"]`)?.forEach(e =>
        {
            if (e.value === value)
            {
                e.checked = true;
            }
            else
            {
                e.checked = false;
            }
        });
    }


    constructor(e: Element)
    {
        this._container = e.closest<HTMLDivElement>(`.${RadioGroupElements.containerClass}`);
    }


    protected getSelectedInput(): HTMLInputElement | null
    {
        return this._container.querySelector<HTMLInputElement>(`input[name="${this._groupName}"]:checked`);        
    }
}


export class RadioGroupNumber extends RadioGroup<number>
{
    public get selectedValue(): number | null
    {
        const selectedInput = this.getSelectedInput();

        if (selectedInput)
        {
            const value = selectedInput.value;
            return parseInt(value);
        }

        return null;
    }

    public set selectedValue(value: number | null)
    {
        this._container.querySelectorAll<HTMLInputElement>(`input[name="${this._groupName}"]`)?.forEach(e =>
        {
            if (e.value === `${value}`)
            {
                e.checked = true;
            }
            else
            {
                e.checked = false;
            }
        });
    }
}