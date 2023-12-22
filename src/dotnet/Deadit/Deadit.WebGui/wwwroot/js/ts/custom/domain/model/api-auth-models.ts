

export class LoginApiRequest
{
    username: string;
    password: string;

    constructor(username: string, password: string)
    {
        this.username = username;
        this.password = password;
    }
}

export class SignupApiRequest
{
    email: string;
    username: string;
    password: string;

    constructor(email: string, username: string, password: string) {
        this.email = email;
        this.username = username;
        this.password = password;
    }
}

