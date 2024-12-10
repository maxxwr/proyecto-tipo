using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Windows.Forms;
using Global.CodigoUsuario;

namespace ConsultaTipoCambio
{
    public partial class FrmDemo2ServicioWebAPI : Form
    {
        public FrmDemo2ServicioWebAPI()
        {
            InitializeComponent();
        }

        public class EnTipoCambio
        {
            public string Fecha { get; set; }
            public string Compra { get; set; }
            public string Venta { get; set; }
        }

        public class EnTipoCambioDia
        {
            [JsonProperty("compra")]
            public string Compra { get; set; }

            [JsonProperty("venta")]
            public string Venta { get; set; }
        }

        private void FrmDemo2ServicioWebAPI_Load(object sender, EventArgs e)
        {
            DateTime fechaHoraActual = DateTime.Today;
            
            txtMes.Text = fechaHoraActual.Month.ToString();
            txtAnio.Text = fechaHoraActual.Year.ToString();

            DataTable tabla = new DataTable("TipoCambio");
            tabla.Columns.Add("Fecha", typeof(string));
            tabla.Columns.Add("Compra", typeof(string));
            tabla.Columns.Add("Venta", typeof(string));
            dgvTipoCambio.DataSource = tabla;

        }

        private string ObtenerUrlServicio(string sDia, string sMes, string sAnio, bool anioMaximoEsActual = true)
        {
            int tipoRespuesta = 2;
            string mensajeRespuesta = "";

            try
            {
                bool esDiaValido = false;
                bool esMesValido = false;
                bool esAnioValido = false;

                CuTexto oCuTexto = new CuTexto();
                esDiaValido = oCuTexto.ValidarSoloNumero(sDia);
                if (esDiaValido || sDia == "")
                {
                    if (sDia != "")
                    {
                        int iDia = Convert.ToInt16(sDia);
                        esDiaValido = iDia > 0 && iDia < 32;
                    }
                    if (esDiaValido || sDia == "")
                    {
                        esMesValido = oCuTexto.ValidarSoloNumero(sMes);
                        if (esMesValido || sMes == "")
                        {
                            if (sMes != "")
                            {
                                int iMes = Convert.ToInt16(sMes);
                                esMesValido = iMes > 0 && iMes < 13;
                            }
                            if (esMesValido || sMes == "")
                            {
                                esAnioValido = oCuTexto.ValidarSoloNumero(sAnio);
                                if (esAnioValido)
                                {
                                    int iAnio = Convert.ToInt32(sAnio);
                                    int iAnioMaximo = anioMaximoEsActual ? DateTime.Today.Year : DateTime.MaxValue.Year;
                                    esAnioValido = iAnio > 1901 && iAnio <= iAnioMaximo;
                                    if (esAnioValido)
                                        tipoRespuesta = 1;
                                    else
                                        mensajeRespuesta = string.Format("El año {0} debe estar entre 1901 y {1}.", sAnio, iAnioMaximo);
                                }
                                else
                                    mensajeRespuesta = string.Format("Los valores del año {0} no es válido.", sAnio);
                            }
                            else
                                mensajeRespuesta = string.Format("El mes {0} debe estar entre 1 y 12.", sMes);
                        }
                        else
                            mensajeRespuesta = string.Format("Los valores del mes {0} no es válido.", sMes);
                    }
                    else
                        mensajeRespuesta = string.Format("El día {0} debe estar entre 1 y 31.", sDia);
                }
                else
                    mensajeRespuesta = string.Format("Los valores del día {0} no es válido.", sDia);

                if (tipoRespuesta == 1)
                {
                    tipoRespuesta = 2;

                    string urlBase = "https://api.sunat.online/cambio/";
                    string urlCompleta;
                    if (sMes == "")
                        urlCompleta = string.Format("{0}{1}", urlBase, sAnio);
                    else
                    {
                        urlCompleta = sDia == ""
                            ? string.Format("{0}{1}-{2}", urlBase, sAnio, sMes.PadLeft(2, '0'))
                            : string.Format("{0}{1}-{2}-{3}", urlBase, sAnio, sMes.PadLeft(2, '0'), sDia.PadLeft(2, '0'));
                    }

                    tipoRespuesta = 1;
                    mensajeRespuesta = urlCompleta;
                }

            }
            catch (Exception ex)
            {
                tipoRespuesta = 3;
                mensajeRespuesta = ex.Message;
            }

            return string.Format("{0}~{1}", tipoRespuesta, mensajeRespuesta);
        }
        
        private void ValidarSoloNumero_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Si la tecla pulsada es un número entonces se digita.
            if (char.IsDigit(e.KeyChar))
                e.Handled = false;
            else
            {
                if (char.IsControl(e.KeyChar)) //permitir teclas de control como retroceso, suor
                    e.Handled = false;
                else
                    //el resto de teclas pulsadas no serán digitados
                    e.Handled = true;
            }
        }
        
        private async void btnConsultarTipoCambioServicioWebAPI_Click(object sender, EventArgs e)
        {
            int tipoRespuesta = 2;
            string mensajeRespuesta = "";

            List<EnTipoCambio> lEnTipoCambio = new List<EnTipoCambio>();
            EnTipoCambio oEnTipoCambio;

            lblMensaje.Text = "";
            btnConsultarTipoCambioServicioWebAPI.Enabled = false;

            Stopwatch oCronometro = new Stopwatch();
            oCronometro.Start();

            string sDia = txtDia.Text.Trim();
            string sMes = txtMes.Text.Trim();
            string sAnio = txtAnio.Text.Trim();

            try
            {
                string resultadoUrlServicio = ObtenerUrlServicio(sDia, sMes, sAnio);
                string[] arrResultadoUrlServicio = resultadoUrlServicio.Split('~');
                if (arrResultadoUrlServicio[0] == "1")
                {
                    using (HttpClient cliente = new HttpClient())
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 |
                                                       SecurityProtocolType.Tls12;
                        using (HttpResponseMessage resultadoConsulta = await cliente.GetAsync(new Uri(arrResultadoUrlServicio[1])))
                        {
                            if (resultadoConsulta.IsSuccessStatusCode)
                            {
                                string contenidoResultado = await resultadoConsulta.Content.ReadAsStringAsync();
                                if (contenidoResultado.Trim() == "")
                                    mensajeRespuesta = "Se realizó correctamente la consulta a la URL de SUNAT pero no devolvió el valor en el contenido.";
                                else
                                {
                                    if (contenidoResultado.IndexOf("error") > -1)
                                    {
                                        string urlServicio = arrResultadoUrlServicio[1];
                                        int ultimoIndice = urlServicio.LastIndexOf('/');
                                        mensajeRespuesta = ultimoIndice > -1
                                            ? string.Format("No existe la información solicitada {0}", urlServicio.Substring(ultimoIndice + 1, urlServicio.Length - ultimoIndice - 1))
                                            : string.Format("No existe la información solicitada con la URL {0}", urlServicio);
                                    }
                                    else
                                    {
                                        Dictionary<string, EnTipoCambioDia> lObjetoResultado = JsonConvert.DeserializeObject<Dictionary<string, EnTipoCambioDia>>(contenidoResultado);
                                        EnTipoCambioDia oEnTipoCambioDia;
                                        foreach (var objetoResultado in lObjetoResultado)
                                        {
                                            oEnTipoCambio = new EnTipoCambio();
                                            oEnTipoCambioDia = objetoResultado.Value;
                                            oEnTipoCambio.Fecha = objetoResultado.Key;
                                            oEnTipoCambio.Compra = oEnTipoCambioDia.Compra;
                                            oEnTipoCambio.Venta = oEnTipoCambioDia.Venta;
                                            lEnTipoCambio.Add(oEnTipoCambio);
                                        }

                                        tipoRespuesta = 1;
                                    }
                                    
                                }
                            }
                            else
                            {
                                mensajeRespuesta = await resultadoConsulta.Content.ReadAsStringAsync();
                                mensajeRespuesta = string.Format("Ocurrió un inconveniente al consultar el tipo de cambio desde la URL de SUNAT.\r\nDetalle: {0}", mensajeRespuesta);
                            }
                        }
                    }
                }
                else
                {
                    tipoRespuesta = Convert.ToInt16(arrResultadoUrlServicio[0]);
                    mensajeRespuesta = arrResultadoUrlServicio[1];
                }
            }
            catch (Exception ex)
            {
                tipoRespuesta = 3;
                mensajeRespuesta = ex.Message;
                if (ex.InnerException != null)
                    mensajeRespuesta = string.Format("{0}\r\n\r\n{1}", mensajeRespuesta, ex.InnerException.Message);
            }

            #region Establecer los valores del objeto de la clase EnTipoCambio a los controles del formulario 

            DataTable tabla = ((DataTable)dgvTipoCambio.DataSource).Clone();
            
            int nTipoCambio = lEnTipoCambio.Count;
            if(nTipoCambio > 0)
            {
                DataRow filaTabla;
                for (int i = 0; i < nTipoCambio; i++)
                {
                    filaTabla = tabla.NewRow();

                    oEnTipoCambio = lEnTipoCambio[i];
                    filaTabla[0] = oEnTipoCambio.Fecha;
                    filaTabla[1] = oEnTipoCambio.Compra;
                    filaTabla[2] = oEnTipoCambio.Venta;

                    tabla.Rows.Add(filaTabla);
                }
            }
            
            dgvTipoCambio.DataSource = tabla;

            #endregion


            oCronometro.Stop();

            if (tipoRespuesta > 1)
                MessageBox.Show(mensajeRespuesta, "Consultar Tipo de Cambio"
                    , MessageBoxButtons.OK
                    , tipoRespuesta == 2 ? MessageBoxIcon.Warning : MessageBoxIcon.Error);

            lblMensaje.Text = string.Format("Se encontró {0} resultado{1} ({2} segundos)", nTipoCambio, nTipoCambio == 1 ? "" : "s", Math.Round(oCronometro.Elapsed.TotalSeconds, 2));

            btnConsultarTipoCambioServicioWebAPI.Enabled = true;
        }

       
    }
}
