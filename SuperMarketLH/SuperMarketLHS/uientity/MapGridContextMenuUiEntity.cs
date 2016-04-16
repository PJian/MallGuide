using EntityManagementService.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace SuperMarketLHS.uientity
{
    class MapGridContextMenuUiEntity
    {
        public Visibility ShowMenuItemOfAddShop { get; set; }
        public Visibility ShowMenuItemOfShopIn { get; set; }
        public Visibility ShowMenuItemOfShopOut { get; set; }

        public Visibility DelArea { get; set; }
        public String CurrentObstacleIndex { get; set; }
    }
}
