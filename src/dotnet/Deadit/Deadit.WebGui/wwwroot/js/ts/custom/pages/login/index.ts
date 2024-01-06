

import { LoginModal } from "../../components/login-modal/login-modal";
import { SuccessfulLoginEvent } from "../../domain/events/events";
import { PageUtilities } from "../../utilities/page-utilities";


PageUtilities.pageReady(() =>
{

    const loginModal = new LoginModal(false);

    addListeners();
});


function addListeners()
{
    SuccessfulLoginEvent.addListener((e) =>
    {
        alert('from main page');
    });
}


