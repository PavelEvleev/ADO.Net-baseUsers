using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ADO.Net_baseUsers
{
    public partial class AddUser : Form
    {
        public DataUser dataUser=new DataUser();
        private string[] Users;
        public AddUser()
        {
            InitializeComponent();
        }
        public AddUser(string[] users):this()
        {
            Users = users;
        }
        public AddUser(DataUser user) : this()
        {
            dataUser = new DataUser(user);
            textBox1.Text = user.log;
            textBox2.Text = user.password.ToString();
            textBox3.Text = user.address;
            textBox4.Text = user.phone.ToString();
            if (user.admin) { checkBox1.Checked = true; checkBox2.Checked = false; } else { checkBox2.Checked = true; checkBox1.Checked = false; }
        }
        public AddUser(DataUser Nuser, string[] users) : this(Nuser)
        {
            //чистим список пользователей, чтобы можно было повторно исп логин, но и для того чтобы логины не повторялись
            List<string>us = users.ToList<string>();
            us.Remove(Nuser.log);
            Users=us.ToArray();
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (Fill())
            {
                if (!existUser())
                {
                    dataUser.log = textBox1.Text;

                    dataUser.address = textBox3.Text;
                    dataUser.password = textBox2.Text.GetHashCode();
                    try
                    {
                        dataUser.phone = int.Parse(textBox4.Text);
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message,"Телефон");
                        return;
                    }
                    if(checkBox1.Checked){ dataUser.admin = true; } else { dataUser.admin = false; }
                    DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Данный логин занят, выберите другой.");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Поля должны быть заполнены!");
                return;
            }
        }
        private bool existUser()
        {
            if (Users != null)
            {
                return Users.Contains<string>(textBox1.Text);
            }
            else return false;
        }
        private bool Fill()
        {
            if (textBox1.Text != null && textBox2.Text != null && textBox3.Text != null && textBox4.Text != null && (checkBox1.Checked || checkBox2.Checked))
                return true;
            return false;
        }
    }
}
