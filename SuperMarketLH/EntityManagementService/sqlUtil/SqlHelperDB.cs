using EntityManagementService.entity;
using EntityManageService.sqlUtil;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace EntityManagementService.sqlUtil
{
    public class SqlHelperDB
    {
        public static SqlConnection conn;
        public static string serverName = "localhost";
        public static string databaseName = "superMarket";
        public static string userName = "sa";
        public static string password = "123456";

        public static DBServer server = SqlHelper.getDBServer();

        public static string CONNECTION_STRING = server==null || !server.Used ? "" :("server=" + server.Ip + ";database=" + databaseName + ";user id=" + server.UserName + ";pwd=" + server.Password + "; pooling=false");

        public static void openConnection()
        {
            string connStr = "server=" + serverName + ";database=" + databaseName + ";uid=" + userName + ";pwd=" + password;
            conn = new SqlConnection(connStr);
            conn.Open();
        }
        /// <summary>
        /// 初始化数据库
        /// </summary>
        public static void initDataBase()
        {
            if (CONNECTION_STRING == null) return;

            createDataBase(databaseName);

            MySqlHelper.ExecuteNonQuery(CONNECTION_STRING, "drop table if exists tabassignactivity ");
            MySqlHelper.ExecuteNonQuery(CONNECTION_STRING, "drop table if exists tabquestion ");

            string sql = "CREATE TABLE tabassignactivity(id  int(11) NOT NULL AUTO_INCREMENT PRIMARY KEY,mobileNum varchar(30),activityName varchar(100),activityId int(11)) DEFAULT CHARSET=utf8;";
            MySqlHelper.ExecuteNonQuery(CONNECTION_STRING, sql);

            sql = " CREATE TABLE tabquestion (id int(11) NOT NULL AUTO_INCREMENT PRIMARY KEY,content VARCHAR(200),a VARCHAR(200), b VARCHAR(200), c VARCHAR(200),d VARCHAR(200),e VARCHAR(200),aNum int(11) default 0,bNum int(11) default 0,cNum int(11) default 0,dNum int(11) default 0,eNum int(11)  default 0) DEFAULT CHARSET=utf8;";

            MySqlHelper.ExecuteNonQuery(CONNECTION_STRING, sql);

        }



        /// <summary>
        /// 在数据库中新建数据库
        /// </summary>
        /// <param name="dataBaseName"></param>
        private static void createDataBase(string dataBaseName)
        {
            if (CONNECTION_STRING == null) return;
            string connStr = String.Format("server={0};user id={1}; password={2}; pooling=false",
                server.Ip, server.UserName, server.Password);
            MySqlHelper.ExecuteNonQuery(connStr, "drop database if exists " + databaseName);
            MySqlHelper.ExecuteNonQuery(connStr, "create database " + databaseName);
        }

        public static void createQuestion(Question question)
        {
            if (CONNECTION_STRING == null) return;
            string sql = "insert into tabquestion(content,a,b,c,d,e,aNum,bNum,cNum,dNum,eNum) values('" + question.Content + "','" +
                question.ChoiceA + "','" + question.ChoiceB + "','" + question.ChoiceC + "','" + question.ChoiceD + "','" + question.ChoiceE + "',0,0,0,0,0)";
            MySqlHelper.ExecuteNonQuery(CONNECTION_STRING, sql);
        }

        public static void insertQuestion(int id)
        {
            if (CONNECTION_STRING == null) return;
            string sql = "insert into question(id,aNum,bNum,cNum,dNum,eNum) values(" + id + ",0,0,0,0,0)";
            MySqlHelper.ExecuteNonQuery(CONNECTION_STRING, sql);
        }

        public static void updateQuestionContent(Question question)
        {
            if (CONNECTION_STRING == null) return;
            string sql = "update tabquestion set content='" + question.Content + "',a='" + question.ChoiceA + "',b='" +
                question.ChoiceB + "',c='" + question.ChoiceC + "',d='" + question.ChoiceD + "',e='" +
                question.ChoiceE + "' where id = " + question.Id;
            MySqlHelper.ExecuteNonQuery(CONNECTION_STRING, sql);
        }
        public static void updateQuestionCount(Question question)
        {
            if (CONNECTION_STRING == null) return;
            string sql = "update tabquestion set aNum=" + question.ChoiceACount + ",bNum=" + question.ChoiceBCount + ",cNum=" +
                question.ChoiceCCount + ",dNum=" + question.ChoiceDCount + ",eNum=" + question.ChoiceECount + " where id = " + question.Id;
            MySqlHelper.ExecuteNonQuery(CONNECTION_STRING, sql);
        }

        public static void deleteQuestion(int id)
        {

            if (CONNECTION_STRING == null) return;

            string sql = "delete from tabquestion where id=" + id;
            MySqlHelper.ExecuteNonQuery(CONNECTION_STRING, sql);
        }
        /// <summary>
        /// 取得所有的问题
        /// </summary>
        /// <returns></returns>
        public static List<Question> getAllQuestions()
        {
            List<Question> questiones = new List<Question>();
            if (CONNECTION_STRING == null) return questiones;
            string sql = "select id,content,a,b,c,d,e,aNum,bNum,cNum,dNum,eNum from tabquestion";
            
            DataSet ds = MySqlHelper.ExecuteDataset(CONNECTION_STRING, sql);
            if (ds.Tables != null && ds.Tables.Count >= 1)
            {
                DataTable dt = ds.Tables[0];

                foreach (DataRow row in dt.Rows)
                {
                    questiones.Add(new Question()
                    {
                        Id = int.Parse(row["id"].ToString()),
                        Content = row["content"].ToString(),
                        ChoiceA = row["a"].ToString(),
                        ChoiceB = row["b"].ToString(),
                        ChoiceC = row["c"].ToString(),
                        ChoiceD = row["d"].ToString(),
                        ChoiceE = row["e"].ToString(),
                        ChoiceACount = int.Parse(row["aNum"].ToString().Equals("") ? "0" : row["aNum"].ToString()),
                        ChoiceBCount = int.Parse(row["bNum"].ToString().Equals("") ? "0" : row["bNum"].ToString()),
                        ChoiceCCount = int.Parse(row["cNum"].ToString().Equals("") ? "0" : row["cNum"].ToString()),
                        ChoiceDCount = int.Parse(row["dNum"].ToString().Equals("") ? "0" : row["dNum"].ToString()),
                        ChoiceECount = int.Parse(row["eNum"].ToString().Equals("") ? "0" : row["eNum"].ToString())
                    });
                }
            }

            return questiones;
        }
        /// <summary>
        /// 保存报名信息
        /// </summary>
        /// <param name="p"></param>
        /// <param name="salePromotion"></param>
        public static void saveAssignActivitiesInfo(string p, SalePromotion salePromotion)
        {
            if (CONNECTION_STRING == null) return;
            string sql = "insert into tabassignactivity(mobileNum,activityName,activityId) values('" + p + "','" + salePromotion.Name + "','" +
                salePromotion.Id + "')";
            MySqlHelper.ExecuteNonQuery(CONNECTION_STRING, sql);
        }


        /// <summary>
        /// 统计活动的报名人数
        /// </summary>
        /// <param name="currentSalePromotion"></param>
        /// <returns></returns>
        public static int getSignerNumOfActivity(SalePromotion currentSalePromotion)
        {
            if (CONNECTION_STRING == null) return 0;
            string sql = "select count(distinct mobileNum) from tabassignactivity where activityId=" + currentSalePromotion.Id;
            DataSet ds = MySqlHelper.ExecuteDataset(CONNECTION_STRING, sql);
            if (ds.Tables != null && ds.Tables.Count >= 1)
            {
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count == 1)
                {
                    return int.Parse(dt.Rows[0][0].ToString());
                }
            }
            return 0;
        }

    }
}
