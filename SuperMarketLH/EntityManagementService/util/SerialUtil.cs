using EntityManagementService.entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace EntityManagementService.util
{
    public class SerialUtil
    {
        /// <summary>
        /// 序列化地图
        /// </summary>
        /// <param name="map"></param>
        public static void writeMap(Floor floor, Map map)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConstantData.getMapDataFileName(floor));
            string dirPath = Path.GetDirectoryName(path);

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            using (Stream fStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
            {
                BinaryFormatter binFormat = new BinaryFormatter();//创建二进制序列化器
                binFormat.Serialize(fStream, map);
            }

        }
        /// <summary>
        /// 反序列化地图
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Map readMap(Floor floor)
        {
            if (floor.Map == null || floor.Map.Equals("")) {
                return null;
            }
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConstantData.getMapDataFileName(floor));
            if (File.Exists(path))
            {
                using (Stream fStream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
                {
                    BinaryFormatter binFormat = new BinaryFormatter();//创建二进制序列化器
                    return (Map)binFormat.Deserialize(fStream);
                }
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 序列化地图
        /// </summary>
        /// <param name="map"></param>
        public static void writeMachine(Machine machine)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConstantData.getMachineDataFileName());
            string dirPath = Path.GetDirectoryName(path);

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            using (Stream fStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
            {
                BinaryFormatter binFormat = new BinaryFormatter();//创建二进制序列化器
                binFormat.Serialize(fStream, machine);
            }

        }
        /// <summary>
        /// 反序列化地图
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Machine readMachine()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConstantData.getMachineDataFileName());
            if (File.Exists(path))
            {
                using (Stream fStream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
                {
                    BinaryFormatter binFormat = new BinaryFormatter();//创建二进制序列化器
                    return (Machine)binFormat.Deserialize(fStream);
                }
            }
            else
            {
                return null;
            }
        }
    }
}
