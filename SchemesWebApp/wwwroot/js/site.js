function insertMessage() {
    var msg = $('.message-input').val().trim();
    if (msg === '') {
        return false;
    }

    // Append user message to the chat interface
    $('<div class="message message-personal">' + msg + '</div>').appendTo($('.mCSB_container')).addClass('new');
    setDate();
    $('.message-input').val(null);
    updateScrollbar();

    // Send user message to the API
    $.ajax({
        type: 'POST',
        url: 'https://349f-106-79-207-115.ngrok-free.app/bot',
        contentType: 'application/json',
        data: JSON.stringify({ message: msg }),
        success: function (response) {
            // Append API response as chatbot message to the chat interface
            $('<div class="message new"><figure class="avatar"><img src="https://s3-us-west-2.amazonaws.com/s.cdpn.io/156381/profile/profile-80.jpg" /></figure>' + response + '</div>').appendTo($('.mCSB_container')).addClass('new');
            setDate();
            updateScrollbar();
        },
        error: function (xhr, status, error) {
            console.error('Error:', error);
            // Handle error if needed
        }
    });

    // Simulate chatbot typing
    $('<div class="message loading new"><figure class="avatar"><img src="https://s3-us-west-2.amazonaws.com/s.cdpn.io/156381/profile/profile-80.jpg" /></figure><span></span></div>').appendTo($('.mCSB_container'));
    updateScrollbar();

    // Simulate delay before showing chatbot response
    setTimeout(function () {
        $('.message.loading').remove();
        fakeMessage(); // Call fakeMessage to generate chatbot response
    }, 1000 + (Math.random() * 20) * 100);
}
