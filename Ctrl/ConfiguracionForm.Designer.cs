
using System;

namespace Ctrl
{
    partial class ConfiguracionForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfiguracionForm));
            this.btnGuardar = new System.Windows.Forms.Button();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numSegFacturacion = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.boxNivelLog = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnSearchFolder = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtProyNuevo = new System.Windows.Forms.TextBox();
            this.tabs = new System.Windows.Forms.TabControl();
            this.tabControlador = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbProtocolo16 = new System.Windows.Forms.RadioButton();
            this.rbProtocolo32 = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.txtIp = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.boxTipo = new System.Windows.Forms.ComboBox();
            this.btnSalir = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.tabGeneral.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSegFacturacion)).BeginInit();
            this.tabs.SuspendLayout();
            this.tabControlador.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnGuardar
            // 
            this.btnGuardar.BackColor = System.Drawing.Color.DarkSalmon;
            this.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuardar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGuardar.Location = new System.Drawing.Point(13, 276);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(126, 37);
            this.btnGuardar.TabIndex = 1;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = false;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // tabGeneral
            // 
            this.tabGeneral.BackColor = System.Drawing.Color.DarkSalmon;
            this.tabGeneral.Controls.Add(this.groupBox2);
            this.tabGeneral.Controls.Add(this.boxNivelLog);
            this.tabGeneral.Controls.Add(this.label6);
            this.tabGeneral.Controls.Add(this.btnSearchFolder);
            this.tabGeneral.Controls.Add(this.label3);
            this.tabGeneral.Controls.Add(this.txtProyNuevo);
            this.tabGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabGeneral.Size = new System.Drawing.Size(274, 243);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "General";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.numSegFacturacion);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(6, 125);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(258, 67);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Recuperar despachos";
            // 
            // numSegFacturacion
            // 
            this.numSegFacturacion.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numSegFacturacion.Location = new System.Drawing.Point(77, 36);
            this.numSegFacturacion.Name = "numSegFacturacion";
            this.numSegFacturacion.Size = new System.Drawing.Size(50, 16);
            this.numSegFacturacion.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(130, 39);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "segundos.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(188, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "No recuperar despachos despues de: ";
            // 
            // boxNivelLog
            // 
            this.boxNivelLog.FormattingEnabled = true;
            this.boxNivelLog.Location = new System.Drawing.Point(8, 88);
            this.boxNivelLog.Name = "boxNivelLog";
            this.boxNivelLog.Size = new System.Drawing.Size(258, 21);
            this.boxNivelLog.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(8, 68);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 16);
            this.label6.TabIndex = 10;
            this.label6.Text = "Nivel de Logs";
            // 
            // btnSearchFolder
            // 
            this.btnSearchFolder.BackColor = System.Drawing.Color.LightSalmon;
            this.btnSearchFolder.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnSearchFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearchFolder.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSearchFolder.Location = new System.Drawing.Point(240, 35);
            this.btnSearchFolder.Margin = new System.Windows.Forms.Padding(1);
            this.btnSearchFolder.Name = "btnSearchFolder";
            this.btnSearchFolder.Size = new System.Drawing.Size(26, 21);
            this.btnSearchFolder.TabIndex = 9;
            this.btnSearchFolder.Text = "...";
            this.btnSearchFolder.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSearchFolder.UseVisualStyleBackColor = false;
            this.btnSearchFolder.Click += new System.EventHandler(this.btnSearchFolder_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(8, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(143, 16);
            this.label3.TabIndex = 8;
            this.label3.Text = "Ruta a PROY_NUEVO";
            // 
            // txtProyNuevo
            // 
            this.txtProyNuevo.Location = new System.Drawing.Point(8, 35);
            this.txtProyNuevo.Name = "txtProyNuevo";
            this.txtProyNuevo.Size = new System.Drawing.Size(228, 20);
            this.txtProyNuevo.TabIndex = 7;
            // 
            // tabs
            // 
            this.tabs.Controls.Add(this.tabGeneral);
            this.tabs.Controls.Add(this.tabControlador);
            this.tabs.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabs.Location = new System.Drawing.Point(0, 0);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(281, 269);
            this.tabs.TabIndex = 0;
            // 
            // tabControlador
            // 
            this.tabControlador.BackColor = System.Drawing.Color.DarkSalmon;
            this.tabControlador.Controls.Add(this.groupBox1);
            this.tabControlador.Controls.Add(this.label2);
            this.tabControlador.Controls.Add(this.txtIp);
            this.tabControlador.Controls.Add(this.label1);
            this.tabControlador.Controls.Add(this.boxTipo);
            this.tabControlador.Location = new System.Drawing.Point(4, 22);
            this.tabControlador.Name = "tabControlador";
            this.tabControlador.Padding = new System.Windows.Forms.Padding(3);
            this.tabControlador.Size = new System.Drawing.Size(273, 243);
            this.tabControlador.TabIndex = 1;
            this.tabControlador.Text = "Controlador";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbProtocolo16);
            this.groupBox1.Controls.Add(this.rbProtocolo32);
            this.groupBox1.Location = new System.Drawing.Point(9, 109);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(258, 44);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Protocolo";
            // 
            // rbProtocolo16
            // 
            this.rbProtocolo16.AutoSize = true;
            this.rbProtocolo16.Checked = true;
            this.rbProtocolo16.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbProtocolo16.Location = new System.Drawing.Point(22, 19);
            this.rbProtocolo16.Name = "rbProtocolo16";
            this.rbProtocolo16.Size = new System.Drawing.Size(84, 17);
            this.rbProtocolo16.TabIndex = 4;
            this.rbProtocolo16.TabStop = true;
            this.rbProtocolo16.Text = "16 surtidores";
            this.rbProtocolo16.UseVisualStyleBackColor = true;
            // 
            // rbProtocolo32
            // 
            this.rbProtocolo32.AutoSize = true;
            this.rbProtocolo32.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbProtocolo32.Location = new System.Drawing.Point(144, 19);
            this.rbProtocolo32.Name = "rbProtocolo32";
            this.rbProtocolo32.Size = new System.Drawing.Size(84, 17);
            this.rbProtocolo32.TabIndex = 5;
            this.rbProtocolo32.Text = "32 surtidores";
            this.rbProtocolo32.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(9, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 16);
            this.label2.TabIndex = 10;
            this.label2.Text = "IP del controlador";
            // 
            // txtIp
            // 
            this.txtIp.Location = new System.Drawing.Point(9, 83);
            this.txtIp.Name = "txtIp";
            this.txtIp.Size = new System.Drawing.Size(257, 20);
            this.txtIp.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 16);
            this.label1.TabIndex = 8;
            this.label1.Text = "Marca de controlador";
            // 
            // boxTipo
            // 
            this.boxTipo.BackColor = System.Drawing.Color.DarkSalmon;
            this.boxTipo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.boxTipo.Location = new System.Drawing.Point(9, 37);
            this.boxTipo.Name = "boxTipo";
            this.boxTipo.Size = new System.Drawing.Size(257, 21);
            this.boxTipo.TabIndex = 7;
            // 
            // btnSalir
            // 
            this.btnSalir.BackColor = System.Drawing.Color.DarkSalmon;
            this.btnSalir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalir.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSalir.Location = new System.Drawing.Point(145, 276);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(126, 37);
            this.btnSalir.TabIndex = 2;
            this.btnSalir.Text = "Salir";
            this.btnSalir.UseVisualStyleBackColor = false;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.Description = "Selecciona la carpeta PROY_NUEVO";
            this.folderBrowserDialog1.ShowNewFolderButton = false;
            // 
            // ConfiguracionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSalmon;
            this.ClientSize = new System.Drawing.Size(281, 323);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.tabs);
            this.Name = "ConfiguracionForm";
            this.ShowIcon = false;
            this.Text = "Configuración";
            this.Load += new System.EventHandler(this.ConfiguracionForm_Load);
            this.tabGeneral.ResumeLayout(false);
            this.tabGeneral.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSegFacturacion)).EndInit();
            this.tabs.ResumeLayout(false);
            this.tabControlador.ResumeLayout(false);
            this.tabControlador.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.Button btnSalir;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtProyNuevo;
        private System.Windows.Forms.Button btnSearchFolder;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TabPage tabControlador;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbProtocolo16;
        private System.Windows.Forms.RadioButton rbProtocolo32;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtIp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox boxTipo;
        private System.Windows.Forms.ComboBox boxNivelLog;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown numSegFacturacion;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
    }
}