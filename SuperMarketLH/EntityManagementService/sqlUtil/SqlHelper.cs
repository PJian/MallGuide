using EntityManagementService.entity;
using System.Data.SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows;

namespace EntityManageService.sqlUtil
{

    /// <summary>
    /// sqlite 数据库帮助类
    /// </summary>
    public class SqlHelper
    {
        public static SQLiteConnection sqLiteConnection;
        public static void openConnection()
        {

            sqLiteConnection = new SQLiteConnection("DataSource=" + AppDomain.CurrentDomain.BaseDirectory + "data/MyDataBase.sqLite;Version=3;");
            sqLiteConnection.Open();
        }

        public static void executeSql(String sql)
        {
            openConnection();
            using (SQLiteCommand cmd = new SQLiteCommand(sql, sqLiteConnection))
            {
                cmd.ExecuteNonQuery();
            }
            sqLiteConnection.Close();
        }
        /// <summary>
        /// 执行sql语句，并且返回insert语句的id
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="getNewId"></param>
        /// <returns></returns>
        public static int executeSql(String sql, bool getNewId)
        {
            int rowId = 0;
            openConnection();
            using (SQLiteCommand cmd = new SQLiteCommand(sql, sqLiteConnection))
            {
                cmd.ExecuteNonQuery();

            }
            if (getNewId)
            {
                using (SQLiteCommand cmd = new SQLiteCommand("select last_insert_rowid()", sqLiteConnection))
                {
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        rowId = int.Parse(reader[0].ToString());
                    }
                }
            }
            sqLiteConnection.Close();
            return rowId;
        }



        public static SQLiteDataReader executeQuery(string sql)
        {
            //openConnection();
            SQLiteDataReader reader = null;
            using (SQLiteCommand cmd = new SQLiteCommand(sql, sqLiteConnection))
            {
                reader = cmd.ExecuteReader();
            }
            //  sqLiteConnection.Close();
            return reader;
        }
        public static DataTable executeQueryDataTable(string sql)
        {
            openConnection();
            DataSet ds;
            using (SQLiteCommand cmd = new SQLiteCommand(sql, sqLiteConnection))
            {
                using (SQLiteDataAdapter reader = new System.Data.SQLite.SQLiteDataAdapter(cmd))
                {
                    ds = new DataSet();
                    reader.Fill(ds);
                }

            }
            sqLiteConnection.Close();
            return ds.Tables[0];
        }

        /// <summary>
        /// 执行带有参数的sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        public static void executeSql(String sql, params SQLiteParameter[] param)
        {
            openConnection();
            using (SQLiteTransaction mytransaction = sqLiteConnection.BeginTransaction())
            {
                using (SQLiteCommand mycommand = new SQLiteCommand(sqLiteConnection))
                {
                    SQLiteParameter myparam = new SQLiteParameter();

                    mycommand.CommandText = sql;
                    mycommand.Parameters.AddRange(param);
                    mycommand.ExecuteNonQuery();
                }
                mytransaction.Commit();
            }
            sqLiteConnection.Close();
        }
        /// <summary>
        ///  保存品牌
        /// </summary>
        /// <param name="brand"></param>
        ///<returns> 返回刚刚插入数据的id</returns>
        public static int saveBrand(Brand brand)
        {
            string sql = "insert into  tabbrand(name,instruction,logo,sortChar,url,imgs,catagoryId) values(";
            sql += "'" + brand.Name + "','" + brand.Introduction + "','" + brand.Logo + "','" + brand.SortChar + "','" + brand.Url;
            sql += "','";
            sql += getImgPaths(brand.ImgPaths) + "'";

            if (brand.CatagoryName != null)
            {
                sql += ",'" + brand.CatagoryName.Id + "'";
            }
            else
            {
                sql += ",''";
            }
            sql += ")";
            return executeSql(sql, true);
            //Console.WriteLine(sql);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="brand"></param>
        public static void deleteBrand(Brand brand)
        {
            String sql = "delete from tabbrand where id=" + brand.Id;
            executeSql(sql);
        }
        /// <summary>
        /// 删除品牌同时级联删除关联信息
        /// </summary>
        /// <param name="brand"></param>
        /// <param name="cascade"></param>
        public static void deleteBrand(Brand brand, bool cascade)
        {
            String sql = "delete from tabbrand where id=" + brand.Id;
            executeSql(sql);
            if (cascade)
            {
                deleteRelationBetShopAndBrand(brand);
            }
        }
        /// <summary>
        /// 更新品牌
        /// </summary>
        /// <param name="brand"></param>
        public static void updateBrand(Brand brand)
        {
            string sql = "update tabbrand set name='" + brand.Name + "' ,url='" +
                brand.Url + "',logo='" +
                brand.Logo + "',instruction='" +
                brand.Introduction + "',sortChar='" +
                brand.SortChar + "',imgs='" +
                getImgPaths(brand.ImgPaths) + "',catagoryId='" +
                (brand.CatagoryName == null ? "" : brand.CatagoryName.Id + "") + "' where id=" + brand.Id;
            executeSql(sql);
        }
        /// <summary>
        /// 根据id找到品牌
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Brand getBrandById(int id)
        {
            string sql = "select name,logo,url,instruction,sortChar,imgs,catagoryId from tabbrand where id = " + id + "";
            DataTable dt = executeQueryDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                return new Brand()
                {
                    Id = id,
                    Name = dt.Rows[0]["name"].ToString(),
                    Logo = dt.Rows[0]["logo"].ToString(),
                    Url = dt.Rows[0]["url"].ToString(),
                    Introduction = dt.Rows[0]["instruction"].ToString(),
                    SortChar = dt.Rows[0]["sortChar"].ToString(),
                    ImgPaths = dt.Rows[0]["imgs"].ToString().Split(','),
                    CatagoryName = getCatagoryById(dt.Rows[0]["catagoryId"].ToString())
                };
            }
            return null;
        }
        /// <summary>
        /// 取得全部的品牌
        /// </summary>
        /// <returns></returns>
        public static List<Brand> getAllBrands()
        {
            List<Brand> brands = new List<Brand>();
            string sql = "select id,name,logo,url,instruction,sortChar,imgs,catagoryId from tabbrand  ";
            DataTable dt = executeQueryDataTable(sql);
            foreach (DataRow row in dt.Rows)
            {
                brands.Add(new Brand()
                {
                    Id = int.Parse(row["id"].ToString()),
                    Name = row["name"].ToString(),
                    Logo = row["logo"].ToString(),
                    Url = row["url"].ToString(),
                    Introduction = row["instruction"].ToString(),
                    SortChar = row["sortChar"].ToString(),
                    ImgPaths = filterImgPathesEmpty(row["imgs"].ToString().Split(',')),
                    CatagoryName = getCatagoryById(row["catagoryId"].ToString())
                });
            }
            return brands;
        }
        /// <summary>
        /// 将路径中的空路径过滤掉
        /// </summary>
        /// <param name="str"></param>
        private static string[] filterImgPathesEmpty(string[] str)
        {
            List<string> rtn = new List<string>();
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] != null && !"".Equals(str[i]))
                {
                    rtn.Add(str[i]);
                }
            }
            return rtn.ToArray();
        }

        /// <summary>
        /// 根据id找到分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Catagory getCatagoryById(string id)
        {
            if (id != null && !id.Trim().Equals(""))
            {
                string sql = "select name,sortChar,logo from tabcatagory where id = " + id;
                DataTable dt = executeQueryDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    return new Catagory()
                    {
                        Id = int.Parse(id),
                        Name = dt.Rows[0]["name"].ToString(),
                        Logo = dt.Rows[0]["logo"].ToString(),
                        SortChar = dt.Rows[0]["sortChar"].ToString(),
                    };
                }
                return null;

            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 根据名字去查找分类
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Catagory getCatagoryByName(string p)
        {
            string sql = "select id,name,sortChar,logo from tabcatagory where name like '%" + p + "%'";
            DataTable dt = executeQueryDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                return new Catagory()
                {
                    Id = int.Parse(dt.Rows[0]["id"].ToString()),
                    Name = dt.Rows[0]["name"].ToString(),
                    Logo = dt.Rows[0]["logo"].ToString(),
                    SortChar = dt.Rows[0]["sortChar"].ToString(),
                };
            }
            return null;
        }

        /// <summary>
        /// 得到全部的分类
        /// </summary>
        /// <returns></returns>
        public static List<Catagory> getAllCatagory1()
        {
            SQLiteDataReader reader = null;
            try
            {
                string sql = "select id,name,sortChar,logo from tabcatagory ";
                List<Catagory> catagories = new List<Catagory>();
                reader = executeQuery(sql); ;
                while (reader.Read())
                {
                    catagories.Add(new Catagory()
                    {
                        Id = int.Parse(reader["id"].ToString()),
                        Name = reader["name"].ToString(),
                        Logo = reader["logo"].ToString(),
                        SortChar = reader["sortChar"].ToString(),
                    });
                }
                return catagories;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }
        /// <summary>
        /// 取得全部的楼层信息
        /// datatable
        /// </summary>
        /// <returns></returns>
        public static List<Floor> getAllFloor()
        {
            List<Floor> floors = new List<Floor>();
            string sql = "select id,name,indexFloor,label,bg,map from tabfloor";
            DataTable dt = executeQueryDataTable(sql);
            foreach (DataRow row in dt.Rows)
            {
                floors.Add(new Floor()
                {
                    Id = int.Parse(row["id"].ToString()),
                    Name = row["name"].ToString(),
                    Label = row["label"].ToString(),
                    Map = row["map"].ToString(),
                    Img = row["bg"].ToString(),
                    Index = int.Parse(row["indexFloor"].ToString())
                });
            }
            return floors;
        }

        /// <summary>
        /// 保存楼层
        /// </summary>
        /// <param name="floor"></param>
        public static int saveFloor(Floor floor)
        {
            string sql = "insert into  tabfloor(name,indexFloor,label,bg,map) values(";
            sql += "'" + floor.Name + "','" + floor.Index + "','" + floor.Label + "','" + floor.Img + "','" + floor.Map;
            sql += "')";
            return executeSql(sql, true);
        }
        /// <summary>
        /// 更新楼层
        /// </summary>
        /// <param name="floor"></param>
        public static void updateFloor(Floor floor)
        {
            string sql = "update tabfloor set name='" +
                floor.Name + "',indexFloor='" + floor.Index + "',label='" + floor.Label + "',bg='" + floor.Img + "',map='" +
                floor.Map + "' where id=" + floor.Id;
            executeSql(sql);
        }

        /// <summary>
        /// 删除楼层
        /// </summary>
        /// <param name="floor"></param>
        public static Boolean deleteFloor(Floor floor)
        {
            if (floor != null && floor.Id != 0)
            {
                string sql = "delete from tabfloor where id = " + floor.Id;
                executeSql(sql);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 根据id查找楼层
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Floor getFloorById(string id)
        {

            string sql = "select name,indexFloor,label,bg,map from tabfloor where id = " + id;
            DataTable dt = executeQueryDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                return new Floor()
                {
                    Id = int.Parse(id),
                    Name = dt.Rows[0]["name"].ToString(),
                    Index = int.Parse(dt.Rows[0]["indexFloor"].ToString()),
                    Label = dt.Rows[0]["label"].ToString(),
                    Map = dt.Rows[0]["map"].ToString(),
                    Img = dt.Rows[0]["bg"].ToString()
                };
            }

            return null;
        }
        /// <summary>
        /// 根据楼层索引进行查找
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static Floor getFloorByIndex(int index)
        {
            string sql = "select id,name,label,bg,map from tabfloor where indexFloor = " + index;
            DataTable dt = executeQueryDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                return new Floor()
                {
                    Id = int.Parse(dt.Rows[0]["id"].ToString()),
                    Name = dt.Rows[0]["name"].ToString(),
                    Index = index,
                    Label = dt.Rows[0]["label"].ToString(),
                    Map = dt.Rows[0]["map"].ToString(),
                    Img = dt.Rows[0]["bg"].ToString()
                };
            }
            return null;
        }
        /// <summary>
        /// 得到全部的楼层
        /// </summary>
        /// <returns></returns>
        public static List<Floor> getAllFloors()
        {
            List<Floor> floors = new List<Floor>();
            SQLiteDataReader reader = null;
            try
            {
                string sql = "select id,name,index,label,bg,map from tabcatagory ";
                reader = executeQuery(sql); ;
                while (reader.Read())
                {
                    floors.Add(new Floor()
                    {
                        Id = int.Parse(reader["id"].ToString()),
                        Name = reader["name"].ToString(),
                        Index = int.Parse(reader["index"].ToString()),
                        Label = reader["label"].ToString(),
                        Map = reader["map"].ToString()
                    });
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return floors;
        }
        /// <summary>
        /// 保存促销活动
        /// </summary>
        /// <param name="salePromotion"></param>
        /// <returns></returns>
        public static int saveSalePromotion(SalePromotion salePromotion)
        {
            string sql = "insert into  tabsalespromotion(name,introduction,startDate,endDate,range,imgs) values(";
            sql += "'" + salePromotion.Name + "','" + salePromotion.Introduction + "','" + salePromotion.StartTime + "','" + salePromotion.EndTime + "','" + salePromotion.Range;
            sql += "','";
            sql += getImgPaths(salePromotion.ImgPaths) + "')";
            return executeSql(sql, true);
        }
        /// <summary>
        /// 更新促销活动
        /// </summary>
        /// <param name="salePromotion"></param>
        /// <returns></returns>
        public static void updateSalePromotion(SalePromotion salePromotion)
        {
            string sql = "update tabsalespromotion set name='" +
                salePromotion.Name + "',introduction='" +
                salePromotion.Introduction + "',startDate='" +
                salePromotion.StartTime + "',endDate='" +
                salePromotion.EndTime + "',range='" +
                salePromotion.Range + "',imgs='" +
                getImgPaths(salePromotion.ImgPaths) + "' where id = " + salePromotion.Id;
            executeSql(sql);

        }
        /// <summary>
        /// 删除促销活动
        /// </summary>
        /// <param name="salePromotion"></param>
        /// <returns></returns>
        public static Boolean deleteSalePromotion(SalePromotion salePromotion)
        {
            string sql = "delete from tabsalespromotion where id =" + salePromotion.Id;
            executeSql(sql);
            return true;
        }
        /// <summary>
        /// 删除促销信息，同时根据cascade参数级联删除关联信息
        /// </summary>
        /// <param name="salePromotion"></param>
        /// <param name="cascade"></param>
        /// <returns></returns>
        public static Boolean deleteSalePromotion(SalePromotion salePromotion, bool cascade)
        {
            string sql = "delete from tabsalepromotion where id =" + salePromotion.Id;
            executeSql(sql);
            if (cascade)
            {
                deleteSalePromotionFromShop(salePromotion);
            }
            return true;
        }
        /// <summary>
        /// 根据id取得促销活动
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static SalePromotion getSalePromotionById(int id)
        {

            string sql = "select name,introduction,startDate,endDate,range,imgs from tabsalespromotion where id = " + id;

            DataTable dt = executeQueryDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                return new SalePromotion()
                {
                    Id = id,
                    Name = dt.Rows[0]["name"].ToString(),
                    Introduction = dt.Rows[0]["introduction"].ToString(),
                    StartTime = dt.Rows[0]["startDate"].ToString(),
                    EndTime = dt.Rows[0]["endDate"].ToString(),
                    Range = int.Parse(dt.Rows[0]["range"].ToString()),
                    ImgPaths = dt.Rows[0]["imgs"].ToString().Split(',')

                };
            }

            return null;
        }
        /// <summary>
        /// 取得会员活动
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<SalePromotion> getMemberShipSalePromotion()
        {
            List<SalePromotion> allSalePromotions = new List<SalePromotion>();
            string sql = "select id,name,introduction,startDate,endDate,range,imgs from tabsalespromotion  where range = " + ConstantData.SALE_PROMOITON_OBJECT_MEMBER;

            DataTable dt = executeQueryDataTable(sql);
            foreach (DataRow row in dt.Rows)
            {
                allSalePromotions.Add(
                    new SalePromotion()
                    {
                        Id = int.Parse(row["id"].ToString()),
                        Name = row["name"].ToString(),
                        Introduction = row["introduction"].ToString(),
                        StartTime = row["startDate"].ToString(),
                        EndTime = row["endDate"].ToString(),
                        Range = int.Parse(row["range"].ToString()),
                        ImgPaths = row["imgs"].ToString().Split(',')

                    }
                    );
            }

            return allSalePromotions;
        }
        /// <summary>
        /// 取得全体活动
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<SalePromotion> getNormalSalePromotion()
        {
            List<SalePromotion> allSalePromotions = new List<SalePromotion>();
            string sql = "select id,name,introduction,startDate,endDate,range,imgs from tabsalespromotion  where range = " + ConstantData.SALE_PROMOTION_OBJECT_ALL;
            DataTable dt = executeQueryDataTable(sql);
            foreach (DataRow row in dt.Rows)
            {
                allSalePromotions.Add(
                    new SalePromotion()
                    {
                        Id = int.Parse(row["id"].ToString()),
                        Name = row["name"].ToString(),
                        Introduction = row["introduction"].ToString(),
                        StartTime = row["startDate"].ToString(),
                        EndTime = row["endDate"].ToString(),
                        Range = int.Parse(row["range"].ToString()),
                        ImgPaths = row["imgs"].ToString().Split(',')
                    }
                    );
            }

            return allSalePromotions;
        }
        /// <summary>
        /// 取得全部的促销活动
        /// </summary>
        /// <returns></returns>
        public static List<SalePromotion> getAllSalePromotions()
        {
            List<SalePromotion> salePromotions = new List<SalePromotion>();
            string sql = "select id,name,introduction,startDate,endDate,range,imgs from tabsalespromotion ";
            DataTable dt = executeQueryDataTable(sql);
            foreach (DataRow row in dt.Rows)
            {
                salePromotions.Add(new SalePromotion()
                {
                    Id = int.Parse(row["id"].ToString()),
                    Name = row["name"].ToString(),
                    Introduction = row["introduction"].ToString(),
                    StartTime = row["startDate"].ToString(),
                    EndTime = row["endDate"].ToString(),
                    Range = int.Parse(row["range"].ToString()),
                    ImgPaths = row["imgs"].ToString().Split(',')
                });
            }
            return salePromotions;
        }

        public static CommonBuildings getCommonBuildingById(int id)
        {
            if (id > 0)
            {
                SQLiteDataReader reader = null;
                try
                {
                    string sql = "select name,sortChar,logo from tabcommonbuildings where id = " + id;
                    reader = executeQuery(sql); ;
                    while (reader.Read())
                    {
                        return new CommonBuildings()
                        {
                            Id = id,
                            Name = reader["name"].ToString(),
                            SortChar = reader["sortChar"].ToString(),
                            Logo = reader["logo"].ToString()
                        };
                    }
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
                return null;

            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 取得全部的公共设施
        /// </summary>
        /// <returns></returns>
        public static List<CommonBuildings> getAllCommonBuildings()
        {
            SQLiteDataReader reader = null;
            List<CommonBuildings> allCommonBuildings = new List<CommonBuildings>();
            try
            {
                string sql = "select id,name,sortChar,logo from tabcommonbuildings  ";
                reader = executeQuery(sql); ;
                while (reader.Read())
                {
                    allCommonBuildings.Add(new CommonBuildings()
                    {
                        Id = int.Parse(reader["name"].ToString()),
                        Name = reader["name"].ToString(),
                        SortChar = reader["sortChar"].ToString(),
                        Logo = reader["logo"].ToString()
                    });
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return allCommonBuildings;
        }
        /// <summary>
        /// 保存商铺
        /// </summary>
        /// <param name="shop"></param>
        public static int saveShop(Shop shop)
        {
            string sql = "insert into  tabshop(name,floor,positionNum,logo,label,firstNameChar,catagoryColor,startTime,endTime,introduction,tel,address,zipCode,type,catagoryId,facilities,x,y) values(";
            sql += "'" + shop.Name + "','" +
                (shop.Floor == null ? "" : shop.Floor.Id + "") + "','" + shop.Index + "','" + shop.Logo + "','" + shop.Label + "','" + shop.SortChar + "','" + shop.CatagoryColor + "','" + shop.StartTime + "','" + shop.EndTime + "','" + shop.Introduction + "','" + shop.Tel + "','" + shop.Address + "','" +
                shop.ZipCode + "','" + shop.Type;
            sql += "','";

            if (shop.CatagoryName != null)
            {
                sql += shop.CatagoryName.Id + "','";
            }
            else
            {
                sql += "','";
            }
            sql += getImgPaths(shop.Facilities) + "'";
            sql += ","+shop.Door.X+","+shop.Door.Y+")";
            return executeSql(sql, true);
        }
        /// <summary>
        /// 删除店铺
        /// </summary>
        /// <param name="shop"></param>
        public static void deletetShop(Shop shop)
        {
            if (shop != null && shop.Id > 0)
            {
                string sql = "delete from tabshop where id=" + shop.Id;
                executeSql(sql);

            }
        }
        /// <summary>
        /// 删除店铺信息，同时根据cascade参数级联删除关联信息
        /// </summary>
        /// <param name="shop"></param>
        /// <param name="cascade"></param>
        public static void deletetShop(Shop shop, bool cascade)
        {
            if (shop != null && shop.Id > 0)
            {
                string sql = "delete from tabshop where id=" + shop.Id;
                executeSql(sql);
                if (cascade)
                {
                    deleteAdditionalRelation(shop);
                }
            }
        }

        /// <summary>
        /// 更新店铺
        /// </summary>
        /// <param name="shop"></param>
        public static void updateShop(Shop shop)
        {
            string sql = "update tabshop set name='" +
                shop.Name + "',floor='" +
                (shop.Floor == null ? "" : "" + shop.Floor.Id) + "',positionNum='" +
                shop.Index + "',logo='" +
                shop.Logo + "',label='" +
                shop.Label + "',firstNameChar='" +
                shop.SortChar + "',catagoryColor='" +
                shop.CatagoryColor + "',introduction='" +
                shop.Introduction + "',tel='" +
                shop.Tel + "',address='" +
                shop.Address + "',zipCode='" +
                shop.ZipCode + "',catagoryId='" +
                (shop.CatagoryName == null ? "" : "" + shop.CatagoryName.Id) + "',type='" +
                shop.Type + "',endTime='" +
                shop.EndTime + "',startTime='" +
                shop.StartTime + "',facilities='" +
                getImgPaths(shop.Facilities) + "',x='" +
                shop.Door.X + "',y='" +
                shop.Door.Y + "' where id=" + shop.Id;
            executeSql(sql);

            //deletetShop(shop);
            //saveShop(shop);
        }
        public static Shop getShopById(int id)
        {
            if (id > 0)
            {
                string sql = "select name,floor,positionNum,logo,label,firstNameChar,catagoryColor,startTime,endTime,introduction,tel,address,zipCode,type,catagoryId,x,y from tabshop where id=" + id;
                DataTable dt = executeQueryDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    return new Shop()
                    {
                        Id = id,
                        Name = dt.Rows[0]["name"].ToString(),
                        Floor = getFloorById(dt.Rows[0]["floor"].ToString()),
                        Index =dt.Rows[0]["positionNum"].ToString(),
                        Logo = dt.Rows[0]["logo"].ToString(),
                        Label = dt.Rows[0]["label"].ToString(),
                        SortChar = dt.Rows[0]["firstNameChar"].ToString(),
                        CatagoryColor = dt.Rows[0]["catagoryColor"].ToString(),
                        StartTime = dt.Rows[0]["startTime"].ToString(),
                        EndTime = dt.Rows[0]["endTime"].ToString(),
                        Introduction = dt.Rows[0]["introduction"].ToString(),
                        Tel = dt.Rows[0]["tel"].ToString(),
                        Address = dt.Rows[0]["address"].ToString(),
                        ZipCode = dt.Rows[0]["zipCode"].ToString(),
                        Type = int.Parse(dt.Rows[0]["type"].ToString()),
                        CatagoryName = getCatagoryById(dt.Rows[0]["catagoryId"].ToString()),
                        Door = new System.Windows.Point(int.Parse(dt.Rows[0]["x"].ToString()), int.Parse(dt.Rows[0]["y"].ToString()))
                    };
                }
            }
            return null;
        }
        /// <summary>
        /// 取得店铺信息，根据cascade参数决定是否取得全部信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cascade"></param>
        /// <returns></returns>
        public static Shop getShopById(int id, bool cascade)
        {
            Shop shop = getShopById(id);
            if (cascade)
            {
                shop.SalePromotion = getSalePromotionRelatiedWithShop(shop);
                shop.Brand = getBrandRelatiedWithShop(shop);
            }
            return shop;
        }
        /// <summary>
        /// 根据分类取得商铺
        /// </summary>
        /// <param name="catagory"></param>
        /// <returns></returns>
        public static List<Shop> getShopByCatagory(Catagory catagory)
        {
            
            List<Shop> shops = new List<Shop>();
            if (catagory == null) return shops;
            string sql = "select id,name,floor,positionNum,logo,label,firstNameChar,catagoryColor,startTime,endTime,introduction,tel,address,zipCode,type,catagoryId,facilities,x,y from tabshop where catagoryId=" + catagory.Id;
            DataTable dt = executeQueryDataTable(sql);
            foreach (DataRow dr in dt.Rows)
            {
                shops.Add(new Shop()
                {
                    Id = int.Parse(dr["id"].ToString()),
                    Name = dr["name"].ToString(),
                    Floor = getFloorById(dr["floor"].ToString()),
                    Index = dr["positionNum"].ToString(),
                    Logo = dr["logo"].ToString(),
                    Label = dr["label"].ToString(),
                    SortChar = dr["firstNameChar"].ToString(),
                    CatagoryColor = dr["catagoryColor"].ToString(),
                    StartTime = dr["startTime"].ToString(),
                    EndTime = dr["endTime"].ToString(),
                    Introduction = dr["introduction"].ToString(),
                    Tel = dr["tel"].ToString(),
                    Address = dr["address"].ToString(),
                    ZipCode = dr["zipCode"].ToString(),
                    Type = int.Parse(dr["type"].ToString()),
                    CatagoryName = getCatagoryById(dr["catagoryId"].ToString()),
                    Brand = getBrandRelatiedWithShop(int.Parse(dr["id"].ToString())),
                    SalePromotion = getSalePromotionRelatiedWithShop(int.Parse(dr["id"].ToString())),
                    Facilities = dr["facilities"].ToString().Split(','),
                    Door = new System.Windows.Point(int.Parse(dt.Rows[0]["x"].ToString()), int.Parse(dt.Rows[0]["y"].ToString()))
                });
            }
            return shops;
        }

        public static List<Shop> getAllShopByFloor(Floor floor)
        {

            List<Shop> shops = new List<Shop>();
            string sql = "select id,name,floor,positionNum,logo,label,firstNameChar,catagoryColor,startTime,endTime,introduction,tel,address,zipCode,type,catagoryId,facilities,x,y from tabshop where floor = " + floor.Id;
            DataTable dt = executeQueryDataTable(sql);
            foreach (DataRow dr in dt.Rows)
            {
                shops.Add(new Shop()
                {
                    Id = int.Parse(dr["id"].ToString()),
                    Name = dr["name"].ToString(),
                    Floor = getFloorById(dr["floor"].ToString()),
                    Index = dr["positionNum"].ToString(),
                    Logo = dr["logo"].ToString(),
                    Label = dr["label"].ToString(),
                    SortChar = dr["firstNameChar"].ToString(),
                    CatagoryColor = dr["catagoryColor"].ToString(),
                    StartTime = dr["startTime"].ToString(),
                    EndTime = dr["endTime"].ToString(),
                    Introduction = dr["introduction"].ToString(),
                    Tel = dr["tel"].ToString(),
                    Address = dr["address"].ToString(),
                    ZipCode = dr["zipCode"].ToString(),
                    Type = int.Parse(dr["type"].ToString()),
                    CatagoryName = getCatagoryById(dr["catagoryId"].ToString()),
                    Brand = getBrandRelatiedWithShop(int.Parse(dr["id"].ToString())),
                    SalePromotion = getSalePromotionRelatiedWithShop(int.Parse(dr["id"].ToString())),
                    Facilities = dr["facilities"].ToString().Split(','),
                    Door = new System.Windows.Point(int.Parse(dr["x"].ToString()), int.Parse(dr["y"].ToString()))
                });
            }
            return shops;

        }


        /// <summary>
        /// 取得全部的商铺
        /// </summary>
        /// <returns></returns>
        public static List<Shop> getAllShop()
        {
            List<Shop> shops = new List<Shop>();
            string sql = "select id,name,floor,positionNum,logo,label,firstNameChar,catagoryColor,startTime,endTime,introduction,tel,address,zipCode,type,catagoryId,facilities,x,y from tabshop";
            DataTable dt = executeQueryDataTable(sql);
            foreach (DataRow dr in dt.Rows)
            {
                shops.Add(new Shop()
                {
                    Id = int.Parse(dr["id"].ToString()),
                    Name = dr["name"].ToString(),
                    Floor = getFloorById(dr["floor"].ToString()),
                    Index = dr["positionNum"].ToString(),
                    Logo = dr["logo"].ToString(),
                    Label = dr["label"].ToString(),
                    SortChar = dr["firstNameChar"].ToString(),
                    CatagoryColor = dr["catagoryColor"].ToString(),
                    StartTime = dr["startTime"].ToString(),
                    EndTime = dr["endTime"].ToString(),
                    Introduction = dr["introduction"].ToString(),
                    Tel = dr["tel"].ToString(),
                    Address = dr["address"].ToString(),
                    ZipCode = dr["zipCode"].ToString(),
                    Type = int.Parse(dr["type"].ToString()),
                    CatagoryName = getCatagoryById(dr["catagoryId"].ToString()),
                    Brand = getBrandRelatiedWithShop(int.Parse(dr["id"].ToString())),
                    SalePromotion = getSalePromotionRelatiedWithShop(int.Parse(dr["id"].ToString())),
                    Facilities = dr["facilities"].ToString().Split(','),
                    Door = new System.Windows.Point(int.Parse(dt.Rows[0]["x"].ToString()), int.Parse(dt.Rows[0]["y"].ToString()))
                });
            }
            return shops;
        }
        /// <summary>
        /// 为商铺和品牌建立关联
        /// </summary>
        /// <param name="shop"></param>
        /// <param name="brand"></param>
        public static void createRelationBetShopAndBrand(Shop shop, Brand brand)
        {
            if (shop != null && shop.Id > 0 && brand != null && brand.Id > 0)
            {
                string sql = "insert into tabbrandshop(brandId,shopId) values(" + brand.Id + "," + shop.Id + ")";
                executeSql(sql);
            }
        }
        /// <summary>
        /// 删去跟商铺相关的品牌联系
        /// </summary>
        /// <param name="shop"></param>
        public static void deleteRelationBetShopAndBrand(Shop shop)
        {
            if (shop != null)
            {
                executeSql("delete from tabbrandshop where shopId=" + shop.Id);
            }
        }
        /// <summary>
        /// 删去跟品牌相关的店铺
        /// </summary>
        /// <param name="brand"></param>
        public static void deleteRelationBetShopAndBrand(Brand brand)
        {
            if (brand != null)
            {
                executeSql("delete from tabbrandshop where brandId=" + brand.Id);
            }
        }
        /// <summary>
        /// 删除特定的品牌店铺关联
        /// </summary>
        /// <param name="shop"></param>
        /// <param name="brand"></param>
        public static void deleteRalationBetShopAndBrand(Shop shop)
        {
            if (shop != null && shop.Brand != null)
            {
                executeSql("delete from tabbrandshop where brandId=" + shop.Brand.Id + " and shopId=" + shop.Id);
            }
        }
        /// <summary>
        /// 添加促销活动到商铺
        /// </summary>
        /// <param name="shop"></param>
        /// <param name="salePromotion"></param>
        public static void addSalePromotionToShop(Shop shop, SalePromotion salePromotion)
        {
            if (shop != null && shop.Id > 0 && salePromotion != null && salePromotion.Id > 0)
            {
                string sql = "insert into tabshopsalespromotion(shopId,salesPromotionId) values(" + shop.Id + "," + salePromotion.Id + ")";
                executeSql(sql);
            }
        }
        /// <summary>
        /// 删去商铺的促销活动
        /// </summary>
        /// <param name="shop"></param>
        public static void deleteSalePromotionFromShop(Shop shop)
        {
            if (shop != null)
            {
                executeSql("delete from tabshopsalespromotion where shopId=" + shop.Id);
            }
        }
        /// <summary>
        /// 删去商铺的促销活动
        /// </summary>
        /// <param name="shop"></param>
        public static void deleteSalePromotionFromShop(SalePromotion salePromoiton)
        {
            if (salePromoiton != null)
            {
                executeSql("delete from tabshopsalespromotion where salesPromotionId=" + salePromoiton.Id);
            }
        }
        public static void deleteAdditionalRelation(Shop shop)
        {
            deleteSalePromotionFromShop(shop);
            deleteRelationBetShopAndBrand(shop);
        }

        /// <summary>
        /// 得到店铺的活动信息
        /// </summary>
        /// <param name="shop"></param>
        /// <returns></returns>
        public static List<SalePromotion> getSalePromotionRelatiedWithShop(int shopId)
        {
            string sql = "select salesPromotionId from tabshopsalespromotion where shopId =  " + shopId;
            string salePromotionId = "";
            List<SalePromotion> salePromotions = new List<SalePromotion>();
            DataTable dt = executeQueryDataTable(sql);
            foreach (DataRow row in dt.Rows)
            {
                salePromotionId = row[0].ToString();
                salePromotions.Add(getSalePromotionById(int.Parse(salePromotionId)));
            }

            return salePromotions;
        }


        /// <summary>
        /// 得到店铺的活动信息
        /// </summary>
        /// <param name="shop"></param>
        /// <returns></returns>
        public static List<SalePromotion> getSalePromotionRelatiedWithShop(Shop shop)
        {
            SQLiteDataReader reader = null;
            string sql = "select max(salesPromotionId) from tabshopsalespromotion where shopId =  " + shop.Id;
            string salePromotionId = "";
            List<SalePromotion> salePromotions = new List<SalePromotion>();
            reader = executeQuery(sql);
            while (reader.Read())
            {
                salePromotionId = reader[0].ToString();
                salePromotions.Add(getSalePromotionById(int.Parse(salePromotionId)));
            }
            reader.Close();
            return salePromotions;
        }

        /// <summary>
        /// 得到店铺的品牌信息
        /// </summary>
        /// <returns></returns>
        public static Brand getBrandRelatiedWithShop(int shopId)
        {
            string sql = "select brandId from tabbrandshop where shopId =  " + shopId;
            DataTable dt = executeQueryDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                return getBrandById(int.Parse(dt.Rows[0][0].ToString()));
            }
            return null;
        }

        /// <summary>
        /// 得到店铺的品牌信息
        /// </summary>
        /// <returns></returns>
        public static Brand getBrandRelatiedWithShop(Shop shop)
        {
            SQLiteDataReader reader = null;
            string sql = "select max(brandId) from tabbrandshop where shopId =  " + shop.Id;
            string brandId = "";
            reader = executeQuery(sql);
            while (reader.Read())
            {
                brandId = reader[0].ToString();
            }
            reader.Close();
            return getBrandById(int.Parse(brandId));
        }

        public static Mall getMallById(int id)
        {
            SQLiteDataReader reader = null;
            List<Shop> shops = new List<Shop>();
            try
            {
                string sql = "select id,name,resourceType,imgPath,moviePath from tabmall ";
                reader = executeQuery(sql); ;
                while (reader.Read())
                {
                    return new Mall()
                    {
                        Id = int.Parse(reader["id"].ToString()),
                        Name = reader["name"].ToString(),
                        ResourceType = int.Parse(reader["resourceType"].ToString()),
                        ImgPath = reader["imgPath"].ToString().Split(',').ToList(),
                        MoviePath = reader["moviePath"].ToString()
                    };
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return null;
        }

        /// <summary>
        /// 保存商场信息
        /// </summary>
        /// <param name="mall"></param>
        public static void saveMall(Mall mall)
        {
            string sql = "insert into tabMall(name,resourceType,imgPath,moviePath) values(@name,@resourceType,@imgPaths,@moviePath)";
            executeSql(sql, new SQLiteParameter("@name", mall.Name),
                new SQLiteParameter("@resourceType", mall.ResourceType),
                new SQLiteParameter("@imgPaths", getImgPaths(mall.ImgPath)),
                new SQLiteParameter("@moviePath", mall.MoviePath));
            mall.Id = getNewId();
        }

        public static void saveHotel(Mall mall)
        {
            string sql = "insert into tabhotel(name,resourceType,imgPath,moviePath) values(@name,@resourceType,@imgPaths,@moviePath)";
            executeSql(sql, new SQLiteParameter("@name", mall.Name),
                new SQLiteParameter("@resourceType", mall.ResourceType),
                new SQLiteParameter("@imgPaths", getImgPaths(mall.ImgPath)),
                new SQLiteParameter("@moviePath", mall.MoviePath));
            mall.Id = getNewId();
        }
        public static void saveShopMall(Mall mall)
        {
            string sql = "insert into tabshopmall(name,resourceType,imgPath,moviePath) values(@name,@resourceType,@imgPaths,@moviePath)";
            executeSql(sql, new SQLiteParameter("@name", mall.Name),
                new SQLiteParameter("@resourceType", mall.ResourceType),
                new SQLiteParameter("@imgPaths", getImgPaths(mall.ImgPath)),
                new SQLiteParameter("@moviePath", mall.MoviePath));
            mall.Id = getNewId();
        }
        public static void saveGlobalProject(Mall mall)
        {
            string sql = "insert into tabglobalproject(name,resourceType,imgPath,moviePath) values(@name,@resourceType,@imgPaths,@moviePath)";
            executeSql(sql, new SQLiteParameter("@name", mall.Name),
                new SQLiteParameter("@resourceType", mall.ResourceType),
                new SQLiteParameter("@imgPaths", getImgPaths(mall.ImgPath)),
                new SQLiteParameter("@moviePath", mall.MoviePath));
            mall.Id = getNewId();
        }
        /// <summary>
        /// 取得最新的mall
        /// </summary>
        //public static Mall getMall() {
        //    SQLiteDataReader reader = null;
        //        string sql = "select max(id) from tabmall";
        //        reader = executeQuery(sql);
        //        int id = 0;
        //        while (reader.Read())
        //        {
        //            id=int.Parse(reader[0].ToString());
        //            break;

        //        }
        //       reader.Close();
        //       return getMallById(id);
        //}

        /// <summary>
        /// 取得最新的id
        /// </summary>
        /// <returns></returns>
        public static int getNewId()
        {
            string sql = "select last_insert_rowid()";
            DataTable table = executeQueryDataTable(sql);
            if (table.Rows.Count > 0)
            {
                return int.Parse(table.Rows[0][0].ToString());
            }
            return -1;
        }

        /// <summary>
        /// 更新商场
        /// </summary>
        /// <param name="mall"></param>
        public static void updateMall(Mall mall)
        {
            string sql = "update tabmall set name='" + mall.Name + "',resourceType=" + mall.ResourceType + ",imgPath='" + getImgPaths(mall.ImgPath) + "',moviePath='" + mall.MoviePath + "' where id=@id";
            executeSql(sql, new SQLiteParameter("@id", mall.Id));
        }
        public static void updateHotel(Mall mall)
        {
            string sql = "update tabhotel set name='" + mall.Name + "',resourceType=" + mall.ResourceType + ",imgPath='" + getImgPaths(mall.ImgPath) + "',moviePath='" + mall.MoviePath + "' where id=@id";
            executeSql(sql, new SQLiteParameter("@id", mall.Id));
        }
        public static void updateShopMall(Mall mall)
        {
            string sql = "update tabshopmall set name='" + mall.Name + "',resourceType=" + mall.ResourceType + ",imgPath='" + getImgPaths(mall.ImgPath) + "',moviePath='" + mall.MoviePath + "' where id=@id";
            executeSql(sql, new SQLiteParameter("@id", mall.Id));
        }

        public static void updateGlobalProject(Mall mall)
        {
            string sql = "update tabglobalproject set name='" + mall.Name + "',resourceType=" + mall.ResourceType + ",imgPath='" + getImgPaths(mall.ImgPath) + "',moviePath='" + mall.MoviePath + "' where id=@id";
            executeSql(sql, new SQLiteParameter("@id", mall.Id));
        }

        /// <summary>
        /// 删除商场
        /// </summary>
        /// <param name="mall"></param>
        public static void delMall(Mall mall)
        {
            string sql = "delete from tabmall where id=@id";
            executeSql(sql, new SQLiteParameter("@id", mall.Id));
        }
        public static void delhotel(Mall mall)
        {
            string sql = "delete from tabhotel where id=@id";
            executeSql(sql, new SQLiteParameter("@id", mall.Id));
        }
        public static void delShopMall(Mall mall)
        {
            string sql = "delete from tabshopmall where id=@id";
            executeSql(sql, new SQLiteParameter("@id", mall.Id));
        }
        /// <summary>
        /// 取得商场信息，数据库中商场只有一个
        /// </summary>
        /// <returns></returns>
        public static Mall getMall()
        {
            //openConnection();
            DataTable dt = null;
            string sql = "select id,name,resourceType,imgPath,moviePath from tabmall where id = (select max(id) from tabmall)";
            dt = executeQueryDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new Mall()
                {
                    Id = int.Parse(row["id"].ToString()),
                    Name = row["name"].ToString(),
                    ResourceType = int.Parse(row["resourceType"].ToString()),
                    ImgPath = new List<String>(row["imgPath"].ToString().Split(',')),
                    MoviePath = row["moviePath"].ToString()
                };
            }
            return null;
        }
        public static Mall getShopMall()
        {
            //openConnection();
            DataTable dt = null;
            string sql = "select id,name,resourceType,imgPath,moviePath from tabshopmall where id = (select max(id) from tabmall)";
            dt = executeQueryDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new Mall()
                {
                    Id = int.Parse(row["id"].ToString()),
                    Name = row["name"].ToString(),
                    ResourceType = int.Parse(row["resourceType"].ToString()),
                    ImgPath = new List<String>(row["imgPath"].ToString().Split(',')),
                    MoviePath = row["moviePath"].ToString()
                };
            }
            return null;
        }
        public static Mall getHotel()
        {
            //openConnection();
            DataTable dt = null;
            string sql = "select id,name,resourceType,imgPath,moviePath from tabhotel where id = (select max(id) from tabhotel)";
            dt = executeQueryDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new Mall()
                {
                    Id = int.Parse(row["id"].ToString()),
                    Name = row["name"].ToString(),
                    ResourceType = int.Parse(row["resourceType"].ToString()),
                    ImgPath = new List<String>(row["imgPath"].ToString().Split(',')),
                    MoviePath = row["moviePath"].ToString()
                };
            }
            return null;
        }

        public static Mall getGlobalProject()
        {
            //openConnection();
            DataTable dt = null;
            string sql = "select id,name,resourceType,imgPath,moviePath from tabglobalproject where id = (select max(id) from tabglobalproject)";
            dt = executeQueryDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new Mall()
                {
                    Id = int.Parse(row["id"].ToString()),
                    Name = row["name"].ToString(),
                    ResourceType = int.Parse(row["resourceType"].ToString()),
                    ImgPath = new List<String>(row["imgPath"].ToString().Split(',')),
                    MoviePath = row["moviePath"].ToString()
                };
            }
            return null;
        }

        /// <summary>
        /// 取得全部的分类
        /// </summary>
        /// <returns></returns>
        public static List<Catagory> getAllCatagory()
        {
            string sql = "select id,name,sortChar,logo from tabcatagory";
            DataTable dt = executeQueryDataTable(sql);
            List<Catagory> catagories = new List<Catagory>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                catagories.Add(new Catagory()
                {
                    Id = int.Parse(dt.Rows[i]["id"].ToString()),
                    Name = dt.Rows[i]["name"].ToString(),
                    SortChar = dt.Rows[i]["sortChar"].ToString(),
                    Logo = dt.Rows[i]["logo"].ToString()
                });
            }
            return catagories;
        }
        public static List<string> getAllSortChar()
        {
            List<string> sortChar = new List<string>();
            for (int i = 65; i <= 90; i++)
            {
                sortChar.Add((char)(i) + "");
                sortChar.Add((char)(i + 32) + "");
            }

            for (int i = 0; i < 10; i++)
            {
                sortChar.Add(i + "");
            }
            return sortChar;
        }

        /// <summary>
        /// 拼接字符串数组
        /// </summary>
        /// <param name="imgs"></param>
        /// <returns></returns>
        private static string getImgPaths(string[] imgs)
        {
            string str = "";
            if (imgs != null)
            {
                for (int i = 0; i < imgs.Length; i++)
                {
                    str += imgs[i] + ",";
                }
                if (str.Length > 0)
                {
                    str = str.Substring(0, str.Length - 1);
                }
            }
            return str;
        }
        /// <summary>
        /// 拼接字符串数组
        /// </summary>
        /// <param name="imgs"></param>
        /// <returns></returns>
        private static string getImgPaths(List<string> imgs)
        {
            string str = "";
            if (imgs != null)
            {
                for (int i = 0; i < imgs.Count; i++)
                {
                    str += imgs[i] + ",";
                }
                if (str.Length > 0)
                {
                    str = str.Substring(0, str.Length - 1);
                }
            }
            return str;
        }



        /// <summary>
        /// 得到所有品牌已经入驻的店铺
        /// </summary>
        /// <param name="brand"></param>
        /// <returns></returns>
        public static List<Shop> getAllShopBrandIn(Brand brand)
        {
            string sql = "select shopId from tabbrandshop where brandId = " + brand.Id;
            List<Shop> shops = new List<Shop>();
            DataTable dt = executeQueryDataTable(sql);
            foreach (DataRow row in dt.Rows)
            {
                shops.Add(getShopById(int.Parse(row[0].ToString())));
            }
            return shops;
        }
        /// <summary>
        /// 得到所有去办指定活动的的商铺
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static List<Shop> getAllShopSalePromotionIn(SalePromotion salePromotion)
        {
            string sql = "select shopId from tabshopsalespromotion where salesPromotionId = " + salePromotion.Id;
            List<Shop> shops = new List<Shop>();
            DataTable dt = executeQueryDataTable(sql);
            foreach (DataRow row in dt.Rows)
            {
                shops.Add(getShopById(int.Parse(row[0].ToString())));
            }
            return shops;
        }
        /// <summary>
        /// 替换品牌关联
        /// </summary>
        /// <param name="shop"></param>
        /// <param name="brand"></param>
        public static void updateRelationBetShopAndBrand(Shop shop, Brand brand)
        {
            string sql = "update tabbrandshop set brandId = '" + brand.Id + "' where shopId=" + shop.Id;
            executeSql(sql);
        }
        /// <summary>
        /// 删除促销活动
        /// </summary>
        /// <param name="shop"></param>
        /// <param name="salePromotion"></param>
        public static void deleteSalePromotionFromShop(Shop shop, SalePromotion salePromotion)
        {
            string sql = "delete from tabshopsalespromotion where shopId=" + shop.Id + " and salesPromotionId=" + salePromotion.Id;
            executeSql(sql);
        }
        /// <summary>
        /// 保存图片资源
        /// </summary>
        /// <param name="resource"></param>
        public static int saveImageADResource(ImageADResource resource)
        {
            string sql = "insert into tabImgResource(imgs,resource_type) values('" +
                getImgPaths(resource.Imgs) + "','" +
                resource.Type + "')";
            return executeSql(sql, true);
        }
        /// <summary>
        /// 更新图片资源
        /// </summary>
        /// <param name="resource"></param>
        public static void updateImageADResource(ImageADResource resource)
        {
            string sql = "update tabImgResource set imgs = '" +
                getImgPaths(resource.Imgs) + "' where id=" + resource.Id;
            executeSql(sql);
        }
        /// <summary>
        /// 根据资源类型查找图片资源
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ImageADResource getImageAdResourceByType(int type)
        {
            string sql = "select id,imgs,resource_type from tabImgResource where resource_type=" + type;
            DataTable dt = executeQueryDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                return new ImageADResource()
                {
                    Id = int.Parse(dt.Rows[0]["id"].ToString()),
                    Imgs = dt.Rows[0]["imgs"].ToString().Split(','),
                    Type = type
                };
            }
            return null;
        }
        /// <summary>
        /// 取得机器位置
        /// </summary>
        /// <returns></returns>
        public static Machine getAMachine()
        {
            string sql = "select id,x,y,floorIndex from tabMachine";
            DataTable dt = executeQueryDataTable(sql);
            if (dt.Rows.Count == 1)
            {
                return new Machine()
                {
                    Id = int.Parse(dt.Rows[0]["id"].ToString()),
                    FloorIndex = int.Parse(dt.Rows[0]["floorIndex"].ToString()),
                    X = int.Parse(dt.Rows[0]["x"].ToString()),
                    Y = int.Parse(dt.Rows[0]["y"].ToString())
                };
            }
            return null;
        }
        /// <summary>
        /// 保存机器
        /// </summary>
        /// <param name="machine"></param>
        public static void saveMachine(Machine machine)
        {
            string sql = "insert into tabMachine(floorIndex,x,y) values('" + machine.FloorIndex + "','" + machine.X + "','" + machine.Y + "')";
            executeSql(sql);
        }
        /// <summary>
        /// 更新机器位置
        /// </summary>
        /// <param name="machine"></param>
        public static void updateMachine(Machine machine)
        {
            string sql = "update tabMachine set floorIndex = '" + machine.FloorIndex + "' ,x='" + machine.X + "',y='" + machine.Y + "' where id = " + machine.Id;
            executeSql(sql);
        }
        /// <summary>
        /// 保存报名信息
        /// </summary>
        /// <param name="p"></param>
        /// <param name="salePromotion"></param>
        public static void saveAssignActivitiesInfo(string p, SalePromotion salePromotion)
        {
            string sql = "insert into tabassignactivity(mobileNum,activityName,activityId) values('" + p + "','" + salePromotion.Name + "','" +
                salePromotion.Id + "')";
            executeSql(sql);
        }
        /// <summary>
        /// 活动的统计信息
        /// </summary>
        public static List<SalePromotion> countInfoOfActivity()
        {
            List<SalePromotion> allSalePromotions = new List<SalePromotion>();
            string sql = "select id,name,count(mobileNum) mobileNumC,activityId from tabassignactivity group by activityId";
            DataTable dt = executeQueryDataTable(sql);
            foreach (DataRow row in dt.Rows)
            {
                allSalePromotions.Add(
                    new SalePromotion()
                    {
                        Id = int.Parse(row["activityId"].ToString()),
                        Name = row["name"].ToString(),
                        Count = int.Parse(row["mobileNumC"].ToString())
                    });
            }
            return allSalePromotions;
        }


        /// <summary>
        /// 统计活动的报名人数
        /// </summary>
        /// <param name="currentSalePromotion"></param>
        /// <returns></returns>
        public static int getSignerNumOfActivity(SalePromotion currentSalePromotion)
        {
            string sql = "select count(distinct mobileNum) from tabassignactivity where activityId=" + currentSalePromotion.Id;
            DataTable dt = executeQueryDataTable(sql);
            if (dt.Rows.Count == 1)
            {
                return int.Parse(dt.Rows[0][0].ToString());
            }
            return 0;
        }
        /// <summary>
        /// 取得所有的问题
        /// </summary>
        /// <returns></returns>
        public static List<Question> getAllQuestion()
        {
            string sql = "select id,content,a,b,c,d,e,aNum,bNum,cNum,dNum,eNum from tabquestion";
            DataTable dt = executeQueryDataTable(sql);
            List<Question> questiones = new List<Question>();
            foreach (DataRow row in dt.Rows)
            {
                questiones.Add(new Question()
                {
                    Id = int.Parse(row["id"].ToString()),
                    Content = row["content"].ToString(),
                    ChoiceA = row["a"].ToString(),
                    ChoiceB = row["b"].ToString(),
                    ChoiceC = row["c"].ToString(),
                    ChoiceD = row["d"].ToString(),
                    ChoiceE = row["e"].ToString(),
                    ChoiceACount = int.Parse(row["aNum"].ToString().Equals("") ? "0" : row["aNum"].ToString()),
                    ChoiceBCount = int.Parse(row["bNum"].ToString().Equals("") ? "0" : row["bNum"].ToString()),
                    ChoiceCCount = int.Parse(row["cNum"].ToString().Equals("") ? "0" : row["cNum"].ToString()),
                    ChoiceDCount = int.Parse(row["dNum"].ToString().Equals("") ? "0" : row["dNum"].ToString()),
                    ChoiceECount = int.Parse(row["eNum"].ToString().Equals("") ? "0" : row["eNum"].ToString())
                });
            }
            return questiones;
        }
        /// <summary>
        /// 保存问题
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public static int saveQuestion(Question question)
        {
            string sql = "insert into tabquestion(content,a,b,c,d,e) values('" + question.Content + "','" +
                question.ChoiceA + "','" + question.ChoiceB + "','" + question.ChoiceC + "','" + question.ChoiceD + "','" + question.ChoiceE + "')";
            return executeSql(sql, true);
        }
        /// <summary>
        /// 删除问题
        /// </summary>
        /// <param name="question"></param>
        public static void removeQuestion(Question question)
        {
            string sql = "delete from tabquestion where id = " + question.Id;
            executeSql(sql);
        }

        /// <summary>
        /// 更新问题
        /// </summary>
        /// <param name="question"></param>
        public static void updateQuestion(Question question)
        {
            string sql = "update tabquestion set content='" + question.Content + "',a='" + question.ChoiceA + "',b='" +
                question.ChoiceB + "',c='" + question.ChoiceC + "',d='" + question.ChoiceD + "',e='" +
                question.ChoiceE + "' where id = " + question.Id;
            executeSql(sql);
        }

        /// <summary>
        /// 统计数+1
        /// </summary>
        /// <param name="question"></param>
        public static void setQuestionCount(Question question)
        {
            string sql = "update tabquestion set aNum=" + question.ChoiceACount + ",bNum=" + question.ChoiceBCount + ",cNum=" +
                question.ChoiceCCount + ",dNum=" + question.ChoiceDCount + ",eNum=" + question.ChoiceECount + " where id = " + question.Id;
            executeSql(sql);
        }

        /// <summary>
        /// 加入更新服务端
        /// </summary>
        /// <param name="ip"></param>
        public static void saveUpdateServer(DataUpdateServer server) { 
            string sql = "insert into tabserver(ip,updateTime) values('"+server.IP+"','')";
            executeSql(sql);
        }


        public static void delUpdateServer(string ip){
            string sql ="delete from tabserver where ip="+ip;
            executeSql(sql);
        }

        public static void updateServer(DataUpdateServer server) { 
            string sql = "update tabserver set updateTime='"+server.UpdateTime+"' where ip='"+server.IP+"'";
            executeSql(sql);
        }



        public static List<DataUpdateServer> getAllServer()
        {
            DataTable dt = executeQueryDataTable("select ip,updateTime from tabserver");
            List<DataUpdateServer> rtn = new List<DataUpdateServer>();
            foreach (DataRow row in dt.Rows)
            {
                rtn.Add(new DataUpdateServer() { 
                    IP = row[0].ToString(),
                    UpdateTime = row[1].ToString(),
                    NodeState = ConstantData.SERVER_NODE_NOT_CONNECT+""//默认都不是联通的状态
                });
            }
            return rtn;
        }

        /// <summary>
        /// 添加更新客户端
        /// </summary>
        public static void saveCientHost(ClientComputer client) {
            string sql = "insert into tabclient(ip,username,updateTime,appdir) values('" + client.IP + "','"+client.UserName+"','"+client.UpdateDate+"','"+client.AppDir+"')";
            executeSql(sql);
        }
        public static void delClientHost(String ip) {
            string sql = "delete from tabclient where ip='" + ip+"'";
            executeSql(sql);
        }

        public static void updateClientHost(ClientComputer client) {
            string sql = "update tabclient set updateTime='" + client.UpdateDate + "' and username='" + client.UserName + "' and appdir='"+client.AppDir+"'  where ip='" + client.IP + "'";
            executeSql(sql);
        }
        public static List<ClientComputer> getAllClients() {
            DataTable dt = executeQueryDataTable("select ip,username,updateTime,appdir from tabclient");
            List<ClientComputer> rtn = new List<ClientComputer>();
            foreach (DataRow row in dt.Rows)
            {
                rtn.Add(new ClientComputer()
                {
                    IP = row[0].ToString(),
                    UpdateDate = row[2].ToString(),
                    UserName = row[1].ToString(),
                    AppDir = row[3].ToString(),
                    IsConnected = false//默认不联通
                });
            }
            return rtn;
        }
    }
}
