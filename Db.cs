using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Proyecto_Agraria_Pacifico
{
    public static class Db
    {
        private static string ConnStr
        {
            get
            {
                var cs = ConfigurationManager.ConnectionStrings["AgrariaDb"];
                if (cs == null) throw new InvalidOperationException("Falta connectionStrings:AgrariaDb en App.config");
                return cs.ConnectionString;
            }
        }

        public static SqlConnection Open()
        {
            var cn = new SqlConnection(ConnStr);
            cn.Open();
            return cn;
        }

        public static int Exec(string sql, params SqlParameter[] ps)
        {
            using (var cn = Open())
            using (var cmd = new SqlCommand(sql, cn))
            {
                if (ps != null && ps.Length > 0) cmd.Parameters.AddRange(ps);
                return cmd.ExecuteNonQuery();
            }
        }

        public static T Scalar<T>(string sql, params SqlParameter[] ps)
        {
            using (var cn = Open())
            using (var cmd = new SqlCommand(sql, cn))
            {
                if (ps != null && ps.Length > 0) cmd.Parameters.AddRange(ps);
                object r = cmd.ExecuteScalar();
                if (r == null || r == DBNull.Value) return default(T);
                return (T)Convert.ChangeType(r, typeof(T));
            }
        }

        public static DataTable Table(string sql, params SqlParameter[] ps)
        {
            using (var cn = Open())
            using (var da = new SqlDataAdapter(sql, cn))
            {
                if (ps != null && ps.Length > 0) da.SelectCommand.Parameters.AddRange(ps);
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public static void InTx(Action<SqlConnection, SqlTransaction> work)
        {
            using (var cn = Open())
            using (var tx = cn.BeginTransaction())
            {
                try
                {
                    work(cn, tx);
                    tx.Commit();
                }
                catch
                {
                    try { tx.Rollback(); } catch { }
                    throw;
                }
            }
        }
    }
}
