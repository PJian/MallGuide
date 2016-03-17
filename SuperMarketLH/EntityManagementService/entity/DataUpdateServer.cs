using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace EntityManagementService.entity
{
    /// <summary>
    /// 数据更新服务端
    /// </summary>
    public class DataUpdateServer : INotifyPropertyChanged
    {
        public string IP { get; set; }
        public string UpdateTime { get; set; }
        /// <summary>
        /// 节点的数据状态
        /// </summary>
        public string State { get; set; }
        private string _stateImg;
        public string StateImg { get {
            return this._stateImg;
        }
            set {
                this._stateImg = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("StateImg"));
                }
            }
        }
        /// <summary>
        /// 节点的联通状态
        /// </summary>
        public string NodeState { get; set; }
        private string _nodeStateImg;
        public string NodeStateImg
        {
            get { return this._nodeStateImg; }
            set
            {
                this._nodeStateImg = value;
                if (PropertyChanged != null) {
                    PropertyChanged(this, new PropertyChangedEventArgs("NodeStateImg"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
