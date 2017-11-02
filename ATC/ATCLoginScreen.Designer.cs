namespace ATC
{
    partial class ATCLoginScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ATCLoginScreen));
            this.enableBtn = new System.Windows.Forms.Button();
            this.idTextBox = new System.Windows.Forms.TextBox();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // enableBtn
            // 
            this.enableBtn.BackColor = System.Drawing.Color.DarkCyan;
            this.enableBtn.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.enableBtn.ForeColor = System.Drawing.Color.White;
            this.enableBtn.Location = new System.Drawing.Point(24, 114);
            this.enableBtn.Name = "enableBtn";
            this.enableBtn.Size = new System.Drawing.Size(184, 39);
            this.enableBtn.TabIndex = 5;
            this.enableBtn.Text = "Enable";
            this.enableBtn.UseVisualStyleBackColor = false;
            this.enableBtn.Click += new System.EventHandler(this.enableBtn_Click);
            // 
            // idTextBox
            // 
            this.idTextBox.ForeColor = System.Drawing.Color.DimGray;
            this.idTextBox.Location = new System.Drawing.Point(24, 63);
            this.idTextBox.Name = "idTextBox";
            this.idTextBox.Size = new System.Drawing.Size(184, 29);
            this.idTextBox.TabIndex = 4;
            this.idTextBox.Text = "Automatic ID";
            this.idTextBox.Enter += idTextBox_Enter;
            this.idTextBox.Leave += idTextBox_Leave;
            // 
            // nameTextBox
            // 
            this.nameTextBox.ForeColor = System.Drawing.Color.DimGray;
            this.nameTextBox.Location = new System.Drawing.Point(24, 24);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(184, 29);
            this.nameTextBox.TabIndex = 3;
            this.nameTextBox.Text = "Enter ATC Name";
            this.nameTextBox.Enter += nameTextBox_Enter;
            this.nameTextBox.Leave += nameTextBox_Leave;
            // 
            // ATCLoginScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(232, 172);
            this.Controls.Add(this.enableBtn);
            this.Controls.Add(this.idTextBox);
            this.Controls.Add(this.nameTextBox);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ATCLoginScreen";
            this.Text = "ATC Login";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button enableBtn;
        private System.Windows.Forms.TextBox idTextBox;
        private System.Windows.Forms.TextBox nameTextBox;
    }
}