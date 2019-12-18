using System;
using MySql.Data.MySqlClient;
using System.Data;
using Renci.SshNet;
using System.Collections.Generic;

public class SSH
{
    public SshClient client;
    public string sshhost;
    public int sshport;
    public string sshuid;
    public string sshpassword;
    public int sshlocalport;
    public System.UInt32 boundport;

    public Mysql mysql = new Mysql();

    public void Initialize(String host, int port, String uid, String password, int localport)
    {
        this.sshhost = host;
        this.sshport = port;
        this.sshuid = uid;
        this.sshpassword = password;
        this.sshlocalport = localport;

        this.client = new SshClient(this.sshhost, this.sshport, this.sshuid, this.sshpassword);
    }

    public void OpenSSHConnection()
    {
        try
        {
            this.client.Connect();
        }
        catch (Exception ex)
        {
            throw (ex);
        }
    }
    public void CloseSSHConnection()
    {
        this.client.Disconnect();
    }

    public void OpenPort()
    {
        ForwardedPortLocal portfwrdl = new ForwardedPortLocal("127.0.0.1", "127.0.0.1", Convert.ToUInt32(this.sshlocalport));
        this.client.AddForwardedPort(portfwrdl);

        try
        {
            portfwrdl.Start();
            this.boundport = portfwrdl.BoundPort;
        }
        catch (Exception ex)
        {
            throw (ex);
        }
    }
}

public class Mysql
{
    public MySqlConnection mysqlconnection;
    private string sqlserver;
    private string sqlport;
    private string sqldatabase;
    private string sqluid;
    private string sqlpassword;



    public void ConnectToMySQL()
    {

    }

    //Initialize private variables and create connection string
    public void Initialize(String server, String port, String database, String uid, String password)
    {
        this.sqlserver = server;
        this.sqlport = port;
        this.sqldatabase = database;
        this.sqluid = uid;
        this.sqlpassword = password;

        string connectionString;
        //create connection string with ssl mode off
        connectionString = "SERVER=" + this.sqlserver + ";" + "PORT=" + this.sqlport + ';' + "DATABASE=" + this.sqldatabase + ";" + "UID=" + this.sqluid + ";" + "PASSWORD=" + this.sqlpassword + ";" + "SslMode=none";

        mysqlconnection = new MySqlConnection(connectionString);
    }

    //Open and Close SQL connection
    public bool OpenSQLConnection()
    {
        try
        {
            mysqlconnection.Open();
            return true;
        }
        catch (MySqlException ex)
        {
            //error code 0: connect failed to server
            //error code 1045: invalid username/password
            switch (ex.Number)
            {
                case 0:
                    Console.WriteLine("unable to connect to server");
                    break;
                case 1042:
                    Console.WriteLine("unable to connect to mysql host");
                    break;
                case 1045:
                    Console.WriteLine("Invalid username or password");
                    break;
            }
            return false;
        }
    }
    public void CloseSQLConnection()
    {
        this.mysqlconnection.Close();
    }

    //SQL Statement Generator

    //Select All From Specified Table
    public String SQLSelectAll(String tablename)
    {
        OpenSQLConnection();
        String context = "SELECT * FROM " + tablename;
        MySqlDataReader reader = null;
        MySqlCommand com = new MySqlCommand(context, this.mysqlconnection);

        reader = com.ExecuteReader();
        DataTable table = reader.GetSchemaTable();

        String re = null;

        if (table.Rows.Count == 0)
        {
            return "no rows returned";
        }
        else
        {
            Boolean flag = true;
            while (reader.Read())
            {
                //Get table colomn names
                if (flag)
                {
                    foreach (DataRow r in table.Rows)
                    {
                        re += String.Format("{0,15}", r["ColumnName"]);
                    }
                    re += '\n';
                    flag = false;
                }
                //Get and format table data
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    re += String.Format("{0,15}", reader.GetString(i));
                }
                re += "\n";
            }
        }
        reader.Close();
        CloseSQLConnection();
        return re;
    }
    //Select Single Row From User table
    //ex: SELECT * FROM User WHERE UserName LIKE "Student" AND UserPassword LIKE "password"
    //will return multiple rows if there are multiple matches
    public User SQLSelectUser(String UserName, String UserPassword)
    {
        OpenSQLConnection();
        String context = "select * from User where UserName like \"" + UserName + "\"" + " AND UserPassword like \"" + UserPassword + "\"";

        MySqlDataReader reader = null;
        MySqlCommand com = new MySqlCommand(context, this.mysqlconnection);

        reader = com.ExecuteReader();
        DataTable table = reader.GetSchemaTable();

        User u = new User();

        if (table.Rows.Count == 0)
        {
            return u;
        }
        else
        {
            while (reader.Read())
            {
                u.ID = System.Convert.ToInt32(reader.GetString(0));
                u.Name = reader.GetString(1);
                u.Password = reader.GetString(2);
                u.Email = reader.GetString(3);
                if (reader.GetString(4) == "True")
                {
                    u.Preset = 1;
                }
                else
                {
                    u.Preset = 0;
                }
            }
        }
        reader.Close();
        CloseSQLConnection();
        return u;
    }
    //select user with just user name
    //only used for password recovery
    public User SQLSelectUser(String UserName)
    {
        OpenSQLConnection();
        String context = "select * from User where UserName like \"" + UserName + "\"";

        MySqlDataReader reader = null;
        MySqlCommand com = new MySqlCommand(context, this.mysqlconnection);

        reader = com.ExecuteReader();
        DataTable table = reader.GetSchemaTable();

        User u = new User();

        if (table.Rows.Count == 0)
        {
            return u;
        }
        else
        {
            while (reader.Read())
            {
                u.ID = System.Convert.ToInt32(reader.GetString(0));
                u.Name = reader.GetString(1);
                u.Password = reader.GetString(2);
                u.Email = reader.GetString(3);
                if (reader.GetString(4) == "True")
                {
                    u.Preset = 1;
                }
                else
                {
                    u.Preset = 0;
                }
            }
        }
        reader.Close();
        CloseSQLConnection();
        return u;
    }
    //change accounts password
    //flag will be change to either 1 or 0 depending on its initial value
    public void SQLChangePassword(String UserName, String newPassword, int flag)
    {
        OpenSQLConnection();
        if (flag == 1) { flag = 0; } else { flag = 1; }
        String context = "update User Set UserPassword = \"" + newPassword + "\", PReset = " + flag + " where UserName = \"" + UserName + "\"";
        MySqlCommand com = new MySqlCommand(context, this.mysqlconnection);
        com.ExecuteReader();
        CloseSQLConnection();
    }
    //Insert User into Usertable
    //ex: INSERT INTO User (UserName, UserPassword) VALUES ("username", "Password");
    public bool SQLInsertUser(String UserName, String UserPassword, String UserEmail)
    {
        OpenSQLConnection();
        String context = "INSERT INTO User (UserName, UserPassword, UserEmail) VALUES (\"" + UserName + "\", \"" + UserPassword + "\", \"" + UserEmail + "\")";
        MySqlCommand com = new MySqlCommand(context, this.mysqlconnection);
        bool r = false;
        try
        {
            com.ExecuteNonQuery();
            r = true;
        }
        catch (MySqlException ex)
        {
            throw (ex);
        }
        CloseSQLConnection();
        return r;
    }
    //Delete user from User table
    public String SQLDeleteUser(String UserName)
    {
        OpenSQLConnection();
        String context = "DELETE FROM User WHERE UserName=\"" + UserName + "\"";
        MySqlCommand com = new MySqlCommand(context, this.mysqlconnection);

        String r = null;
        if (com.ExecuteNonQuery() == 0)
        {
            //0 rows affected sql statement was not successful                
            r = "Could not Delete User: " + UserName;
        }
        else
        {
            //successfully affected rows in database
            r = "Deleted User: " + UserName;
        }
        CloseSQLConnection();
        return r;
    }

    public String SQLInsertScore(string playerName, string minigameName, int minutes, int seconds)
    {
        OpenSQLConnection();
        String context = "INSERT INTO OnlineScore (playerName, minigameName, minutes, seconds) VALUES (\"" + playerName + "\", \"" + minigameName + "\", \"" + minutes + "\", \"" + seconds + "\")";
        MySqlCommand com = new MySqlCommand(context, this.mysqlconnection);

        String r = null;
        if (com.ExecuteNonQuery() == 0)
        {
            //0 rows affected sql statement was not successful                
            r = "Could not add score";
        }
        else
        {
            //successfully affected rows in database
            r = "Added score";
        }
        CloseSQLConnection();
        return r;
    }

    public List<Score> SQLSelectScore(string minigameName)
    {
        OpenSQLConnection();
        String context = "select * from OnlineScore where minigameName = \"" + minigameName + "\" order by minutes,seconds asc";
        //Water balloon game is opposite in time (Who ever lasted the longest)
        if(minigameName == "Water Balloon" || minigameName == "Simon Says") context = "select * from OnlineScore where minigameName = \"" + minigameName + "\" order by minutes,seconds desc";

        MySqlDataReader reader = null;
        MySqlCommand com = new MySqlCommand(context, this.mysqlconnection);

        reader = com.ExecuteReader();
        DataTable table = reader.GetSchemaTable();

        List<Score> scores = new List<Score>();

        if (table.Rows.Count == 0)
        {
            return scores;
        }
        else
        {
            while (reader.Read())
            {
                Score s = new Score();
                s.PlayerName = reader.GetString(0);
                s.MiniGameName = reader.GetString(1);
                s.Minutes = System.Convert.ToInt32(reader.GetString(2));
                s.Seconds = System.Convert.ToInt32(reader.GetString(3));
                scores.Add(s);
            }
        }
        reader.Close();
        CloseSQLConnection();
        return scores;
    }

    public List<Score> SQLSelectAllScores()
    {
        OpenSQLConnection();
        String context = "select * from OnlineScore";

        MySqlDataReader reader = null;
        MySqlCommand com = new MySqlCommand(context, this.mysqlconnection);

        reader = com.ExecuteReader();
        DataTable table = reader.GetSchemaTable();

        List<Score> scores = new List<Score>();

        if (table.Rows.Count == 0)
        {
            return scores;
        }
        else
        {
            while (reader.Read())
            {
                Score s = new Score();
                s.PlayerName = reader.GetString(0);
                s.MiniGameName = reader.GetString(1);
                s.Minutes = System.Convert.ToInt32(reader.GetString(2));
                s.Seconds = System.Convert.ToInt32(reader.GetString(3));
                scores.Add(s);
            }
        }
        reader.Close();
        CloseSQLConnection();
        return scores;
    }

}

