using System;
using Newtonsoft.Json;

namespace ConsultaTipoCambioWeb.Models
{
    public class EnTipoCambio
    {
        public class EnTipoCambioSolicitud
        {
            [JsonProperty("anio")]
            public string Anio { get; set; }
            [JsonProperty("mes")]
            public string Mes { get; set; }
            [JsonProperty("token")]
            public string Token { get; set; }
        }
        public class EnTipoCambioRespuesta
        {
            public string Fecha { get; set; }
            public string Compra { get; set; }
            public string Venta { get; set; }
        }
        public class EnTipoCambioDia
        {
            [JsonProperty("fecPublica")]
            public string FechaPublica { get; set; }

            [JsonProperty("valTipo")]
            public string ValorTipo { get; set; }

            [JsonProperty("codTipo")]
            public string CodigoTipo { get; set; }
        }
    }
}