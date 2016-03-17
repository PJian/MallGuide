using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace EntityManagementService.entity
{
    public class ClientComputer : INotifyPropertyChanged
    {
        private string _ip;

        public string IP
        {
            get { return _ip; }
            set
            {
                _ip = value;
                if (this.PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IP"));
                }
            }
        }
        public string UserName { get; set; }
        public string UpdateDate { get; set; }
        public string AppDir { get; set; }

        private Boolean _isConnected;

        public Boolean IsConnected
        {
            get { return _isConnected; }
            set
            {
                _isConnected = value;
                if (this.PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsConnected"));
                }
            }
        }

        private int _totalFileNum;

        public int TotalFileNum
        {
            get { return _totalFileNum; }
            set
            {
                _totalFileNum = value;
                if (this.PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("TotalFileNum"));
                }
            }
        }

        private int _updateFileNum;

        public int UpdateFileNum
        {
            get { return _updateFileNum; }
            set
            {
                _updateFileNum = value;
                Percentage = Math.Round(((double)UpdateFileNum / TotalFileNum )*100,1) +"%";
                Console.WriteLine("处理进度{0}",Percentage);
                if (this.PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("UpdateFileNum"));
                }
            }
        }
        private string _percentage;

        public string Percentage
        {
            get { return _percentage; }
            set { _percentage = value;
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("Percentage"));

            }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
