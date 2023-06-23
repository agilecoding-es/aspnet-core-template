import { SampleList } from "../models/sample-list";

export class SampleListServices {
    url = {
        AreaAndController: 'sample/samplelist',
        List: 'list'
    }

    constructor() {

    }

    async get():Promise<SampleList[]> {
        return await fetch(`/${this.url.AreaAndController}/${this.url.List}`, {
            method: 'GET'
        }).then(response => response.json());
    }
}