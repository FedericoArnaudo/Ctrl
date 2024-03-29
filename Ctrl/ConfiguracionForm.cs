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
    public partial class ConfiguracionForm : Form
    {
        private string[] controladores = { "CEM", "WOLF", "VOX", "MIDEX", "PAM" };
        public ConfiguracionForm()
        {
            InitializeComponent();
        }
        private void ConfiguracionForm_Load(object sender, EventArgs e)
        {
            //Agrega al Form todas las propiedades visibles para ser seleccionadas
            foreach (string controlador in controladores)
                boxTipo.Items.Add(controlador);

            boxNivelLog.Items.Add("Debug");
            boxNivelLog.Items.Add("Informativo");
            boxNivelLog.Items.Add("Solo errores");

            if (Configuracion.existeConfiguracion())
            {
                Configuracion.InfoConfig config = Configuracion.leerConfiguracion();

                txtProyNuevo.Text = config.rutaProyNuevo;
                boxNivelLog.SelectedIndex = (int)config.nivelLog;
                numSegFacturacion.Value = config.segundosFacturacion;
                boxTipo.SelectedIndex = (int)config.tipo;
                txtIp.Text = config.ip;
                if (config.protocoloSurtidores == 16)
                    rbProtocolo16.Checked = true;
                else
                    rbProtocolo32.Checked = true;
            }
        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            Configuracion.InfoConfig config = new Configuracion.InfoConfig();

            config.rutaProyNuevo = txtProyNuevo.Text;
            config.nivelLog = (Log.LogType)boxNivelLog.SelectedIndex;
            config.segundosFacturacion = (int)numSegFacturacion.Value;
            config.tipo = (Configuracion.TipoControlador)boxTipo.SelectedIndex;
            config.ip = txtIp.Text;
            if (rbProtocolo16.Checked)
                config.protocoloSurtidores = 16;
            else
                config.protocoloSurtidores = 32;

            if (config.rutaProyNuevo != "")
            {
                if (config.ip != "")
                {
                    if (Configuracion.guardarConfiguracion(config))
                    {
                        MessageBox.Show("Configuracón guardada correctamente",
                                "Exito!",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Compruebe los valores ingresados",
                                "Error al guardar la configuración",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Debe ingresar la IP del controlador para continuar.");
                }
            }
            else
            {
                MessageBox.Show("Debe ingresar la ruta del proyecto para continuar.");
            }

            
        }
        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnSearchFolder_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
                txtProyNuevo.Text = folderBrowserDialog1.SelectedPath;
        }


    }
}
