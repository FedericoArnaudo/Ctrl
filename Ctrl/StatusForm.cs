using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ctrl
{
    public partial class StatusForm : Form
    {
        private bool close = false;
        public StatusForm()
        {
            InitializeComponent();
        }
        private void StatusForm_Load(object sender, EventArgs e)
        {
            Log.Instance.writeLog("Programa iniciado", Log.LogType.t_info);

            lblStatus.Text = "Iniciando";

            // Verifica si ya existe un archivo de configuracíón.
            // Si no existe abre la configuración del transportador para crear uno nuevo
            if (!Configuracion.existeConfiguracion())
            {
                ConfiguracionForm tmp = new ConfiguracionForm();
                Log.Instance.writeLog("Configurando CDS", Log.LogType.t_info);
                tmp.Show();
                tmp.Focus();
                tmp.Shown += (s, args) => this.SendToBack();
                tmp.Closed += (s, args) => {
                    this.BringToFront();
                    init();
                };
            }
            else
            {
                init();
            }
        }
        private void init()
        {
            Log.Instance.writeLog("Configuración leída correctamente.", Log.LogType.t_info);
            if (!Controlador.init(Configuracion.leerConfiguracion()))
            {
                MessageBox.Show(
                    "No se pudo inicializar el controlador. Verificar la configuración e iniciarlo nuevamente",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Log.Instance.writeLog("No se pudo inicializar el controlador. Verificar la configuración", Log.LogType.t_error);

                // TODO: Borrar archivo config para que no abra de vuelta.

                //btnCerrar_Click(null, null); // Cerrar
            }

            lblStatus.Text = "Procesando";
        }
        private void StatusForm_Resize(object sender, EventArgs e)
        {
            //Falta implementar para que funcione en segundo plano.
        }
        private void StatusForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Falta implementar para que funcione en segundo plano.
            // Hide form
            this.Visible = false;
            // Make icon visible in taskbar
            if (close == false)
            {
                e.Cancel = true;
            }
        }
        private void btnConfig_Click(object sender, EventArgs e)
        {
            ConfiguracionForm tmp = new ConfiguracionForm();
            tmp.Show();
            tmp.Focus();
            tmp.Closed += (s, args) => this.BringToFront();
        }
    }
}
