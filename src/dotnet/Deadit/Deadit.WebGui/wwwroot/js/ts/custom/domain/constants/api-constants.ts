export const URL_PREFIX = '/api';

// All the various api endpoints
export enum ApiEndpoints 
{
    LOGIN         = `${URL_PREFIX}/auth/login`,
    SIGNUP        = `${URL_PREFIX}/auth/signup`,
    COMMUNITY     = `${URL_PREFIX}/communities`,
    VOTES_COMMENT = `${URL_PREFIX}/votes/comments`,
    VOTES_POST    = `${URL_PREFIX}/votes/posts`,
    POSTS         = `${URL_PREFIX}/posts`,
}

export enum HttpMethods {
    POST   = 'POST',
    GET    = 'GET',
    PUT    = 'PUT',
    DELETE = 'DELETE',
    PATCH  = 'PATCH',
}