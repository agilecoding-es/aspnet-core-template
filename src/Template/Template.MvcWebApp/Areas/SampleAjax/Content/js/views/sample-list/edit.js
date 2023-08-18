import { SampleList } from "../../models/sample-list";
import { SampleItem } from "../../models/sample-item";
import { SampleListServices } from "../../services/sample-list-controller-services";
import { Loader } from "../../../../../../Content/js/loader"
import { Enums } from "../../../../../../Content/js/enums";
import { MessageHelper } from "../../../../../../Content/js/UIComponents/message-taghelper/message-helper";

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
            addItemValidations: "AddItemValidations"
        };
        this.sampleListServices = sampleListServices;
        this.messageHelper = new MessageHelper();
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

    async submitEditorForm(event) {
        event.preventDefault();

        const $submitEditForm = $(this.elements.submitEditForm);
        const loader = new Loader($submitEditForm, Enums.LoadingType.Button).startLoading();

        const form = $(this.elements.editForm).get(0);
        const formData = new FormData(form);

        try {
            let data = await this.sampleListServices.submitEditForm(formData);
            if (data) {
                this.messageHelper.showMesssage(data.content)
            }
        } catch (error) {
            console.error(error);
        } finally {
            loader.endLoading();
        }
    }

    async submitAddItemForm(event) {
        event.preventDefault();

        const $submitAddItemForm = $(this.elements.submitAddItemForm);
        const loader = new Loader($submitAddItemForm, Enums.LoadingType.Button).startLoading();

        const form = $(this.elements.addItemForm).get(0);
        const formData = new FormData(form);

        try {
            let data = await this.sampleListServices.submitAddItemForm(formData)
            if (data) {
                if (data.success) {
                    const $items = $(this.elements.items);
                    $items.append(data.content);

                    const $descriptionItem = $(this.elements.descriptionItem);
                    $descriptionItem.val('');
                    $descriptionItem.trigger("focus");
                } else {
                    this.messageHelper.showMesssage(data.content)
                }
            }
        } catch (error) {
            console.error(error);
        } finally {
            loader.endLoading();
        }
    }

    submitRemoveItemForm(event) {
        event.preventDefault();

        const $submitRemoveItemForm = $(event.currentTarget);
        const $submitButton = $submitRemoveItemForm.find('button[type="submit"]');
        const loader = new Loader($submitButton, Enums.LoadingType.Button).startLoading();

        const form = $submitRemoveItemForm.get(0);
        const formData = new FormData(form);

        this.sampleListServices
            .submitRemoveItemForm(formData)
            .then(data => {
                if (data) {
                    if (data.success) { }
                    const $itemContainer = $submitRemoveItemForm.parent();
                    $itemContainer.remove();
                } else {
                    this.messageHelper.showMesssage(data.content)
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
