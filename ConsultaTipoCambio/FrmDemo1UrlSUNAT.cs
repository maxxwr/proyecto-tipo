using System;
using System.Diagnostics;
using System.Net.Http;
using System.Windows.Forms;

namespace ConsultaTipoCambio
{
    public partial class FrmDemo1UrlSUNAT : Form
    {
        public FrmDemo1UrlSUNAT()
        {
            InitializeComponent();
        }

        public class EnTipoCambio
        {
            public string Fecha { get; set; }
            public string Compra { get; set; }
            public string Venta { get; set; }
        }

        private void FrmDemo1UrlSUNAT_Load(object sender, EventArgs e)
        {

        }

        private async void btnConsultarTipoCambioUrlSUNAT_Click(object sender, EventArgs e)
        {
            int tipoRespuesta = 2;
            string mensajeRespuesta = "";
            EnTipoCambio oEnTipoCambio = null;

            lblMensaje.Text = "";
            btnConsultarTipoCambioUrlSUNAT.Enabled = false;

            Stopwatch oCronometro = new Stopwatch();
            oCronometro.Start();
            
            string url = "https://www.sunat.gob.pe/a/txt/tipoCambio.txt";
            using (HttpClient cliente = new HttpClient())
            {
                using (HttpResponseMessage resultadoConsulta = await cliente.GetAsync(new Uri(url)))
                {
                    if (resultadoConsulta.IsSuccessStatusCode)
                    {
                        string contenidoResultado = await resultadoConsulta.Content.ReadAsStringAsync();
                        if (contenidoResultado.Trim() == "")
                            mensajeRespuesta = "Se realizó correctamente la consulta a la URL de SUNAT pero no devolvió el valor en el contenido.";
                        else
                        {
                            string[] arrContenidoResultado = contenidoResultado.Split('|');

                            oEnTipoCambio = new EnTipoCambio();
                            oEnTipoCambio.Fecha = arrContenidoResultado[0];
                            oEnTipoCambio.Compra = arrContenidoResultado[1];
                            oEnTipoCambio.Venta = arrContenidoResultado[2];
                            tipoRespuesta = 1;
                        }
                    }
                    else
                    {
                        mensajeRespuesta = await resultadoConsulta.Content.ReadAsStringAsync();
                        mensajeRespuesta = string.Format("Ocurrió un inconveniente al consultar el tipo de cambio desde la URL de SUNAT.\r\nDetalle: {0}", mensajeRespuesta);
                    }
                }
            }

            #region Establecer los valores del objeto de la clase EnTipoCambio a los controles del formulario 

            if (oEnTipoCambio == null)
            {
                txtFechaTipoCambio.Text = "";
                txtCompraTipoCambio.Text = "";
                txtVentaTipoCambio.Text = "";
            }
            else
            {
                txtFechaTipoCambio.Text = oEnTipoCambio.Fecha;
                txtCompraTipoCambio.Text = oEnTipoCambio.Compra;
                txtVentaTipoCambio.Text = oEnTipoCambio.Venta;
            }
            #endregion
            
            oCronometro.Stop();

            if (tipoRespuesta > 1)
                MessageBox.Show(mensajeRespuesta, "Consultar Tipo de Cambio"
                    , MessageBoxButtons.OK
                    , tipoRespuesta == 2 ? MessageBoxIcon.Warning : MessageBoxIcon.Error);

            lblMensaje.Text = string.Format("Procesado en {0} seg.", oCronometro.Elapsed.TotalSeconds);

            btnConsultarTipoCambioUrlSUNAT.Enabled = true;

        }

        private void txtFechaTipoCambio_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
