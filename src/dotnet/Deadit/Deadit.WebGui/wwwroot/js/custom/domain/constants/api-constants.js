export const URL_PREFIX = '/api';

export const ApiEndpoints = {
    LOGIN              : `${URL_PREFIX}/auth/login`,
    // EVENTS             : `${URL_PREFIX}/events`,
    // RECURRENCES        : `${URL_PREFIX}/recurrences`,
    // COMPLETIONS        : `${URL_PREFIX}/completions`,
    // USER               : `${URL_PREFIX}/user`,
    // PASSWORD           : `${URL_PREFIX}/password`,
    // EMAIL_VERIFICATIONS: `${URL_PREFIX}/email-verifications`,
    // LABELS             : `${URL_PREFIX}/labels`,
    // EVENT_LABELS       : `${URL_PREFIX}/events`,
    // CHECKLISTS         : `${URL_PREFIX}/checklists`,
}

export const HttpMethods = {
    POST  : 'POST',
    GET   : 'GET',
    PUT   : 'PUT',
    DELETE: 'DELETE',
    PATCH : 'PATCH',
}