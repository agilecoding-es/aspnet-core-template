export class MessageHelper {

    constructor() {
        
    }

    ShowError(content) {
        try {
            const data = JSON.parse(content);

            if (
                typeof data.Content === 'string' &&
                typeof data.messageType === 'number' &&
                typeof data.messages === 'array'
            ) {
                console.log('JSON data has the expected structure.', data);
            } else {
                console.log('JSON data does not have the expected structure.');
            }
        } catch (error) {
            console.error('Error parsing JSON:', error);
        }
    }
}