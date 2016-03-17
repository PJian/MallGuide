using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityManagementService.entity
{
    [Serializable]
    public class SalePromotion : INotifyPropertyChanged
    {
        public SalePromotion()
        {
            this.ImgPaths = new string[0];
        }
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                if (this.PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                }
            }
        }
        private string[] _imgPaths;

        public string[] ImgPaths
        {
            get { return _imgPaths; }
            set { _imgPaths = value; }
        }
        private string _introduction;

        public string Introduction
        {
            get { return _introduction; }
            set
            {
                _introduction = value;
                if (this.PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Introduction"));
                }
            }
        }
        private string _startTime;

        public string StartTime
        {
            get { return _startTime; }
            set
            {
                _startTime = value;

                if (this.PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("StartTime"));
                }
            }
        }
        private string _endTime;

        public string EndTime
        {
            get { return _endTime; }
            set
            {
                _endTime = value;
                if (this.PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("EndTime"));
                }
            }
        }
        private int _range;

        public int Range
        {
            get { return _range; }
            set { _range = value; }
        }

        private int _count;//报名参加的人数

        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }

        public override bool Equals(object obj)
        {
            if (obj is SalePromotion)
            {
                SalePromotion s = (SalePromotion)obj;
                return this.Id == s.Id;
            }
            else
            {
                return false;
            }
        }
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
