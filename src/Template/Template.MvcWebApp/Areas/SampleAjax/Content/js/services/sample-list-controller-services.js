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

        return await fetch(url, {
            method: 'GET'
        }).then(response => response.json());
    }

    async getItems(listId) {
        const url = `/${this.AreaAndController}/${this.Actions.Items}/${listId}`;

        return await fetch(url, {
            method: 'GET',
        }).then(response => response.text());
    }

    async submitEditForm(formData) {
        const url = `/${this.AreaAndController}/${this.Actions.Edit}`;

        fetch(url, {
            method: 'POST',
            body: formData
        }).then(response => response.text());
    }

    async submitAddItemForm(formData) {
        const url = `/${this.AreaAndController}/${this.Actions.AddItem}`;

        return fetch(url, {
            method: 'POST',
            body: formData
        }).then(response => response.json());
    }

    async submitRemoveItemForm(formData) {
        const url = `/${this.AreaAndController}/${this.Actions.RemoveItem}`;

        return fetch(url, {
            method: 'POST',
            body: formData
        }).then(response => response.json());
    }
}