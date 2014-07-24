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
			this.lstDatabases = new System.Windows.Forms.ListBox();
			this.lstTables = new System.Windows.Forms.ListBox();
			this.btnCreate = new System.Windows.Forms.Button();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.btnExit = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lstDatabases
			// 
			this.lstDatabases.FormattingEnabled = true;
			this.lstDatabases.ItemHeight = 20;
			this.lstDatabases.Location = new System.Drawing.Point(8, 32);
			this.lstDatabases.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.lstDatabases.Name = "lstDatabases";
			this.lstDatabases.Size = new System.Drawing.Size(376, 104);
			this.lstDatabases.TabIndex = 1;
			this.lstDatabases.TabStop = false;
			this.lstDatabases.Tag = "データベース";
			this.lstDatabases.DoubleClick += new System.EventHandler(this.lstDatabases_DoubleClick);
			// 
			// lstTables
			// 
			this.lstTables.FormattingEnabled = true;
			this.lstTables.ItemHeight = 20;
			this.lstTables.Location = new System.Drawing.Point(8, 172);
			this.lstTables.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.lstTables.Name = "lstTables";
			this.lstTables.Size = new System.Drawing.Size(376, 104);
			this.lstTables.TabIndex = 3;
			this.lstTables.TabStop = false;
			this.lstTables.Tag = "テーブル";
			// 
			// btnCreate
			// 
			this.btnCreate.Location = new System.Drawing.Point(184, 284);
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
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(113, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "データベース一覧";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(8, 148);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(87, 20);
			this.label2.TabIndex = 2;
			this.label2.Text = "テーブル一覧";
			// 
			// btnExit
			// 
			this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnExit.Location = new System.Drawing.Point(288, 284);
			this.btnExit.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.btnExit.Name = "btnExit";
			this.btnExit.Size = new System.Drawing.Size(100, 38);
			this.btnExit.TabIndex = 5;
			this.btnExit.Text = "終了";
			this.btnExit.UseVisualStyleBackColor = true;
			this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
			// 
			// Form1
			// 
			this.AcceptButton = this.btnCreate;
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.WhiteSmoke;
			this.CancelButton = this.btnExit;
			this.ClientSize = new System.Drawing.Size(394, 352);
			this.Controls.Add(this.btnExit);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnCreate);
			this.Controls.Add(this.lstTables);
			this.Controls.Add(this.lstDatabases);
			this.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.MaximizeBox = false;
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "テーブル定義書出力";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.ListBox lstDatabases;
        private System.Windows.Forms.ListBox lstTables;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnExit;
    }
}

