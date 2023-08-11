import { Enums } from "../../../../../../Content/js/enums";
import { Loader } from "../../../../../../Content/js/loader";
import { SampleListServices } from "../../services/sample-list-controller-services";
import { MessageHelper } from "../../../../../../Content/js/UIComponents/message-taghelper/message-helper";

export class Index {

    constructor(sampleListServices) {
        this.elements = {
            editList: ".js_EditList",
            removeList: ".js_RemoveList",
            deleteContent: ".js_deleteContent",
            submitDeleteFormButton: ".js_DeleteButton",
            submitConfirmDeleteFormButton: ".js_ConfirmDeleteButton",
        };
        this.sampleListServices = sampleListServices;
        this.Lists = [];
        this.messageHelper = new MessageHelper();
        this.bindEvents();
    }

    removeList(event) {
        event.preventDefault();

        const $removeListButton = $(event.currentTarget);
        const loader = new Loader($removeListButton, Enums.LoadingType.Button).startLoading();

        const url = $removeListButton.attr('href');
        const $deleteContent = $(this.elements.deleteContent);

        this.sampleListServices.removeList(url)
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
        const $form = $removeListButton.parent("form");
        const loader = new Loader($removeListButton, Enums.LoadingType.Button).startLoading();

        const url = $form.attr('action');
        const $deleteContent = $(this.elements.deleteContent);

        const form = $form.get(0);
        const formData = new FormData(form);

        this.sampleListServices.submitRemoveForm(formData)
            .then(data => {
                if (data) {
                    if (data.success) {
                        this.closeModal();
                        const listId = $removeListButton.data("id");
                        $(".js_Items").find(`[data-id=${listId}]`).remove();
                        this.messageHelper.showMesssage(data.content);
                    }
                }

            })
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

    closeModal() {
        $(".modal, .modal-backdrop").removeClass("show").addClass("hide");
    }

    bindEvents() {
        $(document)
            .on('click', this.elements.removeList, this.removeList.bind(this))
            .on('click', this.elements.submitDeleteFormButton, this.confirmRemoveList.bind(this))
            .on('click', this.elements.submitConfirmDeleteFormButton, this.confirmRemoveList.bind(this));

    }
}

new Index(new SampleListServices());