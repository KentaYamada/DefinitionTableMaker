using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DefinitionTableMaker
{
	public class DBCommander
	{
		private readonly string ConnectString;

		public DBCommander()
		{
			this.ConnectString = @"Data Source=(local)\SQLEXPRESS; Integrated Security=SSPI;";
		}

		public DBCommander(string dbName)
		{
			this.ConnectString = string.Format(@"Data Source=(local)\SQLEXPRESS; Integrated Security=SSPI; Initial Catalog={0};", dbName);
		}

		public DataTable ExecuteCommand(string sql, params object[] args)
		{
			var dt = new DataTable();

			using (var conn = new SqlConnection(ConnectString))
			using (var comm = new SqlCommand(sql, conn))
			using (var adpt = new SqlDataAdapter(comm))
			{
				adpt.Fill(dt);
			}
			return dt;
		}
	}
}
