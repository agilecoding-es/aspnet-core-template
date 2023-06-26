import { DoFetch } from "../../../../../Content/js/fetch-ajax";
import { SampleItem } from "../models/sample-item";
import { SampleList } from "../models/sample-list";


export class SampleListServices {
    

    constructor() {
        this.AreaAndController= 'sampleajax/samplelist';
        this.Actions = {
            List: 'list',
            Items: 'items',
            Edit: 'edit',
            AddItem: 'additem',
            RemoveItem: 'removeitem'
        }
    }

    async getLists() {
        const url = `/${this.AreaAndController}/${this.Actions.List}`;

        return DoFetch(url, {
            method: 'GET'
        });
    }

    async removeList(url) {
        return DoFetch(url, {
            method: 'GET'
        });
    }

    async submitEditForm(formData) {
        const url = `/${this.AreaAndController}/${this.Actions.Edit}`;

        return DoFetch(url, {
            method: 'POST',
            body: formData
        });
    }

    async getItems(listId) {
        const url = `/${this.AreaAndController}/${this.Actions.Items}/${listId}`;

        return DoFetch(url, {
            method: 'GET',
        });
    }

    async submitAddItemForm(formData) {
        const url = `/${this.AreaAndController}/${this.Actions.AddItem}`;

        return DoFetch(url, {
            method: 'POST',
            body: formData
        });
    }

    async submitRemoveItemForm(formData) {
        const url = `/${this.AreaAndController}/${this.Actions.RemoveItem}`;

        return DoFetch(url, {
            method: 'POST',
            body: formData
        });
    }
}