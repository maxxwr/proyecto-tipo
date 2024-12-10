function obtenerInformacionServidor(metodoHttp, url, funcionRetorno, tipoRespuesta) {
    var xhr = new XMLHttpRequest();
    xhr.open(metodoHttp, url);
    if (tipoRespuesta != null)
        xhr.responseType = tipoRespuesta;
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4) {
            if (tipoRespuesta == null)
                funcionRetorno(xhr.responseText, xhr.status);
            else funcionRetorno(xhr.response, xhr.status);
        }
    }
    xhr.send();
}

function get(url, funcionRetorno) {
    obtenerInformacionServidor("get", url, funcionRetorno);
}

function syntaxHighlight(json) {
    json = json.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
    return json.replace(/("(\\u[a-zA-Z0-9]{4}|\\[^u]|[^\\"])*"(\s*:)?|\b(true|false|null)\b|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?)/g, function (match) {
        var cls = 'number';
        if (/^"/.test(match)) {
            if (/:$/.test(match)) {
                cls = 'key';
            } else {
                cls = 'string';
            }
        } else if (/true|false/.test(match)) {
            cls = 'boolean';
        } else if (/null/.test(match)) {
            cls = 'null';
        }
        return '<span class="' + cls + '">' + match + '</span>';
    });
}


window.onload = function() {
    document.getElementById("txtAnio").value = new Date().getFullYear();
}

function mostrarMensaje(descripcion) {
    document.getElementById("spnMensaje").innerHTML = descripcion;
}

document.getElementById("btnConsultarTipoCambio").onclick = function () {
    mostrarMensaje("Consultando el tipo de cambio ...");
    document.getElementById("spnResultado").innerHTML = "";

    var numeroAnio = document.getElementById("txtAnio").value;
    var cboMes = document.getElementById("cboMes");
    var numeroMes = cboMes.selectedIndex < 0 ? "" : cboMes.options[cboMes.selectedIndex].value;

    mostrarMensaje("Consultando el tipo de cambio del año " + numeroAnio + " y mes " + numeroMes + " ...");

    var url = "TipoCambio/ConsultarTipoCambio/?anio=";
    url += numeroAnio;
    url += "&mes=";
    url += numeroMes;
    get(url, procesarConsultaTipoCambio);
}

function procesarConsultaTipoCambio(respuesta, estadoRespuesta) {
    if (estadoRespuesta === 200) {
        if (respuesta) {
            var arrRespuesta = respuesta.split('~');
            if (arrRespuesta[0] == "1") {
                var lTipoCambioJson = JSON.parse(arrRespuesta[1]);
                var tipoCambioCadena = JSON.stringify(lTipoCambioJson, null, '\t');
                document.getElementById("spnResultado").innerHTML = "<pre>" + syntaxHighlight(tipoCambioCadena) + "</pre>";
                mostrarMensaje("Se ha realizado exitosamente la consulta del tipo de cambio");
            } else
                mostrarMensaje(respuesta);
        } else
            mostrarMensaje(
                "El servidor respondió correctamente al consultar el tipo de cambio, pero no devolvió la información solicitada.");
    } else
        mostrarMensaje("Ocurrió el error " + estadoRespuesta + " al consultar el tipo de cambio.");

}

