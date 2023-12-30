import { LoginModalController } from "../../../components/login-modal/login-modal-controller";
import { NativeEvents } from "../../../domain/constants/native-events";
import { PageUtilities } from "../../../utilities/page-utilities";


PageUtilities.pageReady(async () =>
{
    const loginModal = LoginModalController.setupPage();
    testing();
    
});


function testing()
{
    /*    document.querySelector()*/

    /*    create-community-form*/

    const form: HTMLFormElement = document.querySelector('.create-community-form');

    form.addEventListener(NativeEvents.Submit, async (e) =>
    {
        e.preventDefault();

        if (confirm('Clear form?'))
        {
            form.reset();
        }
    });
}



