using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

namespace HP_log_csharp
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filepath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filepath);

        private string m_strIniFile;
        private List<string> m_arrPM = new List<string>();

        public MainWindow()
        {
            InitializeComponent();

            ReadIni();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            WriteIni();
        }

        private void ReadIni()
        {
            StringBuilder retVal = new StringBuilder();

            string strSection = "ENV";

            string strCur = System.IO.Directory.GetCurrentDirectory();
            m_strIniFile = strCur + @"\env.ini";
            GetPrivateProfileString(strSection, "IP", "210.183.186.15", retVal, 32, m_strIniFile);
            ED_IP.Text = retVal.ToString(); retVal.Clear();
            GetPrivateProfileString(strSection, "ID", "", retVal, 32, m_strIniFile);
            ED_ID.Text = retVal.ToString(); retVal.Clear();

            int nCount = 0;
            strSection = "GROUP";
            GetPrivateProfileString(strSection, "COUNT", "0", retVal, 32, m_strIniFile);
            nCount = Convert.ToInt32(retVal.ToString()); retVal.Clear();
            if (nCount < 1)
                return;

            int i = 0;
            string strKey = "", strVal = "";
            for (i = 0; i < nCount; i++)
            {
                strKey = String.Format("{0:D2}", i);
                GetPrivateProfileString(strSection, strKey, "", retVal, 32, m_strIniFile);
                strVal = retVal.ToString(); retVal.Clear();
                m_arrPM[i] = strVal;
            }

        }

        private void WriteIni()
        {
            string strSection = "ENV";
            WritePrivateProfileString(strSection, "IP", ED_IP.Text, m_strIniFile);
            WritePrivateProfileString(strSection, "ID", ED_ID.Text, m_strIniFile);

            if (m_arrPM.Count < 1)
                return;

            strSection = "GROUP";

            int i = 0;
            string strKey = "";
            for(i = 0; i < m_arrPM.Count; i++)
            {
                strKey = String.Format("{0:D2}", i);
                WritePrivateProfileString(strSection, strKey, m_arrPM[i], m_strIniFile);
            }
        }

        private void BTN_CONNECT_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Connect");
        }

        private void BTN_LOGIN_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("LOGIN");
        }

        private void BTN_RESIST_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BTN_UNRESIST_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ED_INPUT_PM_KeyUp(object sender, KeyEventArgs e)
        {
            string strText = ED_INPUT_PM.Text.Trim();
            if (strText.Length < 1)
                return;


        }
    }
}
