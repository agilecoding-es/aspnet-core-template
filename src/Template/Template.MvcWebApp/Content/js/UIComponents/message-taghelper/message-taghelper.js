(function ($) {
    $(function () {
        // Function to hide the message element after the specified timeout
        function hideMessageElement(element) {
            let delay = $(element).data('hiding-delay');
            let timeout = $(element).data('hiding-delay') * 1000;

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

        // Hide message elements with the 'data-hiding-delay' attribute
        $('[data-hiding-delay]').each(function () {
            hideMessageElement(this);
        });
    });
})(jQuery);