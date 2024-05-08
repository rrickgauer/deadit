
import { LoginModal } from "../../components/login-modal/login-modal";
import { PageUtility } from "../../utilities/page-utility";


PageUtility.pageReady(() => {
    const loginModal = new LoginModal();
});
