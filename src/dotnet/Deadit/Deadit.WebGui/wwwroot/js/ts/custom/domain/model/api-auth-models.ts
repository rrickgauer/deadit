

export class LoginApiRequest
{
    username;
    password;

    constructor(username, password) {
        this.username = username;
        this.password = password;
    }
}

export class SignupApiRequest
{
    email;
    username;
    password;
}

