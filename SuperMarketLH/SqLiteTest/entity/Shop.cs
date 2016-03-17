using SqLiteTest.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityManageService.entity
{
    /// <summary>
    /// 商铺实体
    /// </summary>
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
        private int _index;

        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }
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

        public Catagory Catagory
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
        private string _openTime;

        public string OpenTime
        {
            get { return _openTime; }
            set { _openTime = value; }
        }
        private SalePromotion _salePromotion;

        public SalePromotion SalePromotion
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
        /// 是否点击店铺的时候显示巨幅广告
        /// </summary>
        private Boolean _showBiggerAD;

        public Boolean ShowBiggerAD
        {
            get { return _showBiggerAD; }
            set { _showBiggerAD = value; }
        }
    }
}
