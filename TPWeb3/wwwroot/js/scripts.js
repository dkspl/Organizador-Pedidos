﻿$(document).ready(function () {
    $('#incluir').change(function () {
        $('#formCambio').submit();
    });
    $('.js-example-basic-multiple').select2({
        theme: 'bootstrap4',
    });
    $('.js-example-basic-single').select2({
        theme: 'bootstrap4',
    });
    $('#lista').dataTable({
        "language": {
            "url": "https://cdn.datatables.net/plug-ins/1.10.25/i18n/Spanish.json"
        },
        "ordering": false,
        "destroy": true
    });
    $('#nuevoArticulo').click(function () {
        var $cantidadNueva = $('#cantArt').val();
        var flag = 0;
        if ($cantidadNueva >= 1) {
            $(".div-articulo").each(function () {
                if ($(this).attr('id') == $('#codArt option:selected').val()) {
                    var idArt = "#Articulo_" + $('#codArt option:selected').val();
                    var cantArt = "#Cantidad_" + $('#codArt option:selected').val();
                    $(idArt).val(Number($cantidadNueva) + Number($(idArt).val()));
                    $(cantArt).text($(idArt).val());
                    flag = 1;
                    return false;
                }
            });
            if (flag == 0) {
                $('#listaArticulos').append('<tr class="table-row div-articulo" id="' + $('#codArt option:selected').val() + '">\n<td>' + $('#codArt option:selected').text() + '</td>\n<td>' + $('#codArt option:selected').val() + '</td>\n<td id="Cantidad_' + $('#codArt option:selected').val() + '">' + $('#cantArt').val() + '</td>\n<td><button type="button" class="btn btn-outline-warning quitar">Quitar</button></td>\n<input type="hidden" id="Articulo_' + $('#codArt option:selected').val() + '" name="Articulos[' + $('#codArt option:selected').val() + ']" class="form-control articulo" value="' + $('#cantArt').val() + '"></tr>\n');
            }
            $('.btnAct').removeAttr('disabled');
        }
    });
    $("#guardar").click(function () {
        $("#retorno").val(0);
    });
    $("#guardarycrear").click(function () {
        $("#retorno").val(1);
    });
    $(document).on("click", ".quitar", function (event) {
        event.preventDefault();
        $(this).closest("tr").remove();
        
        var rowCount = $('#listaArticulos').children().length;
        if (rowCount < 1) {
            $('.btnAct').attr('disabled', true);
        } else {
            $('.btnAct').removeAttr('disabled');
        }
    });
    

});