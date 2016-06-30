using EntityManagementService.entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;

namespace EntityManagementService.util
{
    public class XmlUtil
    {
        static ReaderWriterLock rwl = new ReaderWriterLock();//互斥的读写锁
        private static string XML_FILE_PATH =Path.Combine(AppDomain.CurrentDomain.BaseDirectory,@"transfer.xml");
        private static ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim();
        private static void createXml()
        {
            File.Create(XML_FILE_PATH);
            XmlDocument doc = loadDoc(XML_FILE_PATH);
            XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "utf-8", "");
            doc.AppendChild(declaration);
            XmlElement rootNode = doc.CreateElement("Clients");
            doc.AppendChild(rootNode);
            saveDoc(doc, XML_FILE_PATH);
        }

        public static ClientComputer createClientComputerFromXmlNode(XmlElement xmlElement)
        {
            XmlNode nodeIp = xmlElement.SelectSingleNode("IP");
            XmlNode nodeTotalFileNum = xmlElement.SelectSingleNode("TotalFileNum");
            XmlNode nodeUpdateFileNum = xmlElement.SelectSingleNode("UpdateFileNum");
            XmlNode nodeState = xmlElement.SelectSingleNode("State");

            if (nodeIp == null || nodeTotalFileNum == null || nodeUpdateFileNum == null || nodeState == null)
            {
                return null;
            }
            return new ClientComputer()
            {
                IP = nodeIp.InnerText,
                TotalFileNum = int.Parse(nodeTotalFileNum.InnerText),
                UpdateFileNum = int.Parse(nodeUpdateFileNum.InnerText),
                State = nodeState.InnerText
            };
        }
        public static XmlElement createXmlNodeFromClientComputer(XmlDocument doc, ClientComputer clientComputer)
        {
            XmlElement client = doc.CreateElement("Client");
            XmlElement ip = doc.CreateElement("IP");
            ip.InnerText = clientComputer.IP;
            XmlElement totalFileNUm = doc.CreateElement("TotalFileNum");
            totalFileNUm.InnerText = clientComputer.TotalFileNum+"";
            XmlElement updateFileNum = doc.CreateElement("UpdateFileNum");
            updateFileNum.InnerText = clientComputer.UpdateFileNum+"";
            XmlElement state = doc.CreateElement("State");
            state.InnerText = clientComputer.State;
            client.AppendChild(ip);
            client.AppendChild(totalFileNUm);
            client.AppendChild(updateFileNum);
            client.AppendChild(state);
            return client;
        }

        public static void clearClient() {
            if (rwLock.TryEnterWriteLock(100))
            {
                try
                {
                    XmlDocument doc = loadDoc(XML_FILE_PATH);
                    XmlElement root = doc.DocumentElement;
                    root.RemoveAll();
                    saveDoc(doc, XML_FILE_PATH);
                }
                finally
                {
                    rwLock.ExitWriteLock();
                }
            }
        }

        public static void writeClientComputer(ClientComputer clientComputer)
        {
            if (rwLock.TryEnterWriteLock(100))
            {
                try
                {
                    XmlDocument doc = loadDoc(XML_FILE_PATH);
                    XmlElement root = doc.DocumentElement;
                    XmlNode node = root.SelectSingleNode("/Clients/Client[IP='" + clientComputer.IP + "']");
                    if (node != null)
                    {
                        root.RemoveChild(node);
                    }
                    root.AppendChild(createXmlNodeFromClientComputer(doc, clientComputer));
                    saveDoc(doc, XML_FILE_PATH);
                }
                finally
                {
                    rwLock.ExitWriteLock();
                }
            }
           
        }
        public static List<ClientComputer> getAllClientComputer()
        {
            if (rwLock.TryEnterReadLock(100))
            {
                try
                {
                    List<ClientComputer> clientComputers = new List<ClientComputer>();
                    //写操作
                    XmlDocument doc = loadDoc(XML_FILE_PATH);
                    if (!File.Exists(XML_FILE_PATH))
                    {
                        createXml();
                        doc = loadDoc(XML_FILE_PATH);
                    }
                    XmlElement root = doc.DocumentElement;
                    XmlNodeList listNodes = root.SelectNodes("/Clients/Client");
                    if (listNodes != null)
                    {
                        foreach (XmlNode node in listNodes)
                        {
                            ClientComputer clientComputer = createClientComputerFromXmlNode((XmlElement)node);
                            if (clientComputer != null)
                            {
                                clientComputers.Add(clientComputer);
                            }
                        }
                    }
                    return clientComputers;
                }
                finally
                {
                    rwLock.ExitReadLock();
                }
            }
            return null;
            
        }
        /// <summary>
        /// 装载doc
        /// </summary>
        /// <returns></returns>
        public static XmlDocument loadDoc(string path)
        {
            if (!File.Exists(path)) return null;
            XmlDocument doc = null;
            using (XmlReader reader = XmlReader.Create(path))
            {
                doc = new XmlDocument();
                doc.Load(reader);
            }
            return doc;

        }

        /// <summary>
        /// 保存文档
        /// </summary>
        /// <param name="doc"></param>
        public static void saveDoc(XmlDocument doc, string filePath)
        {
            using (XmlTextWriter writer = new XmlTextWriter(filePath, null))
            {
                writer.Formatting = Formatting.Indented;
                doc.Save(writer);
            }

        }

    }
}
