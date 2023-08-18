(function ($) {
    $(function () {
        // Function to hide the message element after the specified timeout
        function hideMessageElements(container, element) {
            let delay = $(container).data('hiding-delay');
            let timeout = delay * 1000;

            if (timeout) {
                let progressBar = $(element).find('.message-progress');
                let increment = 10 / delay;
                let currentWidth = 0;

                let intervalId = setInterval(function () {
                    if (currentWidth + increment < 100) {
                        currentWidth += increment;
                        progressBar.css('width', currentWidth + '%');
                    } else {
                        progressBar.css('width', '100%');
                    }
                }, 100);

                setTimeout(function () {
                    clearInterval(intervalId);
                    $(element).fadeOut('slow', function () {
                        $(this).remove();
                    });
                }, timeout);
            }
        }

        $(document).on('show', '[data-hiding-delay] .message-item', function () {
            const container = $('[data-hiding-delay]');
            let newMessageItem = $('[data-hiding-delay] .message-item:last-child');
            console.log('show');
            hideMessageElements(container, newMessageItem);
        });

        $(document).on('hide', '[data-hiding-delay] .message-item', function () {
            console.log('hide');
        });
    });
})(jQuery);