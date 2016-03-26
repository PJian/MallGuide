using EntityManagementService.entity;
using System;
using System.Collections.Generic;
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

        public static void openConnection() 
        {
            string connStr = "server=" + serverName + ";database=" + databaseName + ";uid=" + userName + ";pwd=" + password;
            conn = new SqlConnection(connStr);
            conn.Open();
        }

        public static void  executeSql(string sql)
        {
            openConnection();
            SqlCommand command = new SqlCommand(sql, conn);
            SqlDataReader datareader = command.ExecuteReader();
            if (datareader.Read())
            {
                //执行语句
                
            }
            else { 
                //执行语句

            }
            datareader.Close();
            conn.Close();
        }

        public static void insertQuestion(int id) 
        {
            string sql = "insert into question(id,aNum,bNum,cNum,dNum,eNum) values("+id+",0,0,0,0,0)";
            executeSql(sql);
        }

        public static void setQuestionCount(Question question)
        {
            string sql = "update question set aNum=" + question.ChoiceACount + ",bNum=" + question.ChoiceBCount + ",cNum=" +
                question.ChoiceCCount + ",dNum=" + question.ChoiceDCount + ",eNum=" + question.ChoiceECount + " where id = "+question.Id;
            executeSql(sql);
        }

        public static void deleteQuestion(int id) 
        {
            string sql = "delete from question where id="+id;
            executeSql(sql);
        }

        public static void insertActivity(int id) 
        {
            string sql = "insert into activity(activityID,peopleCount) values("+id+",0)";
            executeSql(sql);
        }

        public static void updateActivity(int id) 
        {
            string sql = "update activity set peopleCount=peopleCount+1 where activityID="+id;
            executeSql(sql);
        }

    }
}
