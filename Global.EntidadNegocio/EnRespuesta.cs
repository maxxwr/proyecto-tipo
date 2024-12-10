using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global.EntidadNegocio
{
    public class EnRespuesta
    {
        public enum TipoRespuesta
        {
            Correcto = 1,
            Inconveniente = 2,
            Error = 3
        }
        public class Respuesta
        {
            public TipoRespuesta TipoRespuesta { get; set; }
            public string MensajeRespuesta { get; set; }
        }
    }
}
