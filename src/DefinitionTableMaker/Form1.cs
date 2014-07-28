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
                this.GetDatabases();
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
            this.GetTables(this.lstDatabases.SelectedValue.ToString());
        }

        /// <summary>
        /// 定義書作成クリックイベント
        /// </summary>
        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (!this.InputCheck()) { return; }

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
        private bool InputCheck()
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
                var s = Path.GetExtension(this.saveFileDialog1.FileName).Replace(".","");

                if (s != "html" || s != "html")
                {
                    this.saveFileDialog1.FileName = Path.ChangeExtension(this.saveFileDialog1.FileName, ".html");
                }
                using (var sw = new StreamWriter(this.saveFileDialog1.FileName, false, Encoding.GetEncoding("utf-8")))
                {
                    try
                    {
                        sw.Write(html);
                        MessageBox.Show("できあがり～");

                        this.lstTables.DataSource = null;
                    }
                    finally
                    {
                        sw.Close();
                    }
                }
            }
        }
        /// <summary>
        /// データベース一覧取得
        /// </summary>
        private void GetDatabases()
        {
            DataTable dt;
            try
            {
                dt = this.ExecuteCommand("select name from sysdatabases", this.ConnectString);
            }
            catch
            {
                throw;
            }

            this.lstDatabases.ValueMember = "name";
            this.lstDatabases.DisplayMember = "name";
            this.lstDatabases.DataSource = dt;
        }

        /// <summary>
        /// テーブル一覧取得
        /// </summary>
        /// <param name="dbName">DB名</param>
        private void GetTables(string dbName)
        {
            var connectString = string.Format("{0} Initial Catalog={1};", this.ConnectString, dbName);

            DataTable dt;
            try
            {
                dt = this.ExecuteCommand(
                        "select t.name from sys.tables t order by t.name",
                        connectString
                        );
            }
            catch
            {
                throw;
            }

            this.lstTables.DisplayMember = "name";
            this.lstTables.ValueMember = "name";
            this.lstTables.DataSource = dt;
        }

        /// <summary>
        /// テーブル定義情報取得
        /// </summary>
        /// <param name="dbName">DB名</param>
        /// <param name="tableName">テーブル名</param>
        /// <returns>取得結果@DataTable</returns>
        private DataTable GetColumnInfo(string dbName, string tableName)
        {
            var sql = new StringBuilder();

            sql.Length = 0;
            sql.AppendLine("select");
            sql.AppendLine("     c.ORDINAL_POSITION as No --");
            sql.AppendLine("    ,c.COLUMN_NAME      as 列ID --物理ID");
            sql.AppendLine("    ,c.DATA_TYPE        as データ型 --データ型");
            sql.AppendLine("    ,case");
            sql.AppendLine("       when c.CHARACTER_MAXIMUM_LENGTH is null then '-'");
            sql.AppendLine("       when c.CHARACTER_MAXIMUM_LENGTH = '-1' then 'max'");
            sql.AppendLine("       else convert(varchar,c.CHARACTER_MAXIMUM_LENGTH)");
            sql.AppendLine("     end as バイト数");
            sql.AppendLine("    ,case");
            sql.AppendLine("       when c.NUMERIC_PRECISION is null then '-'");
            sql.AppendLine("       else convert(varchar,c.NUMERIC_PRECISION)");
            sql.AppendLine("     end as 桁数");
            sql.AppendLine("    ,case");
            sql.AppendLine("       when c.NUMERIC_SCALE is null then '-'");
            sql.AppendLine("       else convert(varchar, c.NUMERIC_SCALE)");
            sql.AppendLine("     end as 精度");
            sql.AppendLine("    ,case");
            sql.AppendLine("       when c.IS_NULLABLE = 'YES' then '○'");
            sql.AppendLine("       else ''");
            sql.AppendLine("     end as Null許容");
            sql.AppendLine("from information_schema.columns c");
            sql.AppendLine("left join (");
            sql.AppendLine("            select");
            sql.AppendLine("                 k.table_catalog");
            sql.AppendLine("                ,k.table_schema");
            sql.AppendLine("                ,k.table_name");
            sql.AppendLine("                ,k.column_name");
            sql.AppendLine("                ,t.constraint_type");
            sql.AppendLine("            from information_schema.key_column_usage k");
            sql.AppendLine("            inner join information_schema.table_constraints t");
            sql.AppendLine("               on t.constraint_name = k.constraint_name");
            sql.AppendLine("            where t.constraint_type = 'primary key'");
            sql.AppendLine("               or t.constraint_type = 'unique'");
            sql.AppendLine("          ) p");
            sql.AppendLine(" on c.table_catalog = p.table_catalog");
            sql.AppendLine("and c.table_schema  = p.table_schema");
            sql.AppendLine("and c.table_name    = p.table_name");
            sql.AppendLine("and c.column_name   = p.column_name");
            sql.AppendFormat("where c.TABLE_NAME = '{0}'\n", tableName);
            sql.AppendLine("order by");
            sql.AppendLine("   c.ordinal_position");

            return this.ExecuteCommand(sql.ToString(), string.Format("{0} Initial Catalog={1};", this.ConnectString, dbName));
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
            html.AppendLine("  <tr>");
            //カラムヘッダ作成
            foreach (DataColumn col in table.Columns)
            {
                html.AppendFormat("    <th>{0}</th>\n", col.Caption);
            }
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
                try
                {
                    adpt.Fill(dt);
                }
                finally
                {
                    conn.Close();
                }
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
