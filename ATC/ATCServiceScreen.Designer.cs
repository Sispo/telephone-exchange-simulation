namespace ATC
{
    partial class ATCServiceScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ATCServiceScreen));
            this.firstATCBtn = new System.Windows.Forms.Button();
            this.secondATCBtn = new System.Windows.Forms.Button();
            this.anotherBtn = new System.Windows.Forms.Button();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.onlineLbl = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // firstATCBtn
            // 
            this.firstATCBtn.BackColor = System.Drawing.Color.DarkCyan;
            this.firstATCBtn.Font = new System.Drawing.Font("Segoe UI Light", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.firstATCBtn.ForeColor = System.Drawing.Color.White;
            this.firstATCBtn.Location = new System.Drawing.Point(29, 65);
            this.firstATCBtn.Name = "firstATCBtn";
            this.firstATCBtn.Size = new System.Drawing.Size(166, 67);
            this.firstATCBtn.TabIndex = 1;
            this.firstATCBtn.Text = "Mini";
            this.firstATCBtn.UseVisualStyleBackColor = false;
            this.firstATCBtn.Click += new System.EventHandler(this.firstATCBtn_Click);
            // 
            // secondATCBtn
            // 
            this.secondATCBtn.BackColor = System.Drawing.Color.DarkCyan;
            this.secondATCBtn.Font = new System.Drawing.Font("Segoe UI Light", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.secondATCBtn.ForeColor = System.Drawing.Color.White;
            this.secondATCBtn.Location = new System.Drawing.Point(29, 153);
            this.secondATCBtn.Name = "secondATCBtn";
            this.secondATCBtn.Size = new System.Drawing.Size(166, 67);
            this.secondATCBtn.TabIndex = 2;
            this.secondATCBtn.Text = "City";
            this.secondATCBtn.UseVisualStyleBackColor = false;
            this.secondATCBtn.Click += new System.EventHandler(this.secondATCBtn_Click);
            // 
            // anotherBtn
            // 
            this.anotherBtn.BackColor = System.Drawing.Color.White;
            this.anotherBtn.Font = new System.Drawing.Font("Segoe UI Light", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.anotherBtn.ForeColor = System.Drawing.Color.Black;
            this.anotherBtn.Location = new System.Drawing.Point(29, 244);
            this.anotherBtn.Name = "anotherBtn";
            this.anotherBtn.Size = new System.Drawing.Size(166, 67);
            this.anotherBtn.TabIndex = 3;
            this.anotherBtn.Text = "Another";
            this.anotherBtn.UseVisualStyleBackColor = false;
            this.anotherBtn.Click += new System.EventHandler(this.anotherBtn_Click);
            // 
            // dataGridView
            // 
            this.dataGridView.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Location = new System.Drawing.Point(247, 65);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.Size = new System.Drawing.Size(166, 246);
            this.dataGridView.TabIndex = 4;
            // 
            // onlineLbl
            // 
            this.onlineLbl.AutoSize = true;
            this.onlineLbl.Location = new System.Drawing.Point(243, 28);
            this.onlineLbl.Name = "onlineLbl";
            this.onlineLbl.Size = new System.Drawing.Size(59, 21);
            this.onlineLbl.TabIndex = 5;
            this.onlineLbl.Text = "Online:";
            // 
            // ATCServiceScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(455, 352);
            this.Controls.Add(this.onlineLbl);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.anotherBtn);
            this.Controls.Add(this.secondATCBtn);
            this.Controls.Add(this.firstATCBtn);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ATCServiceScreen";
            this.Text = "ATC Service";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button firstATCBtn;
        private System.Windows.Forms.Button secondATCBtn;
        private System.Windows.Forms.Button anotherBtn;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Label onlineLbl;
    }
}