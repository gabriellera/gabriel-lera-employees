namespace UI
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.DataGridView dataGridViewResults;
        private System.Windows.Forms.Label lblStatus;

        /// <summary>
        ///  Clean up any resources being used.
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
        private void InitializeComponent()
        {
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.dataGridViewResults = new System.Windows.Forms.DataGridView();
            this.lblStatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).BeginInit();
            this.SuspendLayout();

            // btnSelectFile
            this.btnSelectFile.Location = new System.Drawing.Point(12, 12);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(120, 30);
            this.btnSelectFile.TabIndex = 0;
            this.btnSelectFile.Text = "Select CSV File";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);

            // lblStatus
            this.lblStatus.Location = new System.Drawing.Point(150, 12);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(400, 30);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "Status";

            // dataGridViewResults
            this.dataGridViewResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewResults.Location = new System.Drawing.Point(12, 50);
            this.dataGridViewResults.Name = "dataGridViewResults";
            this.dataGridViewResults.Size = new System.Drawing.Size(600, 400);
            this.dataGridViewResults.TabIndex = 2;

            // Form1
            this.ClientSize = new System.Drawing.Size(624, 462);
            this.Controls.Add(this.dataGridViewResults);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnSelectFile);
            this.Name = "Form1";
            this.Text = "Employee Collaboration Viewer";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
