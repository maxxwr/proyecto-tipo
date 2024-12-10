
namespace ConsultaTipoCambio
{
    partial class FrmDemo3PaginaSUNAT
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnConsultarTipoCambioUrlSUNAT = new System.Windows.Forms.Button();
            this.lblMensaje = new System.Windows.Forms.Label();
            this.txtAnio = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.dgvTipoCambio = new System.Windows.Forms.DataGridView();
            this.cboMes = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTipoCambio)).BeginInit();
            this.SuspendLayout();
            // 
            // btnConsultarTipoCambioUrlSUNAT
            // 
            this.btnConsultarTipoCambioUrlSUNAT.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnConsultarTipoCambioUrlSUNAT.BackColor = System.Drawing.Color.Firebrick;
            this.btnConsultarTipoCambioUrlSUNAT.Enabled = false;
            this.btnConsultarTipoCambioUrlSUNAT.FlatAppearance.BorderSize = 0;
            this.btnConsultarTipoCambioUrlSUNAT.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConsultarTipoCambioUrlSUNAT.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConsultarTipoCambioUrlSUNAT.ForeColor = System.Drawing.Color.White;
            this.btnConsultarTipoCambioUrlSUNAT.Location = new System.Drawing.Point(192, 81);
            this.btnConsultarTipoCambioUrlSUNAT.Name = "btnConsultarTipoCambioUrlSUNAT";
            this.btnConsultarTipoCambioUrlSUNAT.Size = new System.Drawing.Size(100, 40);
            this.btnConsultarTipoCambioUrlSUNAT.TabIndex = 3;
            this.btnConsultarTipoCambioUrlSUNAT.Text = "Consultar Tipo de cambio";
            this.btnConsultarTipoCambioUrlSUNAT.UseVisualStyleBackColor = false;
            this.btnConsultarTipoCambioUrlSUNAT.Click += new System.EventHandler(this.btnConsultarTipoCambioUrlSUNAT_Click);
            // 
            // lblMensaje
            // 
            this.lblMensaje.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMensaje.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMensaje.ForeColor = System.Drawing.Color.Firebrick;
            this.lblMensaje.Location = new System.Drawing.Point(12, 124);
            this.lblMensaje.Name = "lblMensaje";
            this.lblMensaje.Size = new System.Drawing.Size(460, 15);
            this.lblMensaje.TabIndex = 49;
            this.lblMensaje.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtAnio
            // 
            this.txtAnio.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtAnio.Enabled = false;
            this.txtAnio.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAnio.Location = new System.Drawing.Point(266, 39);
            this.txtAnio.MaxLength = 4;
            this.txtAnio.Name = "txtAnio";
            this.txtAnio.Size = new System.Drawing.Size(100, 22);
            this.txtAnio.TabIndex = 2;
            this.txtAnio.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ValidarSoloNumero_KeyPress);
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(122, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 15);
            this.label5.TabIndex = 51;
            this.label5.Text = "Mes";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(263, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(28, 15);
            this.label6.TabIndex = 51;
            this.label6.Text = "Año";
            // 
            // dgvTipoCambio
            // 
            this.dgvTipoCambio.AllowUserToAddRows = false;
            this.dgvTipoCambio.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTipoCambio.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvTipoCambio.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTipoCambio.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvTipoCambio.ColumnHeadersHeight = 40;
            this.dgvTipoCambio.Location = new System.Drawing.Point(12, 158);
            this.dgvTipoCambio.Name = "dgvTipoCambio";
            this.dgvTipoCambio.RowHeadersWidth = 40;
            this.dgvTipoCambio.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvTipoCambio.RowTemplate.Height = 25;
            this.dgvTipoCambio.Size = new System.Drawing.Size(460, 198);
            this.dgvTipoCambio.TabIndex = 4;
            // 
            // cboMes
            // 
            this.cboMes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMes.Enabled = false;
            this.cboMes.Font = new System.Drawing.Font("Arial", 9.75F);
            this.cboMes.FormattingEnabled = true;
            this.cboMes.Location = new System.Drawing.Point(125, 39);
            this.cboMes.Name = "cboMes";
            this.cboMes.Size = new System.Drawing.Size(121, 24);
            this.cboMes.TabIndex = 52;
            // 
            // FrmDemo3PaginaSUNAT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 361);
            this.Controls.Add(this.cboMes);
            this.Controls.Add(this.dgvTipoCambio);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtAnio);
            this.Controls.Add(this.lblMensaje);
            this.Controls.Add(this.btnConsultarTipoCambioUrlSUNAT);
            this.MaximizeBox = false;
            this.Name = "FrmDemo3PaginaSUNAT";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Consulta directa a la página de SUNAT";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmDemo3PaginaSUNAT_FormClosing);
            this.Load += new System.EventHandler(this.FrmDemo3PaginaSUNAT_Load);
            this.Shown += new System.EventHandler(this.FrmDemo3PaginaSUNAT_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTipoCambio)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConsultarTipoCambioUrlSUNAT;
        private System.Windows.Forms.Label lblMensaje;
        private System.Windows.Forms.TextBox txtAnio;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridView dgvTipoCambio;
        private System.Windows.Forms.ComboBox cboMes;
    }
}

