//alert("registration.js wurde richtig eingebunden");

//warten bis html-dokument komplett geladen wurde
//statt einer function wird ein lambda ausdruck angegeben (=>)
$(document).ready(() => {
    //alert("registration.js wurde richtig eingebunden + ready");

    // $ entspricht document.getElementById/Class/Tag
    // Schreibweise wie in Css, # für ID, . für Class und Tagnamen um auf Tag zuzugreifen
    //Zugriff auf eine Element mit id = 'EMail'
    //Ereignis Blur wird aufgerufen wenn das zugehörige Element den Focus verliert
    $("#EMail").blur(() => {
        //die engegebene Mail adresse an den Server senden und überprüfen ob die Adresse existiert (gescxhieht über AJAX)
        $.ajax({
            url: "/profile/checkEMail",
            method: "GET",
            data: { email: $("#EMail").val() }
        })
            //Der Aufruf zum Server war erfolgreich
            .done((dataFromServer) => {
                //alert("Data:" + JSON.stringify(dataFromServer));
                if (dataFromServer == true) {
                    //falls die Mail-Adresse vorhanden ist soll eine Value angezeigt und das eingabefeld rot umrandet werden
                    $("#email_message").css("visibility", "visible");
                    //eine CSS Klasse dem Element EMail hinzufügen
                    $("#EMail").addClass("redBorder");
                } else {
                    $("#email_message").css("visibility", "hidden");
                    //eine CSS Klasse dem Element EMail entfernen
                    $("#EMail").removeClass("redBorder");
                }
            })
            //Der Aufruf zum Server war nicht erfolgreich (Server wurde nicht gestartet, die Angegebene URL existiert nicht)
            .fail(() => {
                alert("Server oder URL nicht erreichbar");
            });

    });

    //wird der Button (submit) angeklickt soll das ganze Formular ausgeblendet werden

    $("#btnreset").click(() => {
        $("#FormRegistration").hide(2000);
    });
});
