import { Nullable } from "./nullable";



export class PageLoadingUtility
{

    public static get loader()
    {
        return document.querySelector('.loading-screen') as HTMLDivElement;
    }

    public static hideLoader()
    {
        if (Nullable.hasValue(PageLoadingUtility.loader))
        {
            PageLoadingUtility.loader.style.display = 'none';
        }
    }
}