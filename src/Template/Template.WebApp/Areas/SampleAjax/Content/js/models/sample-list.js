import { SampleItem } from "./sample-item";

export class SampleList {

    constructor(id, listName, items) {
        this.Id = id;
        this.ListName = listName;
        this.Items = items || [];
    }
}