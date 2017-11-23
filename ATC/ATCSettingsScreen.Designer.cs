namespace ATC
{
    partial class ATCSettingsScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ATCSettingsScreen));

            this.atcGridView = new System.Windows.Forms.DataGridView();
            this.atcLbl = new System.Windows.Forms.Label();
            this.usersLbl = new System.Windows.Forms.Label();
            this.usersGridView = new System.Windows.Forms.DataGridView();
            this.connectionLbl = new System.Windows.Forms.Label();
            this.connectionsGridView = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.logsGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.atcGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.usersGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.connectionsGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.logsGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // atcGridView
            // 
            this.atcGridView.BackgroundColor = System.Drawing.Color.White;
            this.atcGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.atcGridView.Location = new System.Drawing.Point(12, 53);
            this.atcGridView.Name = "atcGridView";
            this.atcGridView.Size = new System.Drawing.Size(154, 160);
            this.atcGridView.TabIndex = 0;
            // 
            // atcLbl
            // 
            this.atcLbl.AutoSize = true;
            this.atcLbl.Location = new System.Drawing.Point(13, 13);
            this.atcLbl.Name = "atcLbl";
            this.atcLbl.Size = new System.Drawing.Size(93, 21);
            this.atcLbl.TabIndex = 1;
            this.atcLbl.Text = "Online ATCs";
            // 
            // usersLbl
            // 
            this.usersLbl.AutoSize = true;
            this.usersLbl.Location = new System.Drawing.Point(211, 13);
            this.usersLbl.Name = "usersLbl";
            this.usersLbl.Size = new System.Drawing.Size(99, 21);
            this.usersLbl.TabIndex = 3;
            this.usersLbl.Text = "Online Users";
            // 
            // usersGridView
            // 
            this.usersGridView.BackgroundColor = System.Drawing.Color.White;
            this.usersGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.usersGridView.Location = new System.Drawing.Point(210, 53);
            this.usersGridView.Name = "usersGridView";
            this.usersGridView.Size = new System.Drawing.Size(154, 160);
            this.usersGridView.TabIndex = 2;
            // 
            // connectionLbl
            // 
            this.connectionLbl.AutoSize = true;
            this.connectionLbl.Location = new System.Drawing.Point(399, 13);
            this.connectionLbl.Name = "connectionLbl";
            this.connectionLbl.Size = new System.Drawing.Size(153, 21);
            this.connectionLbl.TabIndex = 5;
            this.connectionLbl.Text = "Current Connections";
            // 
            // connectionsGridView
            // 
            this.connectionsGridView.BackgroundColor = System.Drawing.Color.White;
            this.connectionsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.connectionsGridView.Location = new System.Drawing.Point(398, 53);
            this.connectionsGridView.Name = "connectionsGridView";
            this.connectionsGridView.Size = new System.Drawing.Size(274, 160);
            this.connectionsGridView.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 232);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 21);
            this.label1.TabIndex = 7;
            this.label1.Text = "Logs";
            // 
            // logsGridView
            // 
            this.logsGridView.BackgroundColor = System.Drawing.Color.White;
            this.logsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.logsGridView.Location = new System.Drawing.Point(12, 267);
            this.logsGridView.Name = "logsGridView";
            this.logsGridView.Size = new System.Drawing.Size(660, 160);
            this.logsGridView.TabIndex = 6;
            // 
            // ATCSettingsScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 448);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.logsGridView);
            this.Controls.Add(this.connectionLbl);
            this.Controls.Add(this.connectionsGridView);
            this.Controls.Add(this.usersLbl);
            this.Controls.Add(this.usersGridView);
            this.Controls.Add(this.atcLbl);
            this.Controls.Add(this.atcGridView);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ATCSettingsScreen";
            this.Text = "Settings";
            ((System.ComponentModel.ISupportInitialize)(this.atcGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.usersGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.connectionsGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.logsGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

            this.atcGridView.SelectionChanged += dataGridView_SelectionChanged;
            this.usersGridView.SelectionChanged += dataGridView_SelectionChanged;
            this.connectionsGridView.SelectionChanged += dataGridView_SelectionChanged;
            this.logsGridView.SelectionChanged += dataGridView_SelectionChanged;
        }

        #endregion

        private System.Windows.Forms.DataGridView atcGridView;
        private System.Windows.Forms.Label atcLbl;
        private System.Windows.Forms.Label usersLbl;
        private System.Windows.Forms.DataGridView usersGridView;
        private System.Windows.Forms.Label connectionLbl;
        private System.Windows.Forms.DataGridView connectionsGridView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView logsGridView;
    }
}