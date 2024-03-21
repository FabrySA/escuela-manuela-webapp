$(document).ready(function () {

    //-------------------------Login--------------------------

    if ($('#loginForm').length > 0) {

        let borderDefault = $("#user").css("border-color");

        $("#loginForm").submit(function (event) {

            var userField = $("#user").val().trim();
            var passField = $("#pass").val().trim();

            if (userField === "") {
                event.preventDefault();
                $(".wrong").attr("hidden", true);
                $("#missingUser").removeAttr('hidden');
                $('#user').css('border-color', 'red');
            }

            if (passField === "") {
                event.preventDefault();
                $(".wrong").attr("hidden", true);
                $("#missingPass").removeAttr('hidden');
                $('#pass').css('border-color', 'red');
            }
        });

        $(".form-control").on("input", function () {

            $('#user').css('border-color', borderDefault);
            $('#pass').css('border-color', borderDefault);
            $(".warnings").attr("hidden", true);

        });
    }

    //------------------------- Crear Cuenta --------------------------

    if ($('#signUpForm').length > 0) {

        let borderDefault = $("#mail").css("border-color");

        $("#signUpForm").submit(function (event) {

            var emailField = $("#mail").val().trim();
            var nameField = $("#name").val().trim();
            var lst1Field = $("#lastname1").val().trim();
            var lst2Field = $("#lastname2").val().trim();
            var roleField = $("#role").val().trim();

            var hasErrors = false;

            if (roleField === "") {

                $("#missingRole").removeAttr('hidden');
                $('#role').css('border-color', 'red');
                hasErrors = true;
            }

            if (emailField === "" || !isMailValid()) {

                $("#missingMail").removeAttr('hidden');
                $('#mail').css('border-color', 'red');
                hasErrors = true;
            }

            if (nameField === "") {

                $("#missingName").removeAttr('hidden');
                $('#name').css('border-color', 'red');
                hasErrors = true;
            }

            if (lst1Field === "") {

                $("#missingLast1").removeAttr('hidden');
                $('#lastname1').css('border-color', 'red');
                hasErrors = true;
            }

            if (lst2Field === "") {

                $("#missingLast2").removeAttr('hidden');
                $('#lastname2').css('border-color', 'red');
                hasErrors = true;
            }

            if (hasErrors) {
                event.preventDefault();
            }
        });

        //-----Verificaciones en tiempo real del campo de correo---------

        //Verificar si Correo es vállido con Regex
        function isMailValid() {
            var mail = $("#mail").val().trim();
            var mailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

            return mailRegex.test(mail);
        }

        //Validar Campos de Correo
        function validateMail() {
            if (isMailValid()) {
                $("#missingMail").attr("hidden", true);
                $('#mail').css('border-color', borderDefault);
            } else {
                $("#missingMail").removeAttr('hidden');
                $('#mail').css('border-color', 'red');
            }
        }

        // KeyUp Mail
        $("#mail").on("keyup", function () {
            validateMail();
        });

        // Blur Mail
        $("#mail").blur(function () {
            validateMail();
        });

        // cut/copy/paste Mail
        $('#mail').on("cut copy paste", function (e) {
            validateMail();
        });
    }

    //-------------------------Request Password Reset--------------------------

    if ($('#requestPassRstForm').length > 0) {

        $("#requestPassRstForm").submit(function (event) {

            var emailField = $("#mail").val().trim();
            var hasErrors = false;

            if (emailField === "" || !isMailValid()) {

                $("#missingMail").removeAttr('hidden');
                $('#mail').css('border-color', 'red');
                hasErrors = true;
            }

            if (hasErrors) {
                event.preventDefault();
            }
        });

        //Verificaciones en tiempo real del campo de correo

        //Verificar si Correo es válido con Regex
        function isMailValid() {
            var mail = $("#mail").val().trim();
            var mailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

            return mailRegex.test(mail);
        }

        //Validar Campos de Correo
        function validateMail() {
            if (isMailValid()) {
                $("#missingMail").attr("hidden", true);
                $('#mail').css('border-color', borderDefault);
            } else {
                $("#missingMail").removeAttr('hidden');
                $('#mail').css('border-color', 'red');
            }
        }

        // KeyUp Mail
        $("#mail").on("keyup", function () {
            validateMail();
        });

        // Blur Mail
        $("#mail").blur(function () {
            validateMail();
        });

        // cut/copy/paste Mail
        $('#mail').on("cut copy paste", function (e) {
            validateMail();
        });


    }

    //-------------------------Password Reset--------------------------

    if ($('#resetPassForm').length > 0) {
        //---------------Validación de Creación de Cuenta--------------

        //Tomar el color original del borde de los campos
        let borderDefault = $("#pass").css("border-color");

        //Verificar si los campos están vacíos al enviar el formulario
        $("#resetPassForm").submit(function (event) {

            var passField = $("#pass").val().trim();
            var repeatPassField = $("#repeatPass").val().trim();

            var hasErrors = false;

            if (passField === "") {

                $("#missingPass").removeAttr('hidden');
                $('#pass').css('border-color', 'red');
                hasErrors = true;
            }

            if (repeatPassField === "" || repeatPassField !== passField) {

                $("#passMatch").removeAttr('hidden');
                $('#repeatPass').css('border-color', 'red');
                hasErrors = true;
            }

            if (hasErrors) {
                event.preventDefault();
            }
        });

        //-----Verificaciones en tiempo real del campo de contraseña---------

        //Prevenir copiar, pegar y cortar en los campos de contraseña
        $('.passwords').on("cut copy paste", function (e) {
            e.preventDefault();
        });

        //Verificar si ambas contraseñas son iguales en evento KeyUp
        $(".passwords").on("keyup", function () {

            var passField = $("#pass").val().trim();
            var repeatPassField = $("#repeatPass").val().trim();

            if (repeatPassField !== passField) {

                $("#passMatch").removeAttr('hidden');
                $('#repeatPass').css('border-color', 'red');
            } else {

                $("#passMatch").attr("hidden", true);
                $('#repeatPass').css('border-color', borderDefault);
            }
        });


        //Remover advertencias si se comienza a escribir en el campo

        //Campos Contraseña
        $("#pass").on("input", function () {

            $('#pass').css('border-color', borderDefault);
            $("#missingPass").attr("hidden", true);
        });
    }

    //-------------------------Botón para mostrar contraseña--------------------------

    if ($('#togglePassword').length > 0) {


        $("#togglePassword").click(function () {

            var pass = $("#pass");
            var repeatPass = $("#repeatPass");
            var toggle = $("#togglePassword");

            var passType = pass.attr("type");

            if (passType === "password") {
                pass.attr("type", "text");
                repeatPass.attr("type", "text");
                toggle.find("i").removeClass("fa-eye").addClass("fa-eye-slash");
            } else {
                pass.attr("type", "password");
                repeatPass.attr("type", "password");
                toggle.find("i").removeClass("fa-eye-slash").addClass("fa-eye");
            }
        });
    }

    //-------------------Premission & Inability Forms------------------------
    if ($('#formularyForm').length > 0) {

        //Signature Pad
        $(function () {

            var canvas = document.querySelector('#signature');
            var pad = new SignaturePad(canvas);

            $('#clear').click(function () {
                pad.clear();
            });

            $('#submitBTN').click(function () {

                var data = pad.toDataURL();
                $('#SignatureDataUrl').val(data);
            });

        });

        let borderDefault = $("#name").css("border-color");

        //Verificar si los campos están vacíos al enviar el formulario
        $("#formularyForm").submit(function (event) {

            event.preventDefault();
            var allRequiredFilled = true;

            var canvas = document.querySelector('#signature');

            $('.requiredfields').each(function () {

                if ($(this).val().trim() === '') {
                    allRequiredFilled = false;
                    if ((this.id).length > 0) {
                        $('#' + this.id).css('border-color', 'red');
                    }
                } else {
                    if ((this.id).length > 0) {
                        $('#' + this.id).css('border-color', borderDefault);
                    }
                }
            });

            if (isCanvasEmpty(canvas)) {
                allRequiredFilled = false;
                $("#missingSignature").removeAttr('hidden');
            } else {
                $("#missingSignature").attr("hidden", true);
            }

            if (!allRequiredFilled) {
                $("#missingFields").removeAttr('hidden');
            } else {
                $("#missingFields").attr("hidden", true);
                Swal.fire({
                    title: "¿Desea subir el formulario?",
                    text: "Revise que haya incluído la información correcta",
                    icon: "question",
                    showCancelButton: true,

                    customClass: {
                        confirmButton: 'vertical-button',
                        cancelButton: 'vertical-button',
                    },

                    cancelButtonText: "Cancelar",
                    cancelButtonColor: "#6c757d",

                    confirmButtonColor: "#28a745",
                    confirmButtonText: "Aceptar",

                }).then((result) => {
                    if (result.isConfirmed) {
                        $('#formularyForm').unbind('submit').submit();
                    }
                });

            }

            function isCanvasEmpty(canvas) {
                var context = canvas.getContext('2d');
                var pixelData = context.getImageData(0, 0, canvas.width, canvas.height).data;
                for (var i = 0; i < pixelData.length; i += 4) {
                    if (pixelData[i + 3] !== 0) {
                        return false; // Canvas is not empty
                    }
                }
                return true; // Canvas is empty
            }

        });


        $("#reasonInab").change(function () {

            var selected = $(this).val();

            if (selected === "otro") {

                $("#otherReason").addClass("requiredfields").val("")
                $("#otherDiv").removeAttr('hidden')
            } else {

                $("#otherReason").removeClass("requiredfields").val("").css('border-color', borderDefault)
                $("#otherDiv").attr("hidden", true)
            }
        });
    }

    //-------------------Uploading Files (PublicFile, Plan, MiscFile)-----------------------
    if ($('#filesForm').length > 0) {

        let borderDefault = $("#name").css("border-color");

        $("#filesForm").submit(function (event) {

            event.preventDefault();
            var allRequiredFilled = true;

            $('.requiredfields').each(function () {

                if ($(this).val().trim() === '') {

                    allRequiredFilled = false;

                    if ((this.id).length > 0) {
                        $('#' + this.id).css('border-color', 'red');
                    }

                } else {

                    if ((this.id).length > 0) {
                        $('#' + this.id).css('border-color', borderDefault);
                    }
                }
            });

            if (!isFileValid() && $('#fileInput').val().trim() !== "") {
                allRequiredFilled = false;
                $("#badFile").removeAttr('hidden');
            } else {
                $("#badFile").attr("hidden", true);
            }

            if (!allRequiredFilled) {
                $("#missingFields").removeAttr('hidden');
            } else {
                $("#missingFields").attr("hidden", true);
                Swal.fire({
                    title: "¿Desea subir este archivo?",
                    text: "Revise que haya incluído la información correcta",
                    icon: "question",
                    showCancelButton: true,


                    customClass: {
                        confirmButton: 'vertical-button',
                        cancelButton: 'vertical-button',
                    },

                    cancelButtonText: "Cancelar",
                    cancelButtonColor: "#6c757d",

                    confirmButtonColor: "#28a745",
                    confirmButtonText: "Aceptar",

                }).then((result) => {
                    if (result.isConfirmed) {
                        $('#filesForm').unbind('submit').submit();
                    }
                });
            }
        });

        $('#fileInput').change(function () {

            if (!isFileValid() && $('#fileInput').val().trim() !== "") {
                $("#badFile").removeAttr('hidden');
            } else {
                $("#badFile").attr("hidden", true);
            }
        });
    }

    //-------------------Delete Files-----------------------
    if ($('#deleteBTN').length > 0) {


    }
});


//-------------------Verify Extension when Uploading Files------------------------
function isFileValid() {
    var allowedExtensions = ['.pdf', '.doc', '.docx', '.xls', '.xlsx', '.ppt', '.pptx', '.txt', '.rtf', '.odt', '.xlsm', '.xlsb'];
    var fileName = $('#fileInput').val();

    var fileExtension = fileName.split('.').pop();

    if (allowedExtensions.indexOf('.' + fileExtension.toLowerCase()) === -1) {
        return false;
    }
    return true;
}

//Generar Contraseña Aleatoria para el 'Admin Reset'
function generateRandomPass() {

    const length = 12;

    var pass = $("#pass");
    var repeatPass = $("#repeatPass");

    const allowedChars = 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@$?_-';
    let password = '';

    for (let i = 0; i < length; i++) {
        const randomIndex = Math.floor(Math.random() * allowedChars.length);
        password += allowedChars.charAt(randomIndex);
    }

    pass.val(password);
    repeatPass.val(password);
}

function deleteFile(type, id, dataTableID, itemName) {

    var htmlConsec = '';

    if (type === 'permission') {

        var showDeny = true;
        htmlConsec = '<br><br><h5>Al seleccionar <strong>Eliminar Con Consecutivo</strong>, ' +
            'permitirá que el próximo registro pueda usar este consecutivo si corresponde<h5>';
    }

    Swal.fire({

        title: "¿Desea eliminar este archivo?" + "<br><br><code><small>" + itemName + "</small></code>",
        html: "No podrá recuperarlo después" + htmlConsec,
        icon: "question",
        showDenyButton: showDeny,
        showCancelButton: true,

        customClass: {
            confirmButton: 'vertical-button',
            cancelButton: 'vertical-button',
            denyButton: 'vertical-button',
        },


        confirmButtonColor: "#dc3545",
        confirmButtonText: "Eliminar",

        denyButtonColor: "#fd7e14",
        denyButtonText: `Eliminar Con Consecutivo`,

        cancelButtonColor: "#6c757d",
        cancelButtonText: "Cancelar",

    }).then((result) => {

        if (result.isConfirmed || result.isDenied) {

            var table = $('#' + dataTableID).DataTable();
            var removingRow = $('#' + id).closest('tr');
            var consec = "";

            if (result.isDenied) {
                var rowIndex = table.row(removingRow).index();
                consec = table.cell(rowIndex, 0).data();
            }

            $.ajax({
                url: '/FileManager/DeleteFile',
                type: 'DELETE',
                data: { type: type, id: id, consec: consec },

                success: function (response) {
                    Swal.fire({
                        title: "¡Eliminado!",
                        text: "Se ha eliminado el archivo",
                        icon: "success"
                    });

                    table.row(removingRow).remove().draw();
                },
                error: function (xhr, ajaxOptions, thrownError) {

                    Swal.fire({
                        title: "¡Error!",
                        text: "Error al eliminar el archivo",
                        icon: "error"
                    });
                }
            });
        }
    });

}

function deleteAccount(id, usermail) {

    var table = $('#userListDataTable').DataTable();
    var removingRow = $('#' + id).closest('tr');

    Swal.fire({

        title: "¿Desea eliminar este usuario?" + "<br><br><code><small>" + usermail + "</small></code>",

        html: "Se eliminarán <strong>todos los archivos</strong> asociados a este usuario así como su acceso al sistema" +
            "<br><br>Si desea continuar ingrese la palabra <strong>confirmar</strong> en el campo de abajo y presione <strong>ELIMINAR</strong>",

        icon: "question",

        showCancelButton: true,
        confirmButtonText: "Eliminar",
        confirmButtonColor: "#dc3545",

        cancelButtonText: "Cancelar",
        cancelButtonColor: "#6c757d",

        customClass: {
            confirmButton: 'vertical-button',
            cancelButton: 'vertical-button',
        },

        showLoaderOnConfirm: true,
        input: "text",
        inputAttributes: {
            autocapitalize: "off"
        },

        preConfirm: async (inputValue) => {

            if (inputValue.toLowerCase() !== 'confirmar') {
                Swal.showValidationMessage("Por favor, ingrese la palabra correcta");
            }
        },
        allowOutsideClick: () => !Swal.isLoading()

    }).then((result) => {

        if (result.isConfirmed) {

            $.ajax({
                url: '/Account/DeleteAccount',
                type: 'DELETE',
                data: { id: id },

                success: function (response) {

                    Swal.fire({
                        title: "Eliminado Correctamente",
                        text: "Se eliminó el usuario y los archivos asociados",
                        icon: "success"
                    });
                    table.row(removingRow).remove().draw();
                },
                error: function (xhr, ajaxOptions, thrownError) {

                    Swal.fire({
                        title: "¡Error!",
                        text: "Error al eliminar la entrada",
                        icon: "error"
                    });
                }
            });
        }
    });
}


//Manipular los archivos PDF en la lista

function openPDF(filename, pdfDataUri) {

    var newWindow = window.open();

    // Escribir un iframe con el PDF como fuente
    newWindow.document.write('<iframe width="100%" height="100%" frameborder="0" src="' + pdfDataUri + '"></iframe>');

    newWindow.document.title = filename;
}

function downloadPDF(filename, pdfDataUri) {
    var link = document.createElement('a');
    link.href = pdfDataUri;
    link.download = filename;
    link.click();
}

