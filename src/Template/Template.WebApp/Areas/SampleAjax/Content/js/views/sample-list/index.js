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

    //#region Handlers

    clickOnRemoveList(event) {
        event.preventDefault();

        const $removeListButton = $(event.currentTarget);
        const loader = new Loader($removeListButton, Enums.LoadingType.Button).startLoading();

        const url = $removeListButton.attr('href');
        const $deleteContent = $(this.elements.deleteContent);

        this.sampleListServices.removeList(url)
            .then((data) => {
                if (data) {
                        $deleteContent.html(data);
                }
            })
            .catch(console.error)
            .finally(() => {
                loader.endLoading();
            });;
    }

    clickOnConfirmRemoveList(event) {
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
                    }
                    this.messageHelper.showMesssage(data.content);
                }

            })
            .catch(console.error)
            .finally(() => {
                loader.endLoading();
            });;
    }
    //#endregion Handlers

    closeModal() {
        $(".modal, .modal-backdrop").removeClass("show").addClass("hide");
    }

    bindEvents() {
        $(document)
            .on('click', this.elements.removeList, this.clickOnRemoveList.bind(this))
            .on('click', this.elements.submitDeleteFormButton, this.clickOnConfirmRemoveList.bind(this))
            .on('click', this.elements.submitConfirmDeleteFormButton, this.clickOnConfirmRemoveList.bind(this));

    }
}

new Index(new SampleListServices());