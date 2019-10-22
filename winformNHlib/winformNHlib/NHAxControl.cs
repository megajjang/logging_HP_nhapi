using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace winformNHlib
{
    public class MyRealDataEventArgs// : EventArgs
    {
        public short nKey;
        public short nDataLen;
        public string szData;
    }

    public partial class NHAxControl : UserControl
    {
        public event EventHandler<MyRealDataEventArgs> ORecvRealDataEvent;
        public delegate void RealDataReceived(object sender, MyRealDataEventArgs e);

        

        public NHAxControl()
        {
            InitializeComponent();

            axCommOCX1.ORecvRealData += new AxCOMMOCXLib._DCommOCXEvents_ORecvRealDataEventHandler(OnRealDataReceived);
            //axCommOCX1.ORecvMasterFile += new AxCOMMOCXLib._DCommOCXEvents_ORecvMasterFileEvent(OnRecvMasterFile);
        }

        public void OnRealDataReceived(object sender, AxCOMMOCXLib._DCommOCXEvents_ORecvRealDataEvent e)
        {
            MyRealDataEventArgs my = new MyRealDataEventArgs();
            my.nKey = e.nKey;
            my.nDataLen = e.nDataLen;
            my.szData = e.szData;

            ORecvRealDataEvent(sender, my);
        }

        public short OnRecvMasterFile(object sender, AxCOMMOCXLib._DCommOCXEvents_ORecvMasterFileEvent e)
        {
            return 1;
        }

        public short CommLogin(string id, string pwd, string epwd)
        {
            return axCommOCX1.OCommLogin(id, pwd, epwd);
        }

        public void CommTerminate()
        {
            axCommOCX1.OCommTerminate();
        }

        public bool CommSetBrodReal(short key, string data)
        {
            return axCommOCX1.OCommSetBrodReal(key, data);
        }

        public bool RemoveBrodReal(short key, string data)
        {
            return axCommOCX1.ORemoveBrodReal(key, data);
        }

        public short GetMasterFile()
        {
            return axCommOCX1.OGetMasterFileFromServer();
        }

        //public 
    }
}
