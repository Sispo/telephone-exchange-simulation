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
            this.loginBtn = new System.Windows.Forms.Button();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.numberTextBox = new System.Windows.Forms.TextBox();
            this.checkBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // loginBtn
            // 
            this.loginBtn.BackColor = System.Drawing.Color.DarkCyan;
            this.loginBtn.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loginBtn.ForeColor = System.Drawing.Color.White;
            this.loginBtn.Location = new System.Drawing.Point(24, 134);
            this.loginBtn.Name = "loginBtn";
            this.loginBtn.Size = new System.Drawing.Size(184, 39);
            this.loginBtn.TabIndex = 5;
            this.loginBtn.Text = "Enable";
            this.loginBtn.UseVisualStyleBackColor = false;
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.ForeColor = System.Drawing.Color.DimGray;
            this.passwordTextBox.Location = new System.Drawing.Point(24, 63);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.Size = new System.Drawing.Size(184, 29);
            this.passwordTextBox.TabIndex = 4;
            this.passwordTextBox.Text = "Automatic ID";
            // 
            // numberTextBox
            // 
            this.numberTextBox.ForeColor = System.Drawing.Color.DimGray;
            this.numberTextBox.Location = new System.Drawing.Point(24, 24);
            this.numberTextBox.Name = "numberTextBox";
            this.numberTextBox.Size = new System.Drawing.Size(184, 29);
            this.numberTextBox.TabIndex = 3;
            this.numberTextBox.Text = "Enter ATC Name";
            // 
            // checkBox
            // 
            this.checkBox.AutoSize = true;
            this.checkBox.Checked = true;
            this.checkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox.Location = new System.Drawing.Point(25, 103);
            this.checkBox.Name = "checkBox";
            this.checkBox.Size = new System.Drawing.Size(183, 21);
            this.checkBox.TabIndex = 6;
            this.checkBox.Text = "I want to enable a new ATC";
            this.checkBox.UseVisualStyleBackColor = true;
            // 
            // ATCLoginScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(232, 185);
            this.Controls.Add(this.checkBox);
            this.Controls.Add(this.loginBtn);
            this.Controls.Add(this.passwordTextBox);
            this.Controls.Add(this.numberTextBox);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ATCLoginScreen";
            this.Text = "ATC Login";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button loginBtn;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.TextBox numberTextBox;
        private System.Windows.Forms.CheckBox checkBox;
    }
}