-- Describe TABASSIGNACTIVITY
CREATE TABLE tabassignactivity (
    "id" INTEGER,
    "mobileNum" TEXT,
    "activityName" TEXT
, "activityId" INTEGER)

-- Describe TABBRAND
CREATE TABLE "tabbrand" (
    "id" INTEGER PRIMARY KEY AUTOINCREMENT,
    "name" TEXT NOT NULL,
    "instruction" TEXT,
    "logo" TEXT,
    "sortChar" TEXT,
    "url" TEXT,
    "imgs" TEXT,
    "catagoryId" TEXT
)

-- Describe TABBRANDSHOP
CREATE TABLE "TABBrandShop" (
    "id" INTEGER PRIMARY KEY AUTOINCREMENT,
    "brandId" INTEGER NOT NULL,
    "shopId" INTEGER
)

-- Describe TABCATAGORY
CREATE TABLE "TABCatagory" (
    "id" INTEGER PRIMARY KEY AUTOINCREMENT,
    "name" TEXT,
    "sortChar" TEXT,
    "logo" TEXT
)
--insert into tabcatagory
BEGIN TRANSACTION;


insert into TABCatagory ( "name", "sortChar", "logo") values ( '餐饮', 'C', NULL);
insert into TABCatagory ( "name", "sortChar", "logo") values ( '娱乐', 'Y', NULL);
insert into TABCatagory ( "name", "sortChar", "logo") values ( '购物', 'S', NULL);
insert into TABCatagory ( "name", "sortChar", "logo") values ( '儿童', 'C', NULL);

insert into TABCatagory ( "name", "sortChar", "logo") values ( 'KTV', 'K', NULL);
insert into TABCatagory ( "name", "sortChar", "logo") values ( '健身', 'J', NULL);
insert into TABCatagory ( "name", "sortChar", "logo") values ( '影城', 'Y', NULL); 
insert into TABCatagory ( "name", "sortChar", "logo") values ( '苏宁易购', 'S', NULL);
insert into TABCatagory ( "name", "sortChar", "logo") values ( '大润发超市', 'D', NULL);
insert into TABCatagory ( "name", "sortChar", "logo") values ( '大白鲸世界儿童乐园', 'D', NULL);
COMMIT;




-- Describe TABCLIENT
CREATE TABLE tabclient (
    "ip" TEXT NOT NULL,
    "username" TEXT,
    "appdir" TEXT,
    "updateTime" TEXT
)

-- Describe TABCOMMONBUILDINGS
CREATE TABLE "tabcommonbuildings" (
    "id" INTEGER PRIMARY KEY AUTOINCREMENT,
    "name" TEXT,
    "sortChar" TEXT,
    "logo" TEXT
)

-- Describe TABDBSERVER
CREATE TABLE "tabDBserver" (
    "id" INTEGER PRIMARY KEY AUTOINCREMENT,
    "ip" TEXT,
    "username" TEXT,
    "password" TEXT,
    "used" TEXT
)

-- Describe TABFLOOR
CREATE TABLE tabfloor (
    "id" INTEGER PRIMARY KEY AUTOINCREMENT,
    "indexFloor" INTEGER NOT NULL,
    "name" TEXT NOT NULL,
    "label" TEXT,
    "bg" TEXT NOT NULL,
    "map" TEXT
)

-- Describe TABGLOBALPROJECT
CREATE TABLE "tabglobalproject" (
    "id" INTEGER PRIMARY KEY AUTOINCREMENT,
    "name" TEXT,
    "resourceType" TEXT,
    "imgPath" TEXT,
    "moviePath" TEXT
)

-- Describe TABHOTEL
CREATE TABLE "tabHotel" (
    "id" INTEGER PRIMARY KEY AUTOINCREMENT,
    "name" TEXT,
    "resourceType" TEXT,
    "imgPath" TEXT,
    "moviePath" TEXT
)

-- Describe TABIMGRESOURCE
CREATE TABLE tabimgresource (
    "id" INTEGER PRIMARY KEY AUTOINCREMENT ,
    "resource_type" INTEGER,
    "imgs" TEXT
)

-- Describe TABMACHINE
CREATE TABLE "tabmachine" (
    "id" INTEGER PRIMARY KEY AUTOINCREMENT,
    "floorIndex" INTEGER,
    "x" INTEGER,
    "y" INTEGER
)

-- Describe TABMALL
CREATE TABLE "tabmall" (
    "id" INTEGER PRIMARY KEY AUTOINCREMENT,
    "name" TEXT NOT NULL,
    "resourceType" INTEGER,
    "imgPath" TEXT,
    "moviePath" TEXT
)

-- Describe TABQUESTION
CREATE TABLE tabquestion (
"id" INTEGER PRIMARY KEY AUTOINCREMENT,
    "content" TEXT,
    "a" TEXT,
    "b" TEXT,
    "c" TEXT,
    "d" TEXT,
    "e" TEXT,
    "aNum" INTEGER default 0,
    "bNum" INTEGER default 0,
    "cNum" INTEGER default 0,
    "dNum" INTEGER default 0,
    "eNum" INTEGER  default 0
)

-- Describe TABSALESPROMOTION
CREATE TABLE "TABSalesPromotion" (
    "id" INTEGER PRIMARY KEY AUTOINCREMENT,
    "name" TEXT NOT NULL,
    "introduction" TEXT,
    "startDate" TEXT NOT NULL,
    "endDate" TEXT NOT NULL,
    "range" INTEGER DEFAULT (0),
    "imgs" TEXT
)

-- Describe TABSERVER
CREATE TABLE tabserver (
    "ip" TEXT
, "updateTime" TEXT)

-- Describe TABSHOP
CREATE TABLE tabshop (
    "id" INTEGER PRIMARY KEY AUTOINCREMENT,
    "name" TEXT NOT NULL,
    "floor" INTEGER NOT NULL,
    "positionNum" TEXT NOT NULL,
    "logo" TEXT,
    "label" TEXT NOT NULL,
    "firstNameChar" TEXT NOT NULL,
    "catagoryColor" TEXT,
    "startTime" TEXT,
    "introduction" TEXT,
    "tel" TEXT,
    "address" TEXT,
    "zipCode" TEXT,
    "catagoryId" INTEGER,
    "type" INTEGER,
    "endTime" TEXT,
    "facilities" TEXT,
    "x" INTEGER,
    "y" INTEGER
, "brandImg" TEXT)

-- Describe TABSHOPMALL
CREATE TABLE "tabShopMall" (
    "id" INTEGER PRIMARY KEY AUTOINCREMENT,
    "name" TEXT,
    "resourceType" TEXT,
    "imgPath" TEXT,
    "moviePath" TEXT
)

-- Describe TABSHOPSALESPROMOTION
CREATE TABLE "tabShopSalesPromotion" (
    "id" INTEGER PRIMARY KEY AUTOINCREMENT,
    "shopId" INTEGER,
    "salesPromotionId" INTEGER
)