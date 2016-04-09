using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace EntityManagementService.entity
{
    public class SplitPage : INotifyPropertyChanged
    {
        public int PageSize { get; set; }   //每页显示行数

        private int nMax = 0;    //总记录数

        public int NMax
        {
            get { return nMax; }
            set { nMax = value; }
        }

        private int pageCount = 0;   //总页数

        public int PageCount
        {
            get { return pageCount; }
            set { pageCount = value; }
        }

        private int pageCurrent = 0;     //当前行号

        public int PageCurrent
        {
            get { return pageCurrent; }
            set
            {
                pageCurrent = value;

                if (this.PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("PageCurrent"));
                }
            }
        }

        private int nCurrent = 0;    //当前记录行

        public int NCurrent
        {
            get { return nCurrent; }
            set { nCurrent = value; }
        }




        public event PropertyChangedEventHandler PropertyChanged;
    }
}
