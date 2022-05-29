//using System.Data.SqlClient;
//
//namespace TeamContactTelegramBot.CoreSystem
//{
//    class DataBase
//    {
//        SqlConnection sql = new SqlConnection(@"Data Sourse=DESKTOP-N0A0TTI\SQLEXPRESS;Initial Catalog=TeamContact_001;Integrated Security=True");
//
//        public void openConnection()
//        {
//            if (sql.State == System.Data.ConnectionState.Closed)
//                sql.Open();
//        }
//
//        public void closeConnection()
//        {
//            if (sql.State == System.Data.ConnectionState.Open)
//                sql.Close();
//        }
//
//        public SqlConnection getConnection()
//        {
//            return sql;
//        }
//    }
//}
