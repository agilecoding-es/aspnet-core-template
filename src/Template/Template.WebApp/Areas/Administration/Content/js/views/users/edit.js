import { UsersServices } from "../../services/users-controller-services";
import { Loader } from "../../../../../../Content/js/loader"
import { Enums } from "../../../../../../Content/js/enums";
import { MessageHelper } from "../../../../../../Content/js/ui-components/message-taghelper/message-helper";

class Edit {

    constructor(usersServices) {
        this.elements = {
            buttonResendConfirmation: "#resendConfirmation",
        };
        this.usersServices = usersServices;
        this.messageHelper = new MessageHelper();
        this.bindEvents();

        $(() => {
        });
    }

    async clickon_buttonResendConfirmation(event) {
        event.preventDefault();
        const $buttonResendConfirmation = $(this.elements.buttonResendConfirmation);
        const loader = new Loader($buttonResendConfirmation, Enums.LoadingType.Button).startLoading();

        try {
            const userId = $buttonResendConfirmation.data("userid");
            await this.usersServices.resetPasswordAndRedirect(userId);
        } catch (error) {
            console.error(error);
        } finally {
            loader.endLoading();
        }
    }

    bindEvents() {
        $(document)
            .on('click', this.elements.buttonResendConfirmation, this.clickon_buttonResendConfirmation.bind(this));
    }
}

new Edit(new UsersServices());
