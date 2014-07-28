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
        private string _dbName;
        private readonly string ConnectString = @"Data Source=(local)\SQLEXPRESS; Integrated Security=SSPI;";

        public Form1()
        {
            InitializeComponent();
            this._dbName = string.Empty;
        }

        /// <summary>
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                this.lstDatabases.DataSource = this.GetDatabases();
                this.lstDatabases.ValueMember = "name";
                this.lstDatabases.DisplayMember = "name";
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.Message, "アプリケーションエラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// データベース一覧ダブルクリックイベント
        /// </summary>
        private void lstDatabases_DoubleClick(object sender, EventArgs e)
        {
            this.lstTables.DataSource = this.GetTables(this.lstDatabases.SelectedValue.ToString());
            this.lstTables.DisplayMember = "name";
            this.lstTables.ValueMember = "name";
        }

        /// <summary>
        /// 定義書作成クリックイベント
        /// </summary>
        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (!this.areListBoxesChecked()) { return; }

            DataTable dt;
            try
            {
                dt = this.GetColumnInfo(
                        this.lstDatabases.SelectedValue.ToString(),
                        this.lstTables.SelectedValue.ToString()
                        );
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
        private bool areListBoxesChecked()
        {
            var target = this.Controls.OfType<ListBox>()
                .where(x => x.SelectedValue == null)
                .ToList();

            if (0 < target.Count)
            {
                MessageBox.Show(
                    string.Format("{0}を選択して下さい。", target.First.Tag),
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return false
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
                    this.lstTables.DataSource = null;
                }
            }
        }

        /// <summary>
        /// データベース一覧取得
        /// </summary>
        private DataTable GetDatabases()
        {
            return this.ExecuteCommand("select name from sysdatabases", this.ConnectString);
        }

        /// <summary>
        /// テーブル一覧取得
        /// </summary>
        /// <param name="dbName">DB名</param>
        private DataTable GetTables(string dbName)
        {
            var connectString = string.Format("{0} Initial Catalog={1};", this.ConnectString, dbName);
            return this.ExecuteCommand(
                "select t.name from sys.tables t order by t.name",
                connectString
            );
        }

        /// <summary>
        /// テーブル定義情報取得
        /// </summary>
        /// <param name="dbName">DB名</param>
        /// <param name="tableName">テーブル名</param>
        /// <returns>取得結果@DataTable</returns>
        private DataTable GetColumnInfo(string dbName, string tableName)
        {
            var sql = string.Format(@"
            SELECT
                 c.ORDINAL_POSITION as No --
                ,c.COLUMN_NAME      as 列ID --物理ID
                ,c.DATA_TYPE        as データ型 --データ型
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
            , tableName);
            return this.ExecuteCommand(sql, string.Format("{0} Initial Catalog={1};", this.ConnectString, dbName));
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
            html.AppendFormat("  <title>{0}テーブル定義書</title>", this.lstTables.SelectedValue);
            html.AppendLine("  <style type=\"text/css\">");
            html.AppendLine("    table, th, td { border:1px #000000 solid; }");
            html.AppendLine("  </style>");
            html.AppendLine("</head>");
            html.AppendLine("<body>");
            html.AppendLine("  <div align=\"center\">");
            html.AppendLine("  <table>");
            html.AppendLine("  <tr>");
            html.AppendLine("  <th>データベース名</th>");
            html.AppendFormat("  <td>{0}</td>\n", this.lstDatabases.SelectedValue);
            html.AppendLine("  </tr>");
            html.AppendLine("  <tr>");
            html.AppendLine("  <th>テーブル名</th>");
            html.AppendFormat("  <td>{0}</td>\n", this.lstTables.SelectedValue);
            html.AppendLine("  </tr>");
            html.AppendLine("  </table></br>");
            html.AppendLine("  <table>");
            //カラムヘッダ作成
            html.AppendLine("  <tr>");
            table.Columns
                .Select(col => String.Format("    <th>{0}</th>\n", col.Caption))
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

        /// <summary>
        /// SQLコマンド実行
        /// </summary>
        /// <param name="sql">Select文</param>
        /// <param name="connectString">接続文字列</param>
        /// <returns>取得結果@DataTable</returns>
        private DataTable ExecuteCommand(string sql, string connectString)
        {
            var dt = new DataTable();
            using (var conn = new SqlConnection(connectString))
            using (var comm = new SqlCommand(sql.ToString(), conn))
            using (var adpt = new SqlDataAdapter(comm))
            {
                adpt.Fill(dt);
            }
            return dt;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
            Application.Exit();
        }
    }
}
