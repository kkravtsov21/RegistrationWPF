using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Register
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {

        public SqlConnection con;
        bool flag = true;
        public Window1()
        {
            InitializeComponent();
            try
            {
                string conStr = ConfigurationManager.ConnectionStrings["Main_db"].ConnectionString;
                con = new SqlConnection(conStr);
                con.Open();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Close();
            }
        }
        static String GetSHA_256(String str)
        {
            using (var hasher = System.Security.Cryptography.SHA256.Create())
            {

                byte[] strBytes = System.Text.Encoding.ASCII.GetBytes(str);

                byte[] hashBytes = hasher.ComputeHash(strBytes);

                var sb = new System.Text.StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }

                return sb.ToString();
            }
        }
        private void IsNameFree_Click(object sender, RoutedEventArgs e)
        {
            if (UserName.Text.Equals(string.Empty))
            {
                MessageBox.Show("Логин пуст");
                return;
            }
            var cmd = new SqlCommand(@"Select COUNT (ID)
                FROM Users WHERE Login = N'" + UserName.Text + "'", con);
            int n = -1;
            try
            {
                n = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Sql exeption:\n" + ex.Message + "\n" + cmd.CommandText);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Convert Error:\n" + ex.Message);
            }
            if (n == 0)
       MessageBox.Show("Логин свободен");
            try { n = Convert.ToInt32(cmd.ExecuteScalar()); }
            catch (Exception ex) { MessageBox.Show(ex.Message); return; }
            if (n > 0) { MessageBox.Show("Логин занят"); return; }
            cmd.CommandText = String.Format(@"INSERT INTO Users (Login, Name, PassHash, Email, Phone, RegisterDT, ID_Gender)
            values(N'{0}', N'{1}', '{2}', '{3}', '{4}', current_timestamp, {5})",
            UserName.Text,
            RealName.Text,
            GetSHA_256(PassBox.Password)
);

            try { cmd.ExecuteNonQuery(); }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + cmd.CommandText); return;
            }
            MessageBox.Show("Регистрация завершена");

            Regex r1 = new Regex(@"[^a-z0-9_]", RegexOptions.IgnoreCase);//английские буквы, цифры, нижнее подчеркивание
            //\d[0 - 9]   Цифровой символ
            //\D[^ 0 - 9]  Нецифровой символ
            //\s[ \f\n\r\t\v]   Пробельный символ
            //\S[^ \f\n\r\t\v]  Непробельный символ
            //\w[[:word:]]	Буквенный или цифровой символ или знак подчёркивания
            //\W[^[:word:]] Любой символ, кроме буквенного или цифрового символа или знака подчёркивания

            if (r1.IsMatch(UserName.Text))
            {
                MessageBox.Show("Логин содержит недопустимые символы");
                return;
            }

        }
        private void Registration(object sender, RoutedEventArgs e)
        {
            if (UserName.Text.Equals(String.Empty))
            {
              MessageBox.Show("Введите логин");
              return;
            }
            if (PassBox.Password.Equals(String.Empty))
            {
                MessageBox.Show("Введите пароль");
            }
            if (!PassBox.Password.Equals(PassBox.Password))
            {
              MessageBox.Show("Пароли не совпадают");
            }
            //Валидация данных
            var login_regex = new Regex(@"\W");
            if (login_regex.IsMatch(UserName.Text))
            {
                MessageBox.Show("Логин содержит недопустимые символы");
                return;
            }
            var email_regex = new Regex(@"^([+]?[\s0-9]+)?(\d{3}|[(]?[0-9]+[)])?([-]?[\s]?[0-9])+$");
            if (email_regex.IsMatch(Email.Text))
            {
                MessageBox.Show("Email содержит недопустимые символы");
            }
            var phone_regex = new Regex(@"\D");
            if (phone_regex.IsMatch(Phone.Text))
            {
                MessageBox.Show("Телефон содержит недопустимые символы");
            }
            if(con == null)
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings[0].ConnectionString);
                try { con.Open(); }
                catch (Exception ex) {MessageBox.Show(ex.Message);return; }
            }
            string sql = "SELECT ID, Login, RealName, FROM Users WHERE Login = N'" + UserName.Text + "' AND PassHash = '" + GetSHA_256(PassBox.Password) + "'";
            var cmd = new SqlCommand(sql, con);
            SqlDataReader rdr = null;
            try { rdr = cmd.ExecuteReader(); }
            catch(Exception ex) { MessageBox.Show(ex.Message);return; }
            if (rdr.HasRows)
            {
                var ww = new Window3();
                rdr.Read();
                ww.user = new User()
                {
                    id = rdr.GetInt32(0),
                    Login = rdr.GetString(1),
                    RealName = rdr.GetString(2),
                    PassHash = rdr.GetString(3),
                    ID_Gender = rdr.GetString(4),
                    Email = rdr.GetString(5),
                    PhoneNum = rdr.GetString(6),
                    LastVisitDT = rdr.GetDateTime(7)
                    // RegisterDT = rdr.GetDateTime(9),
                    //RecoveryCode = rdr.GetValue(7).Equals(DBNull.Value)
                };
                   // RegisterDT = rdr.GetDateTime(9),
                    //RecoveryCode = rdr.GetValue(7).Equals(DBNull.Value)
                }

                else { MessageBox.Show("Test");}
            }



          



        }
    }

