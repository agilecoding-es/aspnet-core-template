import { SampleList } from "../../models/sample-list";
import { SampleItem } from "../../models/sample-item";
import { SampleListServices } from "../../services/sample-list-controller-services";
import { Loader } from '../../../../../../Content/js/loader'
import { Enums } from "../../../../../../Content/js/enums";

class Edit {
    constructor(sampleListServices) {
        this.elements = {
            listId: "#Id",
            editForm: "form[name=EditForm]",
            submitEditForm: "form[name=EditForm] button[type=submit]",
            itemsWrapper: ".js_Items",
            items: ".js_Items ul",
            addItemForm: "form[name=AddItemForm]",
            submitAddItemForm: "form[name=AddItemForm] button[type=submit]",
            descriptionItem: "form[name=AddItemForm] #Description",
            removeItemForm: "form[name=RemoveItemForm]",
            submitRemoveItemForm: "form[name=RemoveItemForm] button[type=submit]",
        };
        this.sampleListServices = sampleListServices;
        this.bindEvents();

        $(() => {
            this.getListItems();
        });
    }

    getListItems() {
        const $items = $(this.elements.itemsWrapper);
        const listId = $(this.elements.listId).val();
        const loader = new Loader($items, Enums.LoadingType.Div).startLoading();

        this.sampleListServices.getItems(listId)
            .then(response => {
                $items.html(response);
            })
            .finally(() => {
                loader.endLoading();
            });;
    }

    submitEditorForm(event) {
        event.preventDefault();

        const $submitEditForm = $(this.elements.submitEditForm);
        const loader = new Loader($submitEditForm, Enums.LoadingType.Button).startLoading();

        const form = $(this.elements.editForm).get(0);
        const formData = new FormData(form);

        this.sampleListServices
            .submitEditForm(formData)
            .then(response => {
            })
            .catch(error => {
                console.error(error);
            })
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
                if (data && data.content) {
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

    submitRemoveItemForm(event) {
        event.preventDefault();

        const $submitRemoveItemForm = $(this.elements.submitRemoveItemForm);
        const loader = new Loader($submitRemoveItemForm, Enums.LoadingType.Button).startLoading();

        const form = $(this.elements.removeItemForm).get(0);
        const formData = new FormData(form);

        this.sampleListServices
            .submitRemoveItemForm(formData)
            .then(data => {
                if (date & data.isSuccess) {
                    const $itemContainer = $(this.elements.removeItemForm).parent();
                    $itemContainer.remove();
                }
            })
            .catch(error => {
                console.error(error);
            })
            .finally(() => {
                loader.endLoading();
            });;
    }

    bindEvents() {
        $(document)
            .on('submit', this.elements.editForm, this.submitEditorForm.bind(this))
            .on('submit', this.elements.addItemForm, this.submitAddItemForm.bind(this))
            .on('submit', this.elements.removeItemForm, this.submitRemoveItemForm.bind(this));
    }
}

new Edit(new SampleListServices());
