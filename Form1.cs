using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace ADO.Net_baseUsers
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// НЕ ЗАБУДТЕ ИЗМЕНИТЬ Connection.congig при запуске на своей машине
        /// </summary>
        private ConnectionStringSettings connectStr;

        public Form1()
        {
            InitializeComponent();
            connectStr = System.Configuration.ConfigurationManager.ConnectionStrings["SQL"];
            Getlist();
            
        }
        private void Getlist()
        {
            
            listBox1.Items.Clear();
            using (SqlConnection connect = new SqlConnection(connectStr.ConnectionString))
            {
                SqlCommand cmd = connect.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AllUsers";
                connect.Open();
                SqlDataReader read= cmd.ExecuteReader();
                while (read.Read())
                {
                    listBox1.Items.Add(read.GetString(read.GetOrdinal("Login")));
                }
            }
        }

        private void AddUser_Click(object sender, EventArgs e)
        {
            AddUser add = new AddUser(Users());
            if (DialogResult.OK == add.ShowDialog())
            {
                InsertUser(add.dataUser);
            }
            
        }

        private void InsertUser(DataUser user)
        {

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "InsertUser";

                SqlParameter LoginParam = cmd.CreateParameter();
                LoginParam.ParameterName = "@login";
                LoginParam.DbType = DbType.String;
                LoginParam.Direction = ParameterDirection.Input;
                LoginParam.Value = user.log;

                SqlParameter AddressParam = cmd.CreateParameter();
                AddressParam.ParameterName = "@address";
                AddressParam.DbType = DbType.String;
                AddressParam.Direction = ParameterDirection.Input;
                AddressParam.Value = user.address;

                SqlParameter PasswordParam = cmd.CreateParameter();
                PasswordParam.ParameterName = "@password";
                PasswordParam.DbType = DbType.Int32;
                PasswordParam.Direction = ParameterDirection.Input;
                PasswordParam.Value = user.password;

                SqlParameter PhoneParam = cmd.CreateParameter();
                PhoneParam.ParameterName = "@phone";
                PhoneParam.DbType = DbType.Int32;
                PhoneParam.Direction = ParameterDirection.Input;
                PhoneParam.Value = user.phone;

                SqlParameter AdminParam = cmd.CreateParameter();
                AdminParam.ParameterName = "@admin";
                AdminParam.DbType = DbType.Boolean;
                AdminParam.Direction = ParameterDirection.Input;
                AdminParam.Value = user.admin;

                cmd.Parameters.Add(LoginParam);
                cmd.Parameters.Add(PasswordParam);
                cmd.Parameters.Add(AddressParam);
                cmd.Parameters.Add(PhoneParam);
                cmd.Parameters.Add(AdminParam);

                Connection(cmd);    
        }
        
        private string[] Users()
        {
            string[] users = new string[listBox1.Items.Count];
            int i = 0;
            foreach(var user in listBox1.Items)
            {
                users[i++] = user.ToString();
            }
            return users;
        }

        private void DeleteUser_Click(object sender, EventArgs e)
        {
            string user = listBox1.SelectedItem.ToString();
            DeleteUser(user);
        }

        private void DeleteUser(string user)
        {
            if (user != null)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeletetUser";

                SqlParameter LoginParam = cmd.CreateParameter();
                LoginParam.ParameterName = "@login";
                LoginParam.DbType = DbType.String;
                LoginParam.Direction = ParameterDirection.Input;
                LoginParam.Value = user;
                cmd.Parameters.Add(LoginParam);

                Connection(cmd);
            }
        }
        private void Connection(SqlCommand cmd)
        {
            using (SqlConnection connect = new SqlConnection(connectStr.ConnectionString))
            {
                cmd.Connection = connect;
                connect.Open();
                int i = cmd.ExecuteNonQuery();
                MessageBox.Show("rows affected " + i);
                Getlist();
            }
        }

        private void EditUser_doubleClick(object sender, MouseEventArgs e)
        {
            string user = listBox1.SelectedItem.ToString();
            if (user != null)
            {
                SqlCommand cmd = new SqlCommand();

                //////////////////////////////////////
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetUser";

                SqlParameter LoginParam = cmd.CreateParameter();
                LoginParam.DbType = DbType.String;
                LoginParam.Direction = ParameterDirection.Input;
                LoginParam.ParameterName = "@login";
                LoginParam.Value = user;
                cmd.Parameters.Add(LoginParam);

                DataUser User = new DataUser();

                using (SqlConnection connect = new SqlConnection(connectStr.ConnectionString))
                {
                    cmd.Connection = connect;
                    connect.Open();
                    SqlDataReader read=cmd.ExecuteReader();
                    //чтобы получить данные, их нужно сперва прочесть, а потом ковыряться
                    while (read.Read())
                    {   //////////////////////////////////
                        //В каком фармате данные в базе, в том виде и извлекаем, иначе ругается
                        //////////////////////////////////
                        int i = read.GetOrdinal("Login");
                        User.log = (string)read[i];
                        User.address = read.GetString(read.GetOrdinal("Address"));
                        User.password = read.GetInt32(read.GetOrdinal("Password"));
                        User.phone = read.GetInt32(read.GetOrdinal("Phone"));
                        User.admin = read.GetBoolean(read.GetOrdinal("Admin"));
                    }
                }

                AddUser Edit = new AddUser(User,Users());
                if (DialogResult.OK == Edit.ShowDialog())
                {
                    EditUser(User,Edit.dataUser);
                }
            }
        }
        private void EditUser(DataUser oldUser,DataUser newUser)
        {
            if (oldUser != newUser)
            {
                DeleteUser(oldUser.log);
                InsertUser(newUser);

            }
        }
    }
}
