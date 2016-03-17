using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqLiteTest.entity
{
    /// <summary>
    /// 品牌实体
    /// </summary>
    public class Brand
    {
        private int _id;

        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _name;

        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private  string _introduction;

        public virtual string Introduction
        {
            get { return _introduction; }
            set { _introduction = value; }
        }
        private string _logo;

        public virtual string Logo
        {
            get { return _logo; }
            set { _logo = value; }
        }
        private string _imgPaths;

        public virtual string ImgPaths
        {
            get { return _imgPaths; }
            set { _imgPaths = value; }
        }

        private string _url;

        public virtual string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        private string _sortChar;

        public virtual string SortChar
        {
            get { return _sortChar; }
            set { _sortChar = value; }
        }

        //private Catagory _catagory;

        //public virtual Catagory Catagory
        //{
        //    get { return _catagory; }
        //    set { _catagory = value; }
        //}
    }
}
