import { SampleItem } from "./sample-item";

export class SampleList {
    public Id: string;
    public ListName: string;
    public Items: SampleItem[];

    constructor(id: string, listName: string, items: SampleItem[]) {
        this.Id = id;
        this.ListName = listName;
        this.Items = items;

    }
}