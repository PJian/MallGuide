using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EntityManagementService.entity
{
    public class ConstantData
    {
        public const int RESOURCE_TYPE_IMG = 1;
        public const int RESOURCE_TYPE_MOVIE = 2;

        public const string MALL_RESOURCE_IMG_FOLDER = @"data\mall\img";
        public const string MALL_RESOURCE_MOVIE_FOLDER = @"data\mall\movie";

        public const string HOTEL_RESOURCE_IMG_FOLDER = @"data\hotel\img";
        public const string HOTEL_RESOURCE_MOVIE_FOLDER = @"data\hotel\movie";

        public const string SHOP_MALL_RESOURCE_IMG_FOLDER = @"data\shopmall\img";
        public const string SHOP_MALL_RESOURCE_MOVIE_FOLDER = @"data\shopmall\movie";



        public const int IMAGE_AD_RESOURCE_TYPE_FIRE_EMERGENCE = 1;
        public const int IMAGE_AD_RESOURCE_TYPE_ELEC_MAGAZINE = 2;
        public const int IMAGE_AD_RESOURCE_TYPE_EMPLOYEE = 3;
        public const int IMAGE_AD_RESOURCE_TYPE_AD = 4;
        public const int IMAGE_AD_RESOURCE_TYPE_SUROUND_INFO = 5;
        public const int IMAGE_AD_RESOURCE_TYPE_SCREEN_PROTECT = 6;

        public static string getBrandLogoDataFolder(int id) {
            return @"data\brand\"+id+@"\logo";
        }
        public static string getBrandImgsDataFolder(int id)
        {
            return @"data\brand\" + id + @"\imgs";
        }

        public static string getShopLogoDataFolder(int id)
        {
            return @"data\shop\" + id + @"\logo";
        }
        public static string getBrandFacilitiesDataFolder(int id)
        {
            return @"data\shop\" + id + @"\facilities";
        }

        public const int SHOP_TYPE_NORMAL = 1;
        public const int SHOP_TYPE_SPECIAL = 2;


        public const int SALE_PROMOTION_OBJECT_ALL = 1;
        public const int SALE_PROMOITON_OBJECT_MEMBER = 2;


        public static string getSalePromotionImgsDataFolder(int id)
        {
            return @"data\salePromotion\" + id + @"\imgs";
        }



        public static string getFloorBgDataFolder(int id)
        {
            return @"data\floor\" + id + @"\bg";
        }

        public static string getMapDataFileName(Floor floor)
        {
            
            return @"data\floor\" + floor.Id + @"\data\map.data";
        }
        /// <summary>
        /// 得到图片资源的文件夹
        /// </summary>
        /// <returns></returns>
        public static string getImgADResourceFolder(int id) {
            return @"data\resources\" + id ;
        }

        internal static string getMachineDataFileName()
        {
            return @"data\resources\machine.cfg.data";
        }


        public const int SERVER_NODE_CONNNECT = 1;
        public const int SERVER_NODE_NOT_CONNECT = 2;

        public const int SERVER_NODE_UPDATE = 1;
        public const int SERVER_NODE_UPDATE_NOT_YET = 2;

        

    }
}
