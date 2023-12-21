import { NativeEvents } from "../../domain/constants/native-events";
import { LoginApiRequest, SignupApiRequest } from "../../domain/model/api-auth-models";
import { AuthService } from "../../services/auth-service";
import { LoginModalElements } from "./LoginModalElements";


export class LoginModalController
{
    private _elements = new LoginModalElements();
    private _authService = new AuthService();

    public init = () => {
        this.addListeners();
    }

    private addListeners = () => {

        this._elements.formLogin.addEventListener(NativeEvents.Submit, async (e) => {
            e.preventDefault();
            await this.loginAttempt();
        });

        this._elements.formSignup.addEventListener(NativeEvents.Submit, async (e) => {
            e.preventDefault();
            await this.signupAttempt();
        });
    }


    private loginAttempt = async () => {

        const loginModel = this.getLoginApiRequestModel();

        try {
            const response = await this._authService.login(loginModel);
            window.location.href = window.location.href;
        }
        catch (error) {
            alert('Error logging in. Check log');
            console.error(error);
        }
    }

    private getLoginApiRequestModel = () => {
        return new LoginApiRequest(this._elements.loginInputUsername.value, this._elements.loginInputPassword.value);
    }


    private signupAttempt = async () => {
        const userData = new SignupApiRequest(this._elements.signupInputEmail.value, this._elements.signupInputUsername.value, this._elements.signupInputPassword.value);

        try {
            const response = await this._authService.signup(userData);
            console.log(response);
            window.location.href = window.location.href;
        }
        catch (error) {
            console.error(error);
            alert('There was an error creating your account. Check logs.');
            return;
        }
    }






    static setupPage = () => {
        const modal = new LoginModalController();
        modal.init();

        return modal;
    }
}


