import { SampleList } from "../../models/sample-list";
import { SampleItem } from "../../models/sample-item";
import { SampleListServices } from "../../services/sample-list-controller-services";

export class Index {
    sampleListServices: SampleListServices;
    public Lists: SampleList[];

    constructor(sampleListServices: SampleListServices) {
        this.sampleListServices = sampleListServices;
        this.Lists = [];
        this.init();
    }

    loadSampleLists() {
        this.sampleListServices.get()
            .then(lists => {
                const tbody = document.querySelector('.list-results');

                if (lists.length > 0) {
                    lists.forEach((list, i) => {
                        const tr = document.createElement('tr');
                        const td = document.createElement('td');

                        td.textContent = list.ListName
                        tr.appendChild(td);
                        tbody.appendChild(tr);
                    });
                }
                else {
                    const tr = document.createElement('tr');
                    const td = document.createElement('td');
                    td.textContent = "No items";
                    tr.appendChild(td);
                    tbody.appendChild(tr);
                }

                const listResults = document.querySelector('.list-results');
                listResults.innerHTML = tbody.outerHTML
            })
    }

    init() {
        //this.loadSampleLists();
    }
}

new Index(
    new SampleListServices()
);