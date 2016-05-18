using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EntityManagementService.entity
{
    /// <summary>
    /// 商铺实体
    /// </summary>
    /// 
    [Serializable]
    public class Shop
    {
        

        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private Floor _floor;

        public Floor Floor
        {
            get { return _floor; }
            set { _floor = value; }
        }

        private string _index;

        public string Index
        {
            get { return _index; }
            set { _index = value; }
        }

        //private int _index;

        //public int Index
        //{
        //    get { return _index; }
        //    set { _index = value; }
        //}
        /// <summary>
        /// 入驻的品牌
        /// </summary>
        private Brand _brand;

        public Brand Brand
        {
            get { return _brand; }
            set { _brand = value; }
        }
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private string _logo;

        public string Logo
        {
            get { return _logo; }
            set { _logo = value; }
        }
        private Catagory _catagory;

        public Catagory CatagoryName
        {
            get { return _catagory; }
            set { _catagory = value; }
        }
        private string _label;

        public string Label
        {
            get { return _label; }
            set { _label = value; }
        }
        private string _sortChar;

        public string SortChar
        {
            get { return _sortChar; }
            set { _sortChar = value; }
        }
        private string _catagoryColor;

        public string CatagoryColor
        {
            get { return _catagoryColor; }
            set { _catagoryColor = value; }
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
        /// <summary>
        /// 促销活动
        /// </summary>
        private List<SalePromotion> _salePromotion;

        public List<SalePromotion> SalePromotion
        {
            get { return _salePromotion; }
            set { _salePromotion = value; }
        }
        private string _introduction;

        public string Introduction
        {
            get { return _introduction; }
            set { _introduction = value; }
        }
        private string _tel;

        public string Tel
        {
            get { return _tel; }
            set { _tel = value; }
        }
        private string _address;

        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }
        private string _zipCode;

        public string ZipCode
        {
            get { return _zipCode; }
            set { _zipCode = value; }
        }
        /// <summary>
        /// 是否是主力店铺
        /// </summary>
        private int _type;

        public int Type
        {
            get { return _type; }
            set { _type = value; }
        }
        public override bool Equals(object obj)
        {
            if (obj is Shop)
            {
                Shop s = (Shop) obj;
                return this.Id == s.Id;
            }
            else {
                return false;
            }
        }
        public override int GetHashCode()
        {
            return this.Id.GetHashCode() ;
        }
        /// <summary>
        /// 店铺配套的设施
        /// </summary>
        private string[] _facilities;

        public string[] Facilities
        {
            get { return _facilities; }
            set { _facilities = value; }
        }

        /// <summary>
        /// 店铺配套的设施
        /// </summary>
        private string[] _brandImgs;

        public string[] BrandImgs
        {
            get { return _brandImgs; }
            set { _brandImgs = value; }
        }

       

      


        //是地图上的格子，不是真实的点，用来寻路的点
        private Point _door;

        public Point Door
        {
            get { return _door; }
            set { _door = value; }
        }

        /// <summary>
        /// 取得店铺的活动推广图
        /// </summary>
        /// <param name="shop"></param>
        /// <returns></returns>
        public List<string> getShopPromotionImgOfValidate()
        {
            List<string> pathes = new List<string>();
            if (this.SalePromotion == null) return pathes;
            foreach (SalePromotion salePromotion in this.SalePromotion)
            {
                if (DateTime.Parse(salePromotion.EndTime) >= DateTime.Now)
                {
                    pathes.AddRange(salePromotion.ImgPaths.ToList());
                }
            }
            return pathes;
        }



    }
}
