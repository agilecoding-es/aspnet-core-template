import { DoFetch } from "../../../../../Content/js/fetch-ajax";

export class UsersServices {
    constructor() {
        this.AreaAndController = 'administration/users';
        this.Actions = {
            ResetPassword: 'ResetPassword',
            Items: 'items',
            Edit: 'edit',
            Delete: 'delete',
            AddItem: 'additem',
            RemoveItem: 'removeitem'
        }
    }

    async resetPasswordAndRedirect(userId) {
        const url = `/${this.AreaAndController}/${this.Actions.ResetPassword}/${userId}`;

        return DoFetch(url, {
            method: 'POST'
        });
    }   
}