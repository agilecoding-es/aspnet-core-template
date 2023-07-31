import { Enums } from "./enums";

export async function ParseResponse(response) {
    if (response && response.ok) {
        const contentType = response.headers.get('content-type');

        if (contentType && contentType.includes('application/json')) {
            const tempResponse = await response.json();

            if (tempResponse && tempResponse.messageType && tempResponse.messageType == Enums.ResponseType.Error) {
                throw tempResponse.content;
            }
            return tempResponse;
        } else if (contentType && (contentType.includes('text/plain') || contentType.includes('text/html')) ) {
            return await response.text();
        } else if (contentType && contentType.includes('application/octet-stream')) {
            return await response.blob();
        } else {
            throw new Error('Tipo de contenido desconocido');
        }
    }
}

export function DoFetch(url, options) {
    const headers = options && options.headers ? { ...options.headers } : {};
    headers['X-Requested-With'] = 'XMLHttpRequest';

    return fetch(url, { ...options, headers })
        .then(ParseResponse)
        .catch(console.error);
}