using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Global.EntidadNegocio;

namespace Global.CodigoUsuario
{
    public class CuTexto
    {
        public bool ValidarSoloNumero(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return false;
            bool esValido = false;
            int nCaracteres = valor.Length;
            int caracter = valor[0];
            // números => 0 = 48 y 9 = 57
            if (caracter > 47 && caracter < 58)
            {
                esValido = true;
                int i;
                for (i = 1; i < nCaracteres; i++)
                {
                    caracter = valor[i];
                    if (caracter < 48 || caracter > 59)
                    {
                        esValido = false;
                        break;
                    }
                }
            }
            return esValido;
        }

        public EnRespuesta.Respuesta ValidarAnio(string sAnio, bool anioMaximoEsActual = true)
        {
            EnRespuesta.Respuesta oRespuesta = new EnRespuesta.Respuesta
            {
                TipoRespuesta = EnRespuesta.TipoRespuesta.Inconveniente
            };
            if (sAnio == null)
                oRespuesta.MensajeRespuesta = string.Format("El parámetro sAnio es nulo");
            else if (sAnio.Trim() == "")
                oRespuesta.MensajeRespuesta = string.Format("El parámetro sAnio no debe estar vacío");
            else
            {
                bool esAnioValido = ValidarSoloNumero(sAnio);
                if (esAnioValido)
                {
                    int iAnio = Convert.ToInt32(sAnio);
                    int iAnioMaximo = anioMaximoEsActual ? DateTime.Today.Year : DateTime.MaxValue.Year;
                    esAnioValido = iAnio > 1901 && iAnio <= iAnioMaximo;
                    if (esAnioValido)
                        oRespuesta.TipoRespuesta = EnRespuesta.TipoRespuesta.Correcto;
                    else
                        oRespuesta.MensajeRespuesta = string.Format("El año {0} debe estar entre 1901 y {1}.", sAnio, iAnioMaximo);
                }
                else
                    oRespuesta.MensajeRespuesta = string.Format("Los valores del año {0} no es válido.", sAnio);
            }
            return oRespuesta;
        }

        public EnRespuesta.Respuesta ValidarMes(string sMes)
        {
            EnRespuesta.Respuesta oRespuesta = new EnRespuesta.Respuesta
            {
                TipoRespuesta = EnRespuesta.TipoRespuesta.Inconveniente
            };
            if (sMes == null)
                oRespuesta.MensajeRespuesta = string.Format("El parámetro sMes es nulo");
            else if (sMes.Trim() == "")
                oRespuesta.MensajeRespuesta = string.Format("El parámetro sMes no debe estar vacío");
            else
            {
                bool esMesValido = ValidarSoloNumero(sMes);
                if (esMesValido)
                {
                    int iMes = Convert.ToInt32(sMes);
                    esMesValido = iMes > 0 && iMes < 13;
                    if (esMesValido)
                        oRespuesta.TipoRespuesta = EnRespuesta.TipoRespuesta.Correcto;
                    else
                        oRespuesta.MensajeRespuesta = "El mes {0} debe estar entre 1 y 12.";
                }
                else
                    oRespuesta.MensajeRespuesta = string.Format("Los valores del mes {0} no es válido.", sMes);
            }
            return oRespuesta;
        }

    }
}
