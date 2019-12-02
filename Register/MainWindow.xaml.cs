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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Register
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public SqlConnection con;
        bool flag = true;
        public MainWindow()
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

        private void Register_Window(object sender, RoutedEventArgs e)
        {
            Window1 w1 = new Window1();
            w1.Show();
        }

        private void Re_Window(object sender, RoutedEventArgs e)
        {
            Window2 w2 = new Window2();
            w2.Show();
        }

        private void Hi_Window(object sender, RoutedEventArgs e)
        {
            Window3 w3 = new Window3();
            w3.Show();
        }





        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string sql = @"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME=N'Users'";
            var cmd = new SqlCommand(sql, con);
            int n = -1;
            try
            {
                n = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            try
            {
                if (n == 0)
                {
                    cmd.CommandText = @"CREATE TABLE USERS (ID INT PRIMARY KEY IDENTITY,
Login NVARCHAR(50),
RealName NVARCHAR(max),
PassHash Char(64),
ID_Gender INT,
Email NVARCHAR(max),
RegisterDT DATE DEFAULT CURRENT_TIMESTAMP,
LastVisitDT DATE,
PhoneNum VARCHAR(20),
RecoveryCode INT NULL)";
                    cmd.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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



    }
}
