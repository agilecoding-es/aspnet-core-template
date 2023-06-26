import { Enums } from "../../../../../../Content/js/enums";
import { Loader } from "../../../../../../Content/js/loader";
import { SampleListServices } from "../../services/sample-list-controller-services";

export class Index {

    constructor(sampleListServices) {
        this.elements = {
            editList: ".js_EditList",
            removeList: ".js_RemoveList",
            deleteContent: ".js_deleteContent",
            submitDeleteFormButton: "form[name=EditForm] button[type=submit]",
        };
        this.sampleListServices = sampleListServices;
        this.Lists = [];
        this.bindEvents();
    }

    removeList(event) {
        event.preventDefault();

        const $removeListButton = $(event.currentTarget);
        const loader = new Loader($removeListButton, Enums.LoadingType.Button).startLoading();

        const url = $removeListButton.attr('formaction');
        const $deleteContent = $(this.elements.deleteContent);

        this.sampleListServices
            .removeList(url)
            .then((data) => {
                if (data)
                    $deleteContent.html(data);
            })
            .catch(console.error)
            .finally(() => {
                loader.endLoading();
            });;
    }

    confirmRemoveList(event) {
        event.preventDefault();

        const $removeListButton = $(event.currentTarget);
        const loader = new Loader($removeListButton, Enums.LoadingType.Button).startLoading();

        const url = $removeListButton.attr('formaction');

        this.sampleListServices
            .removeList(url)
            .catch(console.error)
            .finally(() => {
                loader.endLoading();
            });;
    }

    submitAddItemForm(event) {
        event.preventDefault();

        const $submitAddItemForm = $(this.elements.submitAddItemForm);
        const loader = new Loader($submitAddItemForm, Enums.LoadingType.Button).startLoading();

        const form = $(this.elements.addItemForm).get(0);
        const formData = new FormData(form);

        this.sampleListServices
            .submitAddItemForm(formData)
            .then(data => {
                if (data) {
                    const $items = $(this.elements.items);
                    $items.append(data.content);

                    const $descriptionItem = $(this.elements.descriptionItem);
                    $descriptionItem.val('');
                    $descriptionItem.trigger("focus");
                }
            })
            .catch(error => {
                console.error(error);
            })
            .finally(() => {
                loader.endLoading();
            });
    }

    bindEvents() {
        $(document)
            .on('click', this.elements.removeList, this.removeList.bind(this))
            .on('click', this.elements.submitDeleteFormButton, this.confirmRemoveList.bind(this));

    }
}

new Index(new SampleListServices());