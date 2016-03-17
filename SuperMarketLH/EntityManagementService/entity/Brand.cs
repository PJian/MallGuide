
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityManagementService.entity
{
    /// <summary>
    /// 品牌实体
    /// </summary>
    [Serializable]
    public class Brand
    {
        public Brand(){
            ImgPaths = new string[0];
        }
        private int _id;

        public  int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _name ="";
         
        public  string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private  string _introduction="";
        
        public  string Introduction
        {
            get { return _introduction; }
            set { _introduction = value; }
        }
        private string _logo="";
       
        public  string Logo
        {
            get { return _logo; }
            set { _logo = value; }
        }
        private string[] _imgPaths;
         
        public  string[] ImgPaths
        {
            get { return _imgPaths; }
            set { _imgPaths = value; }
        }

        private string _url="";
      
        public  string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        private string _sortChar="";
      
        public  string SortChar
        {
            get { return _sortChar; }
            set { _sortChar = value; }
        }

        private Catagory _catagory;

        public  Catagory CatagoryName
        {
            get { return _catagory; }
            set { _catagory = value; }
        }
    }
}
