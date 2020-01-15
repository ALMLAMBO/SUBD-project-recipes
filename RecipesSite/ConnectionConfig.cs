using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace RecipesSite {
	public class ConnectionConfig {
		private MySqlConnectionStringBuilder csb;

		public ConnectionConfig() {
			csb = new MySqlConnectionStringBuilder() {
				Server = "35.232.190.49",
				UserID = "root",
				Password = "agent007",
				Database = "recipessite",
				//SslCa = SslCertificatesPaths.ServerCaFile,
				//SslCert = SslCertificatesPaths.ClientCertFile,
				//SslKey = SslCertificatesPaths.ClientKeyFile,
				//SslMode = MySqlSslMode.Required
			};
		}

		public string getVersion() {
			string version = "";

			using (MySqlConnection conn = 
				new MySqlConnection(csb.ConnectionString)) {

				conn.Open();

				string sql = "select version();";
				MySqlCommand command = new MySqlCommand(sql, conn);
				version = command.ExecuteScalar().ToString();
			}

			return version;
		}

		public string GetConnectionString() {
			return csb.ConnectionString;
		}
	}
}
