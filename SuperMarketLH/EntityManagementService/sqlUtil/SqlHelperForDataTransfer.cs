using EntityManagementService.entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace EntityManagementService.sqlUtil
{
    public class SqlHelperForDataTransfer
    {
        public static SQLiteConnection sqLiteConnection;
        public static void openConnection()
        {

            sqLiteConnection = new SQLiteConnection("DataSource=" + AppDomain.CurrentDomain.BaseDirectory + "DataTransfer;Version=3;LockingMode=normal");
            sqLiteConnection.Open();
        }

        public static void executeSql(String sql)
        {
            openConnection();
            using (SQLiteCommand cmd = new SQLiteCommand(sql, sqLiteConnection))
            {
                cmd.ExecuteNonQuery();
            }
            sqLiteConnection.Close();
        }
        /// <summary>
        /// 执行sql语句，并且返回insert语句的id
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="getNewId"></param>
        /// <returns></returns>
        public static int executeSql(String sql, bool getNewId)
        {
            int rowId = 0;
            openConnection();
            using (SQLiteCommand cmd = new SQLiteCommand(sql, sqLiteConnection))
            {
                cmd.ExecuteNonQuery();
            }
            if (getNewId)
            {
                using (SQLiteCommand cmd = new SQLiteCommand("select last_insert_rowid()", sqLiteConnection))
                {
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        rowId = int.Parse(reader[0].ToString());
                    }
                }
            }
            sqLiteConnection.Close();
            return rowId;
        }



        public static SQLiteDataReader executeQuery(string sql)
        {
            //openConnection();
            SQLiteDataReader reader = null;
            using (SQLiteCommand cmd = new SQLiteCommand(sql, sqLiteConnection))
            {
                reader = cmd.ExecuteReader();
            }
            //  sqLiteConnection.Close();
            return reader;
        }
        public static DataTable executeQueryDataTable(string sql)
        {
            openConnection();
            DataSet ds;
            using (SQLiteCommand cmd = new SQLiteCommand(sql, sqLiteConnection))
            {
                using (SQLiteDataAdapter reader = new System.Data.SQLite.SQLiteDataAdapter(cmd))
                {
                    ds = new DataSet();
                    reader.Fill(ds);
                }

            }
            sqLiteConnection.Close();
            return ds.Tables[0];
        }

        /// <summary>
        /// 执行带有参数的sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        public static void executeSql(String sql, params SQLiteParameter[] param)
        {
            openConnection();
            using (SQLiteTransaction mytransaction = sqLiteConnection.BeginTransaction())
            {
                using (SQLiteCommand mycommand = new SQLiteCommand(sqLiteConnection))
                {
                    SQLiteParameter myparam = new SQLiteParameter();

                    mycommand.CommandText = sql;
                    mycommand.Parameters.AddRange(param);
                    mycommand.ExecuteNonQuery();
                }
                mytransaction.Commit();
            }
            sqLiteConnection.Close();
        }

        public static List<ClientComputer> getAllClients()
        {
            DataTable dt = executeQueryDataTable("select ip,totalFileNum,updateFileNum,status from datatransfertab");
            List<ClientComputer> rtn = new List<ClientComputer>();
            foreach (DataRow row in dt.Rows)
            {
                rtn.Add(new ClientComputer()
                {
                    IP = row[0].ToString(),
                    TotalFileNum = int.Parse(row[1].ToString()),
                    UpdateFileNum = int.Parse(row[2].ToString()),
                    State = row[3].ToString(),
                    IsConnected = false//默认不联通
                });
            }
            return rtn;
        }
        public static ClientComputer getClient(String IP)
        {
            DataTable dt = executeQueryDataTable("select ip,totalFileNum,updateFileNum,status from datatransfertab where ip='" + IP+"'");

            return new ClientComputer()
            {
                IP = dt.Rows[0][0].ToString(),
                TotalFileNum = int.Parse(dt.Rows[0][1].ToString()),
                UpdateFileNum = int.Parse(dt.Rows[0][2].ToString()),
                State = dt.Rows[0][3].ToString(),
                IsConnected = false//默认不联通
            };
        }

        /// <summary>
        /// 添加更新客户端
        /// </summary>
        public static void saveCientHost(ClientComputer client)
        {
            string sql = "insert into datatransfertab(ip,totalFileNum,updateFileNum,status) values('" + client.IP + "','" + client.TotalFileNum + "','" + client.UpdateFileNum + "','" + client.State + "')";
            executeSql(sql);
        }
        public static void delClientHost(String ip)
        {
            string sql = "delete from datatransfertab where ip='" + ip + "'";
            executeSql(sql);
        }

        public static void updateClientHost(ClientComputer client)
        {
            string sql = "update datatransfertab set totalFileNum=" + client.TotalFileNum + " and updateFileNum='" + client.UpdateFileNum + "' and status='" + client.State + "'  where ip='" + client.IP + "'";
            executeSql(sql);
        }

    }
}
