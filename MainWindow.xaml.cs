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
using winformNHlib;

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
        private winformNHlib.NHAxControl m_nhctr;
        private NHAxControl.RealDataReceived OnRealDataReceived;

        public MainWindow()
        {
            InitializeComponent();


            this.OnRealDataReceived += new winformNHlib.NHAxControl.RealDataReceived(OnRealData);

            ReadIni();
        }

        public void OnRealData(object sender, winformNHlib.MyRealDataEventArgs e)
        {

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
                m_arrPM.Add(strVal);
                LIST_PMCODE.Items.Add(strVal);
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
            WritePrivateProfileString(strSection, "COUNT", m_arrPM.Count.ToString(), m_strIniFile);
      
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
            string strCur = System.IO.Directory.GetCurrentDirectory();
            string strCommsu = strCur + @"\system\Commsu.ini";

            WritePrivateProfileString("CONNECT", "HOST_ADDR", ED_IP.Text, strCommsu);

            MessageBox.Show("Connect");
        }

        private void BTN_LOGIN_Click(object sender, RoutedEventArgs e)
        {
            m_nhctr.CommLogin(ED_ID.Text, ED_PASSWORD.Password.ToString(), ED_PASSWORD2.Password.ToString());
            //axNHOCX.OCommLogin(ED_ID.Text, ED_PASSWORD.Password.ToString(), ED_PASSWORD2.Password.ToString());
        }

        private void BTN_RESIST_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BTN_UNRESIST_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ED_INPUT_PM_KeyUp(object sender, KeyEventArgs e)
        {
            //if(e.KeyData == Keys.Enter)
            if ( e.Key != Key.Enter )
                return;

            string strText = ED_INPUT_PM.Text.Trim();
            if (strText.Length < 1)
                return;

            bool bFind = false;
            foreach(string it in m_arrPM)
            {
                if (strText == it)
                {
                    bFind = true;
                    break;
                }
            }

            if (bFind)  // bacause duplicate
            {
                MessageBox.Show("Already Exist.");
                return;
            }

            LIST_PMCODE.Items.Add(strText);
            m_arrPM.Add(strText);
        }

        private void LIST_PMCODE_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Delete)
                return;

            var result = MessageBox.Show("Delete Item?", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
                return;

            //ListViewItem lvitem = LIST_PMCODE.Items.GetItemAt(LIST_PMCODE.SelectedIndex);
            string strItem = LIST_PMCODE.SelectedItems[0].ToString();
            LIST_PMCODE.Items.RemoveAt(LIST_PMCODE.SelectedIndex);
            m_arrPM.Remove(strItem);

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.Integration.WindowsFormsHost host =
                new System.Windows.Forms.Integration.WindowsFormsHost();

            m_nhctr = new winformNHlib.NHAxControl();

            host.Child = m_nhctr;

            
        }

    }
}
