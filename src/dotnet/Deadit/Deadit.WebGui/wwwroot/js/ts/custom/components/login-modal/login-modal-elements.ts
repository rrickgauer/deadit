
export class LoginModalElements 
{
    public modalContainer = document.querySelector('#login-container') as HTMLDivElement;
    public loginForm = this.modalContainer.querySelector('.login-form-login') as HTMLFormElement;
    public signupForm = this.modalContainer.querySelector('.signup-form') as HTMLFormElement;
}
