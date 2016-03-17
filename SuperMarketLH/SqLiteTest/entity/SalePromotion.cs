using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityManageService.entity
{
    public class SalePromotion
    {
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
            set { _name = value; }
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
            set { _introduction = value; }
        }
        private string _startTime;

        public string StartTime
        {
            get { return _startTime; }
            set { _startTime = value; }
        }
        private string _endTime;

        public string EndTime
        {
            get { return _endTime; }
            set { _endTime = value; }
        }
        private int _range;

        public int Range
        {
            get { return _range; }
            set { _range = value; }
        }

    }
}
