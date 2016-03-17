using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityManagementService.entity
{
    /// <summary>
    /// 一些图片资源，用来在客户端进行展示
    /// 1，消防通道
    /// 2，电子杂志
    /// 3，招商广告
    /// 4，招聘信息
    /// 5，周边信息
    /// </summary>
    public class ImageADResource
    {
        private int _id;

        public int Id
        {
          get { return _id; }
          set { _id = value; }
        }
        private string[] _imgs;

        public string[] Imgs
        {
          get { return _imgs; }
          set { _imgs = value; }
        }
        private int _type;
        public int Type
        {
          get { return _type; }
          set { _type = value; }
        }
    }
}
