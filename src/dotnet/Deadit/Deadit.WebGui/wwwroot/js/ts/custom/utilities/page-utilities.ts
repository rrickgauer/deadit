


export class PageUtilities {

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
}

