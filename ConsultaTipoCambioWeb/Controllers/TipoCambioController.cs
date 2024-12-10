using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using ConsultaTipoCambioWeb.Models;
using Global.CodigoUsuario;
using Global.EntidadNegocio;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ConsultaTipoCambioWeb.Controllers
{
    public class TipoCambioController : Controller
    {
        private static ChromeDriverService _servicioChromeDriver;
        private static IWebDriver _driver;
        
        public ActionResult Index()
        {
            try
            {
                CuProceso oCuProceso = new CuProceso();
                oCuProceso.FinalizarArbolProcesos("chromedriver.exe");
            }
            catch { }

            _servicioChromeDriver =
                _servicioChromeDriver = ChromeDriverService.CreateDefaultService();
            _servicioChromeDriver.HideCommandPromptWindow = true;

            var opcionChrome = new ChromeOptions();
            opcionChrome.AddArgument("headless");
            opcionChrome.AddArgument(
                "user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.107 Safari/537.36");
            _driver = new ChromeDriver(_servicioChromeDriver, opcionChrome);

            _driver.Navigate().GoToUrl("https://www.google.com/");
            Task.Delay(500);
            _driver.Navigate().GoToUrl("https://e-consulta.sunat.gob.pe/cl-at-ittipcam/tcS01Alias");
            Task.Delay(100);

            return View();
        }

        public async Task<string> ConsultarTipoCambio(string anio, string mes)
        {
            EnRespuesta.Respuesta oRespuesta = new EnRespuesta.Respuesta
            {
                TipoRespuesta = EnRespuesta.TipoRespuesta.Inconveniente
            };
            
            List<EnTipoCambio.EnTipoCambioRespuesta> lEnTipoCambioRespuesta = new List<EnTipoCambio.EnTipoCambioRespuesta>();
            EnTipoCambio.EnTipoCambioRespuesta oEnTipoCambioRespuesta;
            
            try
            {
                CuTexto oCuTexto = new CuTexto();
                EnRespuesta.Respuesta resultadoValidarAnio = oCuTexto.ValidarAnio(anio);
                if (resultadoValidarAnio.TipoRespuesta == EnRespuesta.TipoRespuesta.Correcto)
                {
                    EnRespuesta.Respuesta resultadoValidarMes = oCuTexto.ValidarMes(mes);
                    if (resultadoValidarMes.TipoRespuesta == EnRespuesta.TipoRespuesta.Correcto)
                    {
                        string token = "";

                        IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;

                        var obj = js.ExecuteScript(
                            "grecaptcha.execute(site_key_sunat, {action: 'token'}).then(function(token) { $('#tokenDownload').val(token); });");
                        await Task.Delay(500);

                        token = js.ExecuteScript(
                            "return $('#tokenDownload').val();").ToString();

                        if (token == "")
                            oRespuesta.MensajeRespuesta = string.Format("No se pudo obtener el token de la información solicitada en el año {0} y mes {1}.", anio, mes.PadLeft(2, '0'));
                        else
                        {
                            string urlListadoJson = "https://e-consulta.sunat.gob.pe/cl-at-ittipcam/tcS01Alias/listarTipoCambio";

                            using (HttpClient cliente = new HttpClient())
                            {
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 |
                                                               SecurityProtocolType.Tls12;
                                EnTipoCambio.EnTipoCambioSolicitud oEnTipoCambioSolicitud = new EnTipoCambio.EnTipoCambioSolicitud();

                                oEnTipoCambioSolicitud.Anio = anio;
                                oEnTipoCambioSolicitud.Mes = (Convert.ToInt16(mes) - 1).ToString();
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
                                                oRespuesta.MensajeRespuesta = string.Format("No existe la información solicitada en el año {0} y mes {1}", anio, mes.PadLeft(2, '0'));
                                            else
                                            {
                                                List<EnTipoCambio.EnTipoCambioDia> lObjetoResultado = JsonConvert.DeserializeObject<List<EnTipoCambio.EnTipoCambioDia>>(contenidoResultado);
                                                string fechaTipoCambio;
                                                string fechaTipoCambioAnterior = "";
                                                int cTipoCambio = -1;
                                                foreach (var objetoResultado in lObjetoResultado)
                                                {
                                                    fechaTipoCambio = objetoResultado.FechaPublica;
                                                    oEnTipoCambioRespuesta = fechaTipoCambioAnterior == fechaTipoCambio ? lEnTipoCambioRespuesta[cTipoCambio] : new EnTipoCambio.EnTipoCambioRespuesta();

                                                    if (objetoResultado.CodigoTipo.ToLower() == "c")
                                                        oEnTipoCambioRespuesta.Compra = objetoResultado.ValorTipo;
                                                    else
                                                        oEnTipoCambioRespuesta.Venta = objetoResultado.ValorTipo;

                                                    if (fechaTipoCambioAnterior == fechaTipoCambio) continue;
                                                    cTipoCambio++;
                                                    oEnTipoCambioRespuesta.Fecha = fechaTipoCambio;
                                                    lEnTipoCambioRespuesta.Add(oEnTipoCambioRespuesta);
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

            return oRespuesta.TipoRespuesta == EnRespuesta.TipoRespuesta.Correcto
                ? string.Format("1~{0}", JsonConvert.SerializeObject(lEnTipoCambioRespuesta))
                : string.Format("{0}~{1}", oRespuesta.TipoRespuesta == EnRespuesta.TipoRespuesta.Inconveniente ? 2 : 3,
                    oRespuesta.MensajeRespuesta);
        }
    }
}