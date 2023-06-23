import { Enums } from './enums';
import { displayType } from './style'

export class Loader {
    constructor(el, type) {
        this.el = el;
        this.type = type || Enums.LoadingType.Div;
    }

    startLoading() {
        if (this.type == Enums.LoadingType.Button) {
            $(this.el).attr('disabled', 'disabled');
            $(this.el).html(`<span class="me-2 spinner-grow spinner-grow-sm text-secondary status-loading" role="status" aria-hidden="true"><span class="visually-hidden"> Loading...</span></span> ${$(this.el).html()}`);
        } else {
            $('<div class="d-flex justify-content-center status-loading">' +
              ' <div class="spinner-grow text-secondary" role="status" >' +
              '   <span class="visually-hidden" >Loading...</span>' +
              ' </div>'+
              '</div>'
            ).appendTo(this.el);
        }

        return this;
    }

    endLoading() {
        $(this.el).children('.status-loading').remove();

        $(this.el).removeAttr("disabled");
        return this;
    }
}