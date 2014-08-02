namespace DefinitionTableMaker
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
			this.btnCreate = new System.Windows.Forms.Button();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.btnExit = new System.Windows.Forms.Button();
			this.cmbDatabases = new System.Windows.Forms.ComboBox();
			this.cmbTables = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// btnCreate
			// 
			this.btnCreate.Location = new System.Drawing.Point(56, 84);
			this.btnCreate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.btnCreate.Name = "btnCreate";
			this.btnCreate.Size = new System.Drawing.Size(100, 38);
			this.btnCreate.TabIndex = 4;
			this.btnCreate.Text = "定義書作成";
			this.btnCreate.UseVisualStyleBackColor = true;
			this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(128, 28);
			this.label1.TabIndex = 0;
			this.label1.Text = "データベース一覧";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 44);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(120, 28);
			this.label2.TabIndex = 2;
			this.label2.Text = "テーブル一覧";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnExit
			// 
			this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnExit.Location = new System.Drawing.Point(160, 84);
			this.btnExit.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.btnExit.Name = "btnExit";
			this.btnExit.Size = new System.Drawing.Size(100, 38);
			this.btnExit.TabIndex = 5;
			this.btnExit.Text = "終了";
			this.btnExit.UseVisualStyleBackColor = true;
			this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
			// 
			// cmbDatabases
			// 
			this.cmbDatabases.DisplayMember = "name";
			this.cmbDatabases.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbDatabases.FormattingEnabled = true;
			this.cmbDatabases.Location = new System.Drawing.Point(136, 8);
			this.cmbDatabases.Name = "cmbDatabases";
			this.cmbDatabases.Size = new System.Drawing.Size(121, 28);
			this.cmbDatabases.TabIndex = 6;
			this.cmbDatabases.ValueMember = "name";
			this.cmbDatabases.SelectedValueChanged += new System.EventHandler(this.cmbDatabases_SelectedValueChanged);
			this.cmbDatabases.Leave += new System.EventHandler(this.cmbDatabases_Leave);
			// 
			// cmbTables
			// 
			this.cmbTables.DisplayMember = "name";
			this.cmbTables.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbTables.FormattingEnabled = true;
			this.cmbTables.Location = new System.Drawing.Point(136, 44);
			this.cmbTables.Name = "cmbTables";
			this.cmbTables.Size = new System.Drawing.Size(121, 28);
			this.cmbTables.TabIndex = 7;
			this.cmbTables.ValueMember = "name";
			// 
			// Form1
			// 
			this.AcceptButton = this.btnCreate;
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.WhiteSmoke;
			this.CancelButton = this.btnExit;
			this.ClientSize = new System.Drawing.Size(271, 129);
			this.Controls.Add(this.cmbTables);
			this.Controls.Add(this.cmbDatabases);
			this.Controls.Add(this.btnExit);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnCreate);
			this.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.MaximizeBox = false;
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "テーブル定義書出力";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnExit;
		private System.Windows.Forms.ComboBox cmbDatabases;
		private System.Windows.Forms.ComboBox cmbTables;
    }
}

