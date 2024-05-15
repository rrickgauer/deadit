

export interface IController
{
    control: () => void;
}

export interface IControllerAsync
{
    control: () => Promise<any>;
}