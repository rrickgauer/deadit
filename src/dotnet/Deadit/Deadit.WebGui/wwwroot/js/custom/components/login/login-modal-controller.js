import { NativeEvents } from "../../domain/constants/native-events";
import { LoginApiRequest } from "../../domain/model/api-auth-models";
import { AuthService } from "../../services/auth-service";


export class LoginModalController
{
    /** @type {HTMLDivElement} */
    #modal = document.querySelector('.login-form-login');

    /** @type {HTMLInputElement} */
    #loginUsernameInput = document.querySelector('#login-form-login-input-username');

    /** @type {HTMLInputElement} */
    #loginPasswordInput = document.querySelector('#login-form-login-input-password');

    #authService = new AuthService();

    init = () => {
        this.#addListeners();
    }

    #addListeners = () => {

        this.#modal.addEventListener(NativeEvents.Submit, async (e) => {
            e.preventDefault();
            await this.#loginAttempt();
        });
    }


    #loginAttempt = async () => {

        const loginModel = this.#getLoginApiRequestModel();

        try {
            const response = await this.#authService.login(loginModel);
            window.location.href = window.location.href;
        }
        catch (error) {
            alert('Error logging in. Check log');
            console.error(error);
        }


    }

    #getLoginApiRequestModel = () => {
        return new LoginApiRequest(this.#loginUsernameInput.value, this.#loginPasswordInput.value);
    }


    static setupPage = () => {
        const modal = new LoginModalController();
        modal.init();

        return modal;
    }
}


