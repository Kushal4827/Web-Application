using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Configuration;
using System.Threading;
using log4net;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace VDEM.Lib.Database
{
    public class DBConnection
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DBConnection));
        private SqlConnection connection;
        private SqlTransaction tr = null;
        private static DBConnection myInstance;

        public bool initialization;
        public bool busy;
        public String strErrorMessage;

        //Constructor
        public DBConnection()
        {
            initialization = false;
            busy = false;
            strErrorMessage = "";
            string connectionString = ConfigurationManager.ConnectionStrings["VDEM"].ConnectionString;
            Initialize(connectionString);
        }

        //Constructor
        public DBConnection(String strConnectionString)
        {
            strErrorMessage = "";
            Initialize(strConnectionString);
        }

        public static DBConnection getInstance(String ConnectionString)
        {
            if (myInstance == null)
            {
                myInstance = new DBConnection(ConnectionString);
            }
            return myInstance;
        }

        //Code for private Funcitons......
        private void Initialize(string connectionString)
        {
            try
            {
                connection = new SqlConnection(connectionString);
            }
            catch (Exception ex)
            {
                strErrorMessage = ex.Message;
                connection = null;
                initialization = false;
                log.Error("DBConnection.Initialize() " + strErrorMessage);
                return;
            }
            initialization = true;
        }

        public bool OpenConnection(ref String strErrorMessage)
        {
            try
            {
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                }
            }
            catch (SqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        strErrorMessage = "Cannot connect to server.  Contact administrator";
                        break;
                    case 1045:
                        strErrorMessage = "Invalid username/password, please try again";
                        break;
                    default:
                        strErrorMessage = ex.Message;
                        break;
                }
                log.Error("DBConnection.OpenConnection() " + strErrorMessage);

                return false;
            }
            strErrorMessage = "";
            return true;
        }

        public bool CloseConnection(ref String strErrorMessage)
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (SqlException ex)
            {
                strErrorMessage = ex.Message;
                log.Error("DBConnection.OpenConnection() " + strErrorMessage);
                return false;
            }
        }

        public void BeginTransaction()
        {
            tr = connection.BeginTransaction();
        }

        public void CommitTransaction()
        {
            if (tr != null)
            {
                tr.Commit();
            }
        }
        public void RollBackTransaction()
        {
            if (tr != null)
            {
                tr.Rollback();
            }
        }

        private void TrimAndOr(String strAndOr, ref String strQueryCondition)
        {
            if (strAndOr == "or")
            {
                strQueryCondition = strQueryCondition.TrimEnd(' ');
                strQueryCondition = strQueryCondition.TrimEnd('r');
                strQueryCondition = strQueryCondition.TrimEnd('o');
                strQueryCondition = strQueryCondition.TrimEnd(' ');
            }

            if (strAndOr == "and")
            {
                strQueryCondition = strQueryCondition.TrimEnd(' ');
                strQueryCondition = strQueryCondition.TrimEnd('d');
                strQueryCondition = strQueryCondition.TrimEnd('n');
                strQueryCondition = strQueryCondition.TrimEnd('a');
                strQueryCondition = strQueryCondition.TrimEnd(' ');
            }
        }

        private int Insert<T>(ref String strErrorMessage, ref T dataRow)
        {
            Type mTableName = typeof(T);
            String strTableName = mTableName.Name;  //mTableName.GetType().Name;
            Dictionary<String, int> DictSyncColumns = new Dictionary<String, int>();
            String strCommandText = "INSERT INTO " + strTableName + " VALUES(";

            //strings used to createLogQuery
            String strCommandTextForLog = " VALUES(";
            String filePathForMasterSync = String.Empty;
            String createdByForSync = String.Empty;
            String userPlantCode = String.Empty;
            List<string> syncColumns = new List<string>();


            MemberInfo[] arrMemberInfo;
            arrMemberInfo = mTableName.GetMembers();

            // Prepare Command Text names
            foreach (MemberInfo propertyInfo in arrMemberInfo)
            {
                //Get Only Public Property;
                if (propertyInfo.MemberType == MemberTypes.Property)
                {
                    try
                    {
                        String strColumnName = propertyInfo.Name;
                        if (strColumnName == "ID")
                        {
                            if (DictSyncColumns.ContainsKey(strColumnName))
                            {
                                strCommandTextForLog = strCommandTextForLog + "@" + strColumnName + ", ";
                                syncColumns.Add(strColumnName);
                            }
                            continue;
                        }
                        strCommandText = strCommandText + "@" + strColumnName + ", ";
                        if (DictSyncColumns.ContainsKey(strColumnName))
                        {
                            strCommandTextForLog = strCommandTextForLog + "@" + strColumnName + ", ";
                            syncColumns.Add(strColumnName);
                        }

                    }
                    catch (Exception ex)
                    {
                        strErrorMessage = ex.Message;
                        log.Error("DBConnection.Insert() " + strCommandText);
                        log.Error("DBConnection.Insert() " + strErrorMessage);
                        return -1;
                    }
                }
            }

            strCommandText = strCommandText.TrimEnd(' ');
            strCommandText = strCommandText.TrimEnd(',');
            strCommandText = strCommandText + ");";

            strCommandText = strCommandText + " ;SELECT @@IDENTITY;";
            SqlCommand cmd = new SqlCommand(strCommandText);
            cmd.Connection = connection;

            strCommandTextForLog = strCommandTextForLog.TrimEnd(' ');
            strCommandTextForLog = strCommandTextForLog.TrimEnd(',');
            strCommandTextForLog = strCommandTextForLog + ");";
            strCommandTextForLog = "INSERT INTO " + strTableName + "(" + string.Join(",", syncColumns) + ")" + strCommandTextForLog;

            SqlCommand cmdForLog = new SqlCommand(strCommandTextForLog);
            cmdForLog.Connection = connection;

            // AssignValues names
            foreach (MemberInfo propertyInfo in arrMemberInfo)
            {
                //Get Only Public Property;
                if (propertyInfo.MemberType == MemberTypes.Property)
                {
                    try
                    {
                        String strColumnName = propertyInfo.Name;
                        if (strColumnName == "PK_ID")
                        {
                            continue;
                        }

                        var property = dataRow.GetType().GetProperty(strColumnName);
                        var value = property.GetValue(dataRow);
                        if (value == null) 
                        {
                            value = DBNull.Value;
                        }
                        cmd.Parameters.AddWithValue("@" + strColumnName, value);
                        if (DictSyncColumns.ContainsKey(strColumnName))
                        {
                            cmdForLog.Parameters.AddWithValue("@" + strColumnName, value);
                            if (DictSyncColumns[strColumnName] == 1)
                                filePathForMasterSync = value.ToString();
                        }
                        if (strColumnName == "CREATED_BY")
                        {
                            createdByForSync = value.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        strErrorMessage = ex.Message;
                        log.Error("DBConnection.Insert() " + strCommandText);
                        log.Error("DBConnection.Insert() " + strErrorMessage);
                        return -1;
                    }
                }
            }

            int rc = -1;
            decimal Newvalue;
            try
            {
                //rc = cmd.ExecuteNonQuery();
                Newvalue = (decimal)cmd.ExecuteScalar();
                rc = 0;
            }
            catch (Exception ex)
            {
                strErrorMessage = ex.Message;
                return -1;
            }

            //String strWhereCondition = " ORDER BY PK_ID DESC";
            String strWhereCondition = " WHERE ID = " + Newvalue;
            rc = Select_ForSave(ref strErrorMessage, strWhereCondition, ref dataRow);

            return rc;
        }

        private int Update<T>(ref String strErrorMessage, ref T dataRow)
        {
            Type mTableName = typeof(T);
            String strTableName = mTableName.Name;  //mTableName.GetType().Name;
            String strCommandText = "UPDATE " + strTableName + " SET ";
            String strCommandTextForLog = "UPDATE " + strTableName + " SET ";
            Dictionary<String, int> DictSyncColumns = new Dictionary<String, int>();
            int InsertFlag = 0;
            String filePathForMasterSync = String.Empty;
            String createdByForSync = String.Empty;
            String userPlantCode = String.Empty;


            MemberInfo[] arrMemberInfo;
            arrMemberInfo = dataRow.GetType().GetMembers();

            // Prepare Command Text names
            foreach (MemberInfo propertyInfo in arrMemberInfo)
            {
                //Get Only Public Property;
                if (propertyInfo.MemberType == MemberTypes.Property)
                {
                    try
                    {
                        String strColumnName = propertyInfo.Name;
                        if (strColumnName != "PK_ID")
                        {
                            strCommandText = strCommandText + strColumnName + " = @" + strColumnName + ", ";
                            if (DictSyncColumns.ContainsKey(strColumnName))
                                strCommandTextForLog = strCommandTextForLog + strColumnName + " = @" + strColumnName + ", ";
                            var property = dataRow.GetType().GetProperty(strColumnName);
                        }
                    }
                    catch (Exception ex)
                    {
                        strErrorMessage = ex.Message;
                        log.Error("DBConnection.Update() " + strCommandText);
                        log.Error("DBConnection.Update() " + strErrorMessage);
                        return -1;
                    }
                }
            }

            strCommandText = strCommandText.TrimEnd(' ');
            strCommandText = strCommandText.TrimEnd(',');
            strCommandText = strCommandText + " WHERE PK_ID = @PK_ID;";

            strCommandTextForLog = strCommandTextForLog.TrimEnd(' ');
            strCommandTextForLog = strCommandTextForLog.TrimEnd(',');
            strCommandTextForLog = strCommandTextForLog + " WHERE PK_ID = @PK_ID";



            SqlCommand cmd = new SqlCommand(strCommandText);
            cmd.Connection = connection;

            SqlCommand cmdForLog = new SqlCommand(strCommandTextForLog);
            cmdForLog.Connection = connection;

            // AssignValues names
            foreach (MemberInfo propertyInfo in arrMemberInfo)
            {
                //Get Only Public Property;
                if (propertyInfo.MemberType == MemberTypes.Property)
                {
                    try
                    {
                        String strColumnName = propertyInfo.Name;
                        var property = dataRow.GetType().GetProperty(strColumnName);
                        var value = property.GetValue(dataRow);
                        if (value == null)
                        {
                            value = DBNull.Value;
                        }
                        cmd.Parameters.AddWithValue("@" + strColumnName, value);
                        if (DictSyncColumns.ContainsKey(strColumnName))
                        {
                            cmdForLog.Parameters.AddWithValue("@" + strColumnName, value);
                            if (DictSyncColumns[strColumnName] == 1)
                                filePathForMasterSync = value.ToString();
                        }
                        if (strColumnName == "CREATED_BY")
                        {
                            createdByForSync = value.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        strErrorMessage = ex.Message;
                        log.Error("DBConnection.Update() " + strCommandText);
                        log.Error("DBConnection.Update() " + strErrorMessage);

                        return -1;
                    }
                }
            }
            int rc;
            try
            {
                rc = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                strErrorMessage = ex.Message;
                log.Error("DBConnection.Update() " + strCommandText);
                log.Error("DBConnection.Update() " + strErrorMessage);
                return -1;
            }
            return rc;
        }

        private int Update<T>(ref String strErrorMessage, ref T dataRow, ref List<String> mlColumns)
        {
            Type mTableName = typeof(T);
            String strTableName = mTableName.Name;  //mTableName.GetType().Name;
            String strCommandText = "UPDATE " + strTableName + " SET ";

            MemberInfo[] arrMemberInfo;
            arrMemberInfo = dataRow.GetType().GetMembers();

            // Prepare Command Text names
            foreach (MemberInfo propertyInfo in arrMemberInfo)
            {
                //Get Only Public Property;
                if (propertyInfo.MemberType == MemberTypes.Property)
                {
                    try
                    {
                        String strColumnName = propertyInfo.Name;
                        if (strColumnName != "PK_ID")
                        {
                            for (int i = 0; i < mlColumns.Count; i++)
                            {
                                if (mlColumns[i] == strColumnName)
                                {
                                    strCommandText = strCommandText + strColumnName + " = @" + strColumnName + ", ";
                                    break;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        strErrorMessage = ex.Message;
                        log.Error("DBConnection.Update() " + strCommandText);
                        log.Error("DBConnection.Update() " + strErrorMessage);

                        return -1;
                    }
                }
            }

            strCommandText = strCommandText.TrimEnd(' ');
            strCommandText = strCommandText.TrimEnd(',');
            strCommandText = strCommandText + " WHERE PK_ID = @PK_ID;";



            SqlCommand cmd = new SqlCommand(strCommandText);
            cmd.Connection = connection;
            cmd.CommandTimeout = 60; //60 Seconds.
            // AssignValues names
            foreach (MemberInfo propertyInfo in arrMemberInfo)
            {
                //Get Only Public Property;
                if (propertyInfo.MemberType == MemberTypes.Property)
                {
                    try
                    {
                        mlColumns.Add("PK_ID");
                        for (int i = 0; i < mlColumns.Count; i++)
                        {
                            String strColumnName = propertyInfo.Name;
                            if (mlColumns[i] == strColumnName)
                            {
                                var property = dataRow.GetType().GetProperty(strColumnName);
                                var value = property.GetValue(dataRow);
                                cmd.Parameters.AddWithValue("@" + strColumnName, value);
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        strErrorMessage = ex.Message;
                        log.Error("DBConnection.Update() " + strCommandText);
                        log.Error("DBConnection.Update() " + strErrorMessage);

                        return -1;
                    }
                }
            }

            int rc;
            try
            {
                rc = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                strErrorMessage = ex.Message;
                log.Error("DBConnection.Update() " + strCommandText);
                log.Error("DBConnection.Update() " + strErrorMessage);

                return -1;
            }

            return rc;
        }


        //Code for public Funcitons......

        public String SearchAllColumns<T>(String strAndOr, ref T dataRow, String searchText)
        {
            String strQueryCondition = "WHERE";

            // get all public static properties of MyClass type
            MemberInfo[] arrMemberInfo;
            arrMemberInfo = dataRow.GetType().GetMembers();

            // write property names
            foreach (MemberInfo propertyInfo in arrMemberInfo)
            {
                //Get Only Public Property;
                if (propertyInfo.MemberType == MemberTypes.Property)
                {
                    String strColumnName = propertyInfo.Name;
                    strQueryCondition = strQueryCondition + " " + strColumnName + " like '%" + searchText + "%' " + strAndOr + " ";
                }
            }

            TrimAndOr(strAndOr, ref strQueryCondition);
            return strQueryCondition;
        }

        public String SearchInColumns(String strAndOr, ref List<String> mlColumns, String searchText)
        {
            String strQueryCondition = "WHERE";

            for (int i = 0; i < mlColumns.Count; i++)
            {
                strQueryCondition = strQueryCondition + " " + mlColumns[i] + " like '%" + searchText + "%' " + strAndOr + " ";
            }

            TrimAndOr(strAndOr, ref strQueryCondition);

            return strQueryCondition;
        }

        public String PIHMS_Where(String strWhereCondition)
        {
            //String strWhereCondition_New;
            //strWhereCondition_New = strWhereCondition.Replace("WHERE ", "WHERE IsRowDeleted = 'N' and ");
            //return strWhereCondition_New;

            return strWhereCondition;
        }


        private int Select_ForSave<T>(ref String strErrorMessage, String strWhereCondition, ref T dataRow)
        {
            Type mTableName = typeof(T);

            String strTableName = mTableName.Name;  //mTableName.GetType().Name;


            strWhereCondition = PIHMS_Where(strWhereCondition);
            String strQuery = "SELECT TOP 1 * FROM " + strTableName + " WITH (NOLOCK) " + strWhereCondition;

            //Open connection
            if (this.OpenConnection(ref strErrorMessage) == true)
            {
                //Create Command
                SqlCommand cmd = new SqlCommand(strQuery, connection);
                //Create a data reader and Execute the command
                SqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    if (dataReader.HasRows)
                    {

                        // get all public static properties of MyClass type
                        MemberInfo[] arrMemberInfo;
                        arrMemberInfo = dataRow.GetType().GetMembers();

                        // write property names
                        foreach (MemberInfo propertyInfo in arrMemberInfo)
                        {
                            //Get Only Public Property;
                            if (propertyInfo.MemberType == MemberTypes.Property)
                            {
                                try
                                {
                                    String strColumnName = propertyInfo.Name;
                                    var property = dataRow.GetType().GetProperty(strColumnName);

                                    object value = dataReader[strColumnName];
                                    if (value == DBNull.Value)
                                        value = null;

                                    property.SetValue(dataRow, value, null);

                                }
                                catch (Exception ex)
                                {
                                    dataReader.Close();
                                    strErrorMessage = ex.Message;
                                    log.Error("DBConnection.Select_ForSave() " + strQuery);
                                    log.Error("DBConnection.Select_ForSave() " + strErrorMessage);
                                    return -1;
                                }
                            }
                        }

                        break;
                    }
                }

                //close Data Reader
                dataReader.Close();
                return 1;
            }
            else
            {
                return -2;
            }
        }

        public int Select<T>(ref String strErrorMessage, String strWhereCondition, ref T dataRow)
        {
            Type mTableName = typeof(T);

            String strTableName = mTableName.Name;  //mTableName.GetType().Name;


            strWhereCondition = PIHMS_Where(strWhereCondition);
            String strQuery = "SELECT * FROM " + strTableName + " WITH (NOLOCK) " + strWhereCondition;

            //Open connection
            if (this.OpenConnection(ref strErrorMessage) == true)
            {
                //Create Command
                SqlCommand cmd = new SqlCommand(strQuery, connection);
                //Create a data reader and Execute the command
                SqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    if (dataReader.HasRows)
                    {

                        // get all public static properties of MyClass type
                        MemberInfo[] arrMemberInfo;
                        arrMemberInfo = dataRow.GetType().GetMembers();

                        // write property names
                        foreach (MemberInfo propertyInfo in arrMemberInfo)
                        {
                            //Get Only Public Property;
                            if (propertyInfo.MemberType == MemberTypes.Property)
                            {
                                try
                                {
                                    String strColumnName = propertyInfo.Name;
                                    var property = dataRow.GetType().GetProperty(strColumnName);

                                    object value = dataReader[strColumnName];
                                    if (value == DBNull.Value)
                                        value = null;

                                    property.SetValue(dataRow, value, null);

                                }
                                catch (Exception ex)
                                {
                                    strErrorMessage = ex.Message;
                                    log.Error("DBConnection.Select() " + strQuery);
                                    log.Error("DBConnection.Select() " + strErrorMessage);
                                    return -1;
                                }
                            }
                        }

                        break;
                    }
                }

                //close Data Reader
                dataReader.Close();
                // CloseConnection(ref strErrorMessage);
                return 1;
            }
            else
            {
                return -2;
            }

            // CloseConnection(ref strErrorMessage);
        }

        public int Select<T>(ref String strErrorMessage, String strWhereCondition, ref List<T> mlRows)
        {
            Type mTableName = typeof(T);

            String strTableName = mTableName.Name;  //mTableName.GetType().Name;

            strWhereCondition = PIHMS_Where(strWhereCondition);
            String strQuery = "SELECT * FROM " + strTableName + " WITH (NOLOCK) " + strWhereCondition;

            //Open connection
            try
            {
                if (this.OpenConnection(ref strErrorMessage) == true)
                {
                    //Create Command
                    SqlCommand cmd = new SqlCommand(strQuery, connection);
                    //Create a data reader and Execute the command
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                    {
                        if (dataReader.HasRows)
                        {
                            //Create New Instace of the Table Row Object
                            T T_Object = Activator.CreateInstance<T>();

                            // get all public static properties of MyClass type
                            MemberInfo[] arrMemberInfo;
                            arrMemberInfo = T_Object.GetType().GetMembers();

                            // write property names
                            foreach (MemberInfo propertyInfo in arrMemberInfo)
                            {
                                //Get Only Public Property;
                                if (propertyInfo.MemberType == MemberTypes.Property)
                                {
                                    try
                                    {
                                        String strColumnName = propertyInfo.Name;
                                        var property = T_Object.GetType().GetProperty(strColumnName);

                                        object value = dataReader[strColumnName];
                                        if (value == DBNull.Value)
                                            value = null;

                                        property.SetValue(T_Object, value, null);
                                    }
                                    catch (Exception ex)
                                    {
                                        strErrorMessage = ex.Message;
                                        log.Error("DBConnection.Select() " + strQuery);
                                        log.Error("DBConnection.Select() " + strErrorMessage);
                                        return -1;
                                    }
                                }
                            }

                            //Adding the Read Row to the List
                            mlRows.Add(T_Object);
                        }
                    }

                    //close Data Reader
                    dataReader.Close();
                    // CloseConnection(ref strErrorMessage);
                    return mlRows.Count();
                }
                else
                {
                    return -2;
                }

            }
            catch (Exception ex)
            {
                strErrorMessage = ex.Message;
                log.Error("DBConnection.Select() " + strQuery);
                log.Error("DBConnection.Select() " + strErrorMessage);
                return -1;
            }

            // CloseConnection(ref strErrorMessage);
            return 0;
        }

        public int Select<T>(ref String strErrorMessage, ref List<String> mlColumns, String strWhereCondition, ref List<T> mlRows)
        {
            Type mTableName = typeof(T);

            String strTableName = mTableName.Name;  //mTableName.GetType().Name;

            strWhereCondition = PIHMS_Where(strWhereCondition);
            String strQuery = "SELECT";
            for (int i = 0; i < mlColumns.Count; i++)
            {
                strQuery = strQuery + " " + mlColumns[i] + ",";
            }
            strQuery = strQuery.TrimEnd(',');
            strQuery = strQuery + " FROM " + strTableName + " WITH (NOLOCK) " + strWhereCondition;

            //Open connection
            if (this.OpenConnection(ref strErrorMessage) == true)
            {
                //Create Command
                SqlCommand cmd = new SqlCommand(strQuery, connection);
                //Create a data reader and Execute the command
                SqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    if (dataReader.HasRows)
                    {
                        //Create New Instace of the Table Row Object
                        T T_Object = Activator.CreateInstance<T>();

                        // get only Specified columns
                        for (int i = 0; i < mlColumns.Count; i++)
                        {
                            try
                            {
                                String strColumnName = mlColumns[i];
                                var property = T_Object.GetType().GetProperty(strColumnName);

                                object value = dataReader[strColumnName];
                                if (value == DBNull.Value)
                                    value = null;

                                property.SetValue(T_Object, value, null);
                            }
                            catch (Exception ex)
                            {
                                // CloseConnection(ref strErrorMessage);
                                strErrorMessage = ex.Message;
                                log.Error("DBConnection.Select() " + strQuery);
                                log.Error("DBConnection.Select() " + strErrorMessage);

                                return -1;
                            }
                        }

                        //Adding the Read Row to the List
                        mlRows.Add(T_Object);
                    }
                }

                //close Data Reader
                dataReader.Close();

                // CloseConnection(ref strErrorMessage);
                return mlRows.Count();
            }
            else
            {
                // CloseConnection(ref strErrorMessage);
                return -2;
            }

            // CloseConnection(ref strErrorMessage);
            return 0;
        }

        public int SelectScalar(ref String strErrorMessage, String strQuery, ref object res)
        {
            if (this.OpenConnection(ref strErrorMessage) == true)
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(strQuery, connection);
                    res = cmd.ExecuteScalar();
                    // CloseConnection(ref strErrorMessage);
                    return 1;
                }
                catch (Exception ex)
                {
                    strErrorMessage = ex.Message;
                    log.Error("DBConnection.SelectScalar() " + strQuery);
                    log.Error("DBConnection.SelectScalar() " + strErrorMessage);
                    return -1;
                }
            }
            else
            {
                return -2;
            }

            // CloseConnection(ref strErrorMessage);
            return 0;
        }

        public int Save<T>(ref String strErrorMessage, ref T m_Row)
        {
            //Open connection


            if (this.OpenConnection(ref strErrorMessage) == false)
            {
                return -1;
            }

            T t1 = m_Row;
            var property = t1.GetType().GetProperty("PK_ID");
            decimal value = (decimal)property.GetValue(t1);
            if (value == 0)
            {
                if (Insert(ref strErrorMessage, ref t1) < 0)
                {
                    // this.CloseConnection(ref strErrorMessage);
                    return -1;
                }
            }
            else
            {
                if (Update(ref strErrorMessage, ref t1) < 0)
                {
                    // this.CloseConnection(ref strErrorMessage);
                    return -1;
                }
            }

            //Just Give 100 Milisecond sleep.
            Thread.Sleep(100);

            // this.CloseConnection(ref strErrorMessage);
            return 0;
        }

        public int Save<T>(ref String strErrorMessage, ref List<T> mlRows)
        {
            //Open connection


            if (this.OpenConnection(ref strErrorMessage) == false)
            {
                return -1;
            }

            for (int i = 0; i < mlRows.Count; i++)
            {
                T t1 = mlRows[i];
                var property = t1.GetType().GetProperty("ID");
                decimal value = (decimal)property.GetValue(t1);
                if (value == 0)
                {
                    if (Insert(ref strErrorMessage, ref t1) < 0)
                    {
                        // this.CloseConnection(ref strErrorMessage);
                        return -1;
                    }
                }
                else
                {
                    if (Update(ref strErrorMessage, ref t1) < 0)
                    {
                        // this.CloseConnection(ref strErrorMessage);
                        return -1;
                    }
                }

                //Just Give 100 Milisecond sleep.
                Thread.Sleep(100);
            }

            // this.CloseConnection(ref strErrorMessage);
            return 0;
        }

        public int InsertWithPK<T>(ref String strErrorMessage, ref List<T> mlRows)
        {
            //Open connection


            if (this.OpenConnection(ref strErrorMessage) == false)
            {
                return -1;
            }

            for (int i = 0; i < mlRows.Count; i++)
            {
                T t1 = mlRows[i];
                if (Insert(ref strErrorMessage, ref t1) < 0)
                {
                    // this.CloseConnection(ref strErrorMessage);
                    return -1;
                }

                //Just Give 100 Milisecond sleep.
                Thread.Sleep(100);
            }

            // this.CloseConnection(ref strErrorMessage);
            return 0;
        }

        public int Save<T>(ref String strErrorMessage, ref List<T> mlRows, ref List<String> mlColumns)
        {
            //Open connection
            if (this.OpenConnection(ref strErrorMessage) == false)
            {
                return -1;
            }

            for (int i = 0; i < mlRows.Count; i++)
            {
                T t1 = mlRows[i];
                var property = t1.GetType().GetProperty("ID");
                long value = (long)property.GetValue(t1);
                if (value == 0)
                {
                    if (Insert(ref strErrorMessage, ref t1) < 0)
                    {
                        // this.CloseConnection(ref strErrorMessage);
                        return -1;
                    }
                }
                else
                {
                    if (Update(ref strErrorMessage, ref t1, ref mlColumns) < 0)
                    {
                        // this.CloseConnection(ref strErrorMessage);
                        return -1;
                    }
                }
            }

            // this.CloseConnection(ref strErrorMessage);
            return 0;
        }

        public int DeleteAllRows<T>(ref String strErrorMessage, ref T dr)
        {
            //Open connection
            if (this.OpenConnection(ref strErrorMessage) == false)
            {
                return -1;
            }

            Type mTableName = typeof(T);
            String strCommandText = "DELETE  FROM " + mTableName.Name;
            SqlCommand cmd = new SqlCommand(strCommandText);
            cmd.Connection = connection;
            cmd.CommandTimeout = 60; //60 Seconds.

            int rc;
            try
            {
                rc = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                strErrorMessage = ex.Message;
                log.Error("DBConnection.DeleteAllRows() " + strCommandText);
                log.Error("DBConnection.DeleteAllRows() " + strErrorMessage);
                return -1;
            }

            // this.CloseConnection(ref strErrorMessage);
            return 0;
        }

        public int DeleteAllRows(ref String strErrorMessage, String strTable)
        {
            //Open connection
            if (this.OpenConnection(ref strErrorMessage) == false)
            {
                return -1;
            }

            String strCommandText = "DELETE  FROM " + strTable;
            SqlCommand cmd = new SqlCommand(strCommandText);
            cmd.Connection = connection;
            cmd.CommandTimeout = 60; //60 Seconds.

            int rc;
            try
            {
                rc = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                strErrorMessage = ex.Message;
                log.Error("DBConnection.DeleteAllRows() " + strCommandText);
                log.Error("DBConnection.DeleteAllRows() " + strErrorMessage);

                return -1;
            }

            // this.CloseConnection(ref strErrorMessage);
            return 0;
        }

        public int DeleteAllRowsCondition(ref String strErrorMessage, String strTable, String condition)
        {
            //Open connection
            if (this.OpenConnection(ref strErrorMessage) == false)
            {
                return -1;
            }

            String strCommandText = "DELETE  FROM " + strTable + " " + condition;
            SqlCommand cmd = new SqlCommand(strCommandText);
            cmd.Connection = connection;
            cmd.CommandTimeout = 60; //60 Seconds.

            int rc;
            try
            {
                rc = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                strErrorMessage = ex.Message;
                log.Error("DBConnection.DeleteAllRowsCondition() " + strCommandText);
                log.Error("DBConnection.DeleteAllRowsCondition() " + strErrorMessage);

                return -1;
            }
            // this.CloseConnection(ref strErrorMessage);
            return 0;
        }

    }
}
