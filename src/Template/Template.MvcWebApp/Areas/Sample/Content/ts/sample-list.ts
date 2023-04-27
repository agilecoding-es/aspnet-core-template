import { SampleItem } from "./sample-item";

export class SampleList {
    public ListName: string;
    public Items: SampleItem[];

    constructor(listName: string, items: SampleItem[]) {
        this.ListName = listName;
        this.Items = items;

    }
}