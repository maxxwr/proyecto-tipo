using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Global.CodigoUsuario;
using Global.EntidadNegocio;

namespace ConsultaTipoCambio
{
    public partial class FrmDemo3PaginaSUNAT : Form
    {
        public FrmDemo3PaginaSUNAT()
        {
            InitializeComponent();
        }

        private ChromeDriverService _servicioChromeDriver;
        private IWebDriver _driver;

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
        
        private void FrmDemo3PaginaSUNAT_Shown(object sender, EventArgs e)
        {
            this.Hide();
        }
        private async void FrmDemo3PaginaSUNAT_Load(object sender, EventArgs e)
        {
            try
            {
                CuProceso oCuProceso = new CuProceso();
                oCuProceso.FinalizarArbolProcesos("chromedriver.exe");
            }
            catch { }

            _servicioChromeDriver = ChromeDriverService.CreateDefaultService();
            _servicioChromeDriver.HideCommandPromptWindow = true;

            var opcionChrome = new ChromeOptions();
            opcionChrome.AddArgument("headless");
            opcionChrome.AddArgument(
                "user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.107 Safari/537.36");
            _driver = new ChromeDriver(_servicioChromeDriver, opcionChrome);
            await Task.Run(async () =>
            {
                _driver.Navigate().GoToUrl("https://www.google.com/");
                await Task.Delay(500);
                _driver.Navigate().GoToUrl("https://e-consulta.sunat.gob.pe/cl-at-ittipcam/tcS01Alias");
                await Task.Delay(100);
            });


            DateTime fechaHoraActual = DateTime.Today;
            
            cboMes.Items.AddRange(new object[] {
                "Enero",
                "Febrero",
                "Marzo",
                "Abril",
                "Mayo",
                "Junio",
                "Julio",
                "Agosto",
                "Setiembre",
                "Octubre",
                "Noviembre",
                "Diciembre"
            });
            cboMes.SelectedIndex = fechaHoraActual.Month - 1;

            txtAnio.Text = fechaHoraActual.Year.ToString();

            DataTable tabla = new DataTable("TipoCambio");
            tabla.Columns.Add("Fecha", typeof(string));
            tabla.Columns.Add("Compra", typeof(string));
            tabla.Columns.Add("Venta", typeof(string));
            dgvTipoCambio.DataSource = tabla;

            cboMes.Enabled = true;
            txtAnio.Enabled = true;
            btnConsultarTipoCambioUrlSUNAT.Enabled = true;

            this.Show();
            this.WindowState = FormWindowState.Normal;
            txtAnio.Focus();
        }
        private void ValidarSoloNumero_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Si la tecla pulsada es un número entonces se digita.
            if (char.IsDigit(e.KeyChar))
                e.Handled = false;
            else
            {
                if (char.IsControl(e.KeyChar)) //permitir teclas de control como retroceso, supr
                    e.Handled = false;
                else
                    //el resto de teclas pulsadas no serán digitados
                    e.Handled = true;
            }
        }
        private async void btnConsultarTipoCambioUrlSUNAT_Click(object sender, EventArgs e)
        {
            EnRespuesta.Respuesta oRespuesta = new EnRespuesta.Respuesta
            {
                TipoRespuesta = EnRespuesta.TipoRespuesta.Inconveniente
            };

            List<EnTipoCambioRespuesta> lEnTipoCambio = new List<EnTipoCambioRespuesta>();
            EnTipoCambioRespuesta oEnTipoCambio;

            lblMensaje.Text = "Consultando ...";
            lblMensaje.Refresh();
            btnConsultarTipoCambioUrlSUNAT.Enabled = false;

            Stopwatch oCronometro = new Stopwatch();
            oCronometro.Start();

            try
            {
                int iMes = cboMes.SelectedIndex;
                string sAnio = txtAnio.Text.Trim();
                CuTexto oCuTexto = new CuTexto();
                EnRespuesta.Respuesta resultadoValidarAnio = oCuTexto.ValidarAnio(sAnio);
                if (resultadoValidarAnio.TipoRespuesta == EnRespuesta.TipoRespuesta.Correcto)
                {
                    string token = "";
                    
                    IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;

                    var obj = js.ExecuteScript(
                        "grecaptcha.execute(site_key_sunat, {action: 'token'}).then(function(token) { $('#tokenDownload').val(token); });");
                    await Task.Delay(500);

                    token = js.ExecuteScript(
                        "return $('#tokenDownload').val();").ToString();

                    if (token == "")
                        oRespuesta.MensajeRespuesta = string.Format("No se pudo obtener el token de la información solicitada en el año {0} y mes {1}.", sAnio, (iMes + 1).ToString().PadLeft(2, '0'));
                    else
                    {
                        string urlListadoJson = "https://e-consulta.sunat.gob.pe/cl-at-ittipcam/tcS01Alias/listarTipoCambio";

                        using (HttpClient cliente = new HttpClient())
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 |
                                                           SecurityProtocolType.Tls12;
                            EnTipoCambioSolicitud oEnTipoCambioSolicitud = new EnTipoCambioSolicitud();

                            oEnTipoCambioSolicitud.Anio = sAnio;
                            oEnTipoCambioSolicitud.Mes = iMes.ToString();
                            oEnTipoCambioSolicitud.Token = token;
                            using (HttpResponseMessage resultadoConsulta = await cliente.PostAsJsonAsync(new Uri(urlListadoJson), oEnTipoCambioSolicitud))
                            {
                                if (resultadoConsulta.IsSuccessStatusCode)
                                {
                                    string contenidoResultado = await resultadoConsulta.Content.ReadAsStringAsync();
                                    if (contenidoResultado.Trim() == "")
                                        oRespuesta.MensajeRespuesta = "Se realizó correctamente la consulta a la URL de SUNAT pero no devolvió el valor en el contenido.";
                                    else
                                    {
                                        if (contenidoResultado == "[]")
                                            oRespuesta.MensajeRespuesta = string.Format("No existe la información solicitada en el año {0} y mes {1}", sAnio, (iMes + 1).ToString().PadLeft(2, '0'));
                                        else
                                        {
                                            List<EnTipoCambioDia> lObjetoResultado = JsonConvert.DeserializeObject<List<EnTipoCambioDia>>(contenidoResultado);
                                            string fechaTipoCambio;
                                            string fechaTipoCambioAnterior = "";
                                            int cTipoCambio = -1;
                                            foreach (var objetoResultado in lObjetoResultado)
                                            {
                                                fechaTipoCambio = objetoResultado.FechaPublica;
                                                oEnTipoCambio = fechaTipoCambioAnterior == fechaTipoCambio ? lEnTipoCambio[cTipoCambio] : new EnTipoCambioRespuesta();

                                                if (objetoResultado.CodigoTipo.ToLower() == "c")
                                                    oEnTipoCambio.Compra = objetoResultado.ValorTipo;
                                                else
                                                    oEnTipoCambio.Venta = objetoResultado.ValorTipo;

                                                if (fechaTipoCambioAnterior == fechaTipoCambio) continue;
                                                cTipoCambio++;
                                                oEnTipoCambio.Fecha = fechaTipoCambio;
                                                lEnTipoCambio.Add(oEnTipoCambio);
                                                fechaTipoCambioAnterior = fechaTipoCambio;
                                            }

                                            oRespuesta.TipoRespuesta = EnRespuesta.TipoRespuesta.Correcto;
                                        }
                                    }
                                }
                                else
                                {
                                    oRespuesta.MensajeRespuesta = await resultadoConsulta.Content.ReadAsStringAsync();
                                    oRespuesta.MensajeRespuesta = string.Format("Ocurrió un inconveniente al consultar el tipo de cambio desde la URL de SUNAT.\r\nDetalle: {0}", oRespuesta.MensajeRespuesta);
                                }
                            }
                        }
                    }
                }
                else
                {
                    oRespuesta.TipoRespuesta = resultadoValidarAnio.TipoRespuesta;
                    oRespuesta.MensajeRespuesta = resultadoValidarAnio.MensajeRespuesta;
                }
            }
            catch (Exception ex)
            {
                oRespuesta.TipoRespuesta = EnRespuesta.TipoRespuesta.Error;
                oRespuesta.MensajeRespuesta = ex.Message;
                if (ex.InnerException != null)
                    oRespuesta.MensajeRespuesta = string.Format("{0}\r\n\r\n{1}", oRespuesta.MensajeRespuesta, ex.InnerException.Message);
            }

            #region Establecer los valores del objeto de la clase EnTipoCambio a los controles del formulario 

            DataTable tabla = ((DataTable)dgvTipoCambio.DataSource).Clone();

            int nTipoCambio = lEnTipoCambio.Count;
            if (nTipoCambio > 0)
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

            if (oRespuesta.TipoRespuesta != EnRespuesta.TipoRespuesta.Correcto)
                MessageBox.Show(oRespuesta.MensajeRespuesta, "Consultar Tipo de Cambio"
                    , MessageBoxButtons.OK
                    , oRespuesta.TipoRespuesta == EnRespuesta.TipoRespuesta.Inconveniente ? MessageBoxIcon.Warning : MessageBoxIcon.Error);

            lblMensaje.Text = string.Format("Se encontró {0} resultado{1} ({2} segundos)", nTipoCambio, nTipoCambio == 1 ? "" : "s", Math.Round(oCronometro.Elapsed.TotalSeconds, 2));

            btnConsultarTipoCambioUrlSUNAT.Enabled = true;
        }
        
        private void FrmDemo3PaginaSUNAT_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();

            try
            {
                if (_driver != null)
                {
                    _driver.Close();
                    _driver.Quit();
                    _driver.Dispose();
                }
                if(_servicioChromeDriver != null)
                    _servicioChromeDriver.Dispose();
            }
            catch {}
            
            
        }

        
    }
}
