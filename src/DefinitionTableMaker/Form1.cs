using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DefinitionTableMaker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
			var comm = new DBCommander();

            try
            {
				this.cmbDatabases.DataSource = comm.ExecuteCommand("select name from sysdatabases");
                
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.Message, "アプリケーションエラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

		}

		
		private void cmbDatabases_SelectedValueChanged(object sender, EventArgs e)
		{
			
		}

        /// <summary>
        /// 定義書作成クリックイベント
        /// </summary>
        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (!this.AreListBoxesChecked()) 
			{
				return; 
			}

			var sql = string.Format(@"
            SELECT
                 c.ORDINAL_POSITION as No
                ,c.COLUMN_NAME      as 列ID
                ,c.DATA_TYPE        as データ型
                ,CASE
                   WHEN c.CHARACTER_MAXIMUM_LENGTH is null then '-'
                   WHEN c.CHARACTER_MAXIMUM_LENGTH = '-1' then 'max'
                   ELSE convert(varchar,c.CHARACTER_MAXIMUM_LENGTH)
                 END AS バイト数
                ,CASE
                   WHEN c.NUMERIC_PRECISION is null then '-'
                   ELSE convert(varchar,c.NUMERIC_PRECISION)
                 END AS 桁数
                ,CASE
                   WHEN c.NUMERIC_SCALE is null then '-'
                   ELSE convert(varchar, c.NUMERIC_SCALE)
                 END AS 精度
                ,CASE
                   WHEN c.IS_NULLABLE = 'YES' then '○'
                   ELSE ''
                 END AS Null許容
            FROM information_schema.columns c
            LEFT JOIN (
                        SELECT
                             k.table_catalog
                            ,k.table_schema
                            ,k.table_name
                            ,k.column_name
                            ,t.constraint_type
                        FROM information_schema.key_column_usage k
                        INNER join information_schema.table_constraints t
                           ON t.constraint_name = k.constraint_name
                        WHERE t.constraint_type = 'primary key'
                           OR t.constraint_type = 'unique'
                      ) p
             ON c.table_catalog = p.table_catalog
            AND c.table_schema  = p.table_schema
            AND c.table_name    = p.table_name
            AND c.column_name   = p.column_name
            WHERE c.TABLE_NAME = '{0}'
            ORDER BY
            c.ordinal_position"
			, this.cmbTables.SelectedValue);

			var comm = new DBCommander(this.cmbDatabases.SelectedValue.ToString());
            DataTable dt = null;

            try
            {
				dt = comm.ExecuteCommand(sql);
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.Message, "アプリケーションエラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            var html = this.CreateHtml(dt);
            try
            {
                this.SaveFile(html);
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message, "アプリケーションエラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// リストボックス選択チェック
        /// </summary>
        /// <returns>True:正常 False:未選択</returns>
        private bool AreListBoxesChecked()
        {
            var target = this.Controls.OfType<ListBox>()
                .Where(x => x.SelectedValue == null)
                .ToList();

            if (0 < target.Count)
            {
                MessageBox.Show(
                    string.Format("{0}を選択して下さい。", target.First().Tag),
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
				return false;
            }
            return true;
        }

        /// <summary>
        /// HTMLデータ保存
        /// </summary>
        /// <param name="html">生成されたHTMLタグ</param>
        private void SaveFile(string html)
        {
            this.saveFileDialog1.Filter = "HTMLファイル(*.html;*.htm)|*.html;*.htm";

            if (DialogResult.OK == this.saveFileDialog1.ShowDialog())
            {
                var s = Path.GetExtension(this.saveFileDialog1.FileName).Replace(".", "");

                if (s != "html" || s != "html")
                {
                    this.saveFileDialog1.FileName = Path.ChangeExtension(this.saveFileDialog1.FileName, ".html");
                }
                using (var sw = new StreamWriter(this.saveFileDialog1.FileName, false, Encoding.GetEncoding("utf-8")))
                {
                    sw.Write(html);
                    MessageBox.Show("できあがり～");
                    (this.cmbTables.DataSource as DataTable).Rows.Clear();
                }
            }
        }

        /// <summary>
        /// HTMLタグ生成
        /// </summary>
        /// <param name="table">テーブル定義情報</param>
        /// <returns>生成したHTMLタグ</returns>
        private string CreateHtml(DataTable table)
        {
            var html = new StringBuilder();

            html.Length = 0;
            html.AppendLine("<!DOCTYPE html>");
            html.AppendLine("<html>");
            html.AppendLine("<head>");
            html.AppendLine("  <meta charset=\"utf-8\">");
            html.AppendFormat("  <title>{0}テーブル定義書</title>", this.cmbTables.SelectedValue);
            html.AppendLine("  <style type=\"text/css\">");
            html.AppendLine("    table, th, td { border:1px #000000 solid; }");
            html.AppendLine("  </style>");
            html.AppendLine("</head>");
            html.AppendLine("<body>");
            html.AppendLine("  <div align=\"center\">");
            html.AppendLine("  <table>");
            html.AppendLine("  <tr>");
            html.AppendLine("  <th>データベース名</th>");
            html.AppendFormat("  <td>{0}</td>\n", this.cmbDatabases.SelectedValue);
            html.AppendLine("  </tr>");
            html.AppendLine("  <tr>");
            html.AppendLine("  <th>テーブル名</th>");
            html.AppendFormat("  <td>{0}</td>\n", this.cmbTables.SelectedValue);
            html.AppendLine("  </tr>");
            html.AppendLine("  </table></br>");
            html.AppendLine("  <table>");
            //カラムヘッダ作成
            html.AppendLine("  <tr>");
            table.Columns.OfType<DataColumn>()
                .Select(col => string.Format("    <th>{0}</th>\n", col.Caption))
                .ToList()
                .ForEach(row_str => html.AppendFormat(row_str));
            html.AppendLine("</tr>");

            foreach (DataRow dr in table.Rows)
            {
                html.AppendLine("<tr>");
                foreach (DataColumn col in table.Columns)
                {
                    html.AppendFormat("    <td>{0}</td>\n", dr[col]);
                }
                html.AppendLine("</tr>");
            }
            html.AppendLine("</table>");
            html.AppendLine("</div>");
            html.AppendLine("</body>");
            html.AppendLine("</html>");

            return html.ToString();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
            Application.Exit();
        }

		private void cmbDatabases_Leave(object sender, EventArgs e)
		{
			var comm = new DBCommander(this.cmbDatabases.SelectedValue.ToString());

			try
			{
				this.cmbTables.DataSource = comm.ExecuteCommand("select t.name from sys.tables as t order by t.name");
			}
			catch (SqlException ex)
			{
				MessageBox.Show(ex.Message, "アプリケーションエラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}
    }
}
