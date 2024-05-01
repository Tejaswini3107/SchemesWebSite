
    var lang = "";
    var CustomerID = 0;

function fromView(language, customerID) {
    CustomerID = customerID;
    lang = language;
}
 var selectedLanguage = lang;


$('#language').on('change', function () {

    var selectedLanguage = document.getElementById('language').value;
    $("#selectedlanguage").val(selectedLanguage);
    $("#langaugeModal").modal('show');
    $("#langaugeModal").find(".modal-body").html("Language Changed to " + selectedLanguage);
    $('#language').val(selectedLanguage);
    });

    $("#langaugeModal .okbtn").on("click", function () {
    var currentURL = window.location.href.toString();
    var selectedLanguage = $("#selectedlanguage").val();

    // Define the substring to check
    var substring = "home/homepage";

    // Check if the current URL contains the substring
    if (currentURL.toLowerCase().indexOf(substring) !== -1) {
        $.ajax({
            url: "/Home/HomePage",
            type: "GET",
            data: {
                customerID: CustomerID,
                selectedlang: selectedLanguage
            },
            success: function (data) {
                // Handle success response
            },
            error: function (err) {
                alert("Error: " + err.statusText);
            }
        });
                }
    else {
        $.ajax({
            url: "/Home/GetSchemesList",
            type: "GET",
            data: {
                name: $("#schemeName").html(),
                langCode: selectedLanguage
            },
            success: function (data) {
                // Handle success response
            },
            error: function (err) {
                alert("Error: " + err.statusText);
            }
        });
                }
            });

    $("#schemeName").on('click', function () {
        $.ajax({
            url: "/Home/GetSchemesList?name=" + $("#schemeName").html() + "&langCode=" + selectedLanguage,
            type: "GET",
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (data) {

            },
            error: function (err) {
                alert(err);
            }
        });
            });

    $("#disable").hide();

    $(function () {
        $("#addClass").on('click',function () {
            $('#sidebar_secondary').addClass('popup-box-on');
        });
        $(".open-btn").on('click', function () {
        $('#sidebar_secondary').removeClass('d-none');
    $('.chatButtonDiv').addClass('d-none');

                });
        $("#removeClass").on('click', function () {
        $('#sidebar_secondary').removeClass('popup-box-on');
                });


    $('#sidebar_secondary').on('click', 'span.uk-input-group-addon a', function () {
                    var message = $('#submit_message').val(); // Get entered text

    $('.chat_box').append('<div class="chat_message_wrapper chat_message_right">' +
        '<ul class="chat_message "><li><p>' + message + '</p></li></ul></div>');
    $('#submit_message').val('');
    $.ajax({
        type: 'GET',
    url: "Home/Chatbot?message=" + message,
    contentType: 'application/json',
    success: function (response) {
        $('.chat_box').append('<div class="chat_message_wrapper"><div class="chat_user_avatar"><img src = "https://bootdey.com/img/Content/avatar/avatar1.png" class="md-user-image"></div>' +
            '<ul class="chat_message"><li><p>' + response + '</p></li></ul></div>');
                        },
    error: function (xhr, status, error) {
        console.error('Error:', error);
                        }
                    });
                });
    $(".closeBtn").on('click', function () {
        $('#sidebar_secondary').addClass('d-none');
    $('.chatButtonDiv').removeClass('d-none');

                });

            });
    $('#sidebarToggle').on('click',function () {
        $('#accordionSidebar').toggleClass('sidebar-hidden');
    });

//if (selectedLanguage == "hi") {
//    $('.banks').text('बैंकों');
//    $('.emiHeader').text('ईएमआई कैलकुलेटर');
//    $('.resultsLbl').text('परिणाम');
//    $('#loanEMITxt').text('ऋण ईएमआई: ₹');
//    $('#TIPTxt').text('कुल देय ब्याज: ₹');
//    $('#TPTxt').text('कुल भुगतान (मूलधन+ब्याज): ₹');
//    $('#loanTenureLbl').text('ऋण अवधि (वर्ष)');
//    $('#interestRateLbl').text('ब्याज दर (%)');
//    $('#loanAmountLbl').text('ऋण राशि(₹)');
//    $('#KARNATAKALoanEMI').text('कर्नाटक');
//    $('#SBILoanEMI').text('स्टे.बैं.इं');
//    $('#UNIONLoanEMI').text('यूनियन');

//}
//else if (selectedLanguage == "kn") {
//    $('.banks').text('');
//    $('.emiHeader').text('');
//    $('.resultsLbl').text('');
//    $('#loanEMITxt').text('');
//    $('#TIPTxt').text('');
//    $('#TPTxt').text('');
//    $('#loanTenureLbl').text('');
//    $('#interestRateLbl').text('');
//    $('#loanAmountLbl').text('');
//    $('#KARNATAKALoanEMI').text('');
//    $('#SBILoanEMI').text('');
//    $('#UNIONLoanEMI').text('');
//}
//else if (selectedLanguage == "ml") {
//    $('.banks').text('');
//    $('.emiHeader').text('');
//    $('.resultsLbl').text('');
//    $('#loanEMITxt').text('');
//    $('#TIPTxt').text('');
//    $('#TPTxt').text('');
//    $('#loanTenureLbl').text('');
//    $('#interestRateLbl').text('');
//    $('#loanAmountLbl').text('');
//    $('#KARNATAKALoanEMI').text('');
//    $('#SBILoanEMI').text('');
//    $('#UNIONLoanEMI').text('');
//}
//else if (selectedLanguage == "ta") {
//    $('.banks').text('');
//    $('.emiHeader').text('');
//    $('.resultsLbl').text('');
//    $('#loanEMITxt').text('');
//    $('#TIPTxt').text('');
//    $('#TPTxt').text('');
//    $('#loanTenureLbl').text('');
//    $('#interestRateLbl').text('');
//    $('#loanAmountLbl').text('');
//    $('#KARNATAKALoanEMI').text('');
//    $('#SBILoanEMI').text('');
//    $('#UNIONLoanEMI').text('');
//}
//else if (selectedLanguage == "te") {
//    $('.banks').text('');
//    $('.emiHeader').text('');
//    $('.resultsLbl').text('');
//    $('#loanEMITxt').text('');
//    $('#TIPTxt').text('');
//    $('#TPTxt').text('');
//    $('#loanTenureLbl').text('');
//    $('#interestRateLbl').text('');
//    $('#loanAmountLbl').text('');
//    $('#KARNATAKALoanEMI').text('');
//    $('#SBILoanEMI').text('');
//    $('#UNIONLoanEMI').text('');
//}
//else if (selectedLanguage == "ur") {
//    $('.banks').text('');
//    $('.emiHeader').text('');
//    $('.resultsLbl').text('');
//    $('#loanEMITxt').text('');
//    $('#TIPTxt').text('');
//    $('#TPTxt').text('');
//    $('#loanTenureLbl').text('');
//    $('#interestRateLbl').text('');
//    $('#loanAmountLbl').text('');
//    $('#KARNATAKALoanEMI').text('');
//    $('#SBILoanEMI').text('');
//    $('#UNIONLoanEMI').text('');
//}