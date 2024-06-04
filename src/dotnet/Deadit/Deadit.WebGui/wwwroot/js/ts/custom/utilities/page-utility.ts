
export class PageUtility {

    static pageReady = (fn) =>
    {
        if (document.readyState !== 'loading')
        {
            fn();
        }
        else
        {
            document.addEventListener('DOMContentLoaded', fn);
        }
    }

    static refreshPage()
    {
        window.location.href = window.location.href;
    }
}

