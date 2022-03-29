using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Example_SearchEngine.Manager
{
    internal class XmlManager
    {
        public enum USE_TYPE
        {
            WRITE,
            READ,
        }

        public XmlManager(USE_TYPE useType)
        {
            switch(useType)
            {
                case USE_TYPE.WRITE:
                    if(System.IO.Directory.Exists(CrawlDirectoryPath))
                    {
                        System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(CrawlDirectoryPath);
                        foreach(var file in dirInfo.GetFiles())
                        {
                            file.MoveTo(CrawlDirectoryPath + DirectorySeparator + "backup" + DirectorySeparator + file.Name);
                        }
                    }
                    else
                    {
                        System.IO.Directory.CreateDirectory(CrawlDirectoryPath);
                        System.IO.Directory.CreateDirectory(CrawlDirectoryPath + DirectorySeparator + "backup");
                    }
                    xDoc = new XDocument(new XDeclaration("1.0", "utf-8", null), new XElement("result"));
                    break;
                case USE_TYPE.READ:
                    if(System.IO.Directory.Exists(CrawlDirectoryPath))
                    {
                        System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(CrawlDirectoryPath);
                        System.IO.FileInfo[] fileInfos = dirInfo.GetFiles();
                        if(fileInfos.Length > 0)
                        {
                            xDoc = XDocument.Load(fileInfos.First().FullName);
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        public void Write(Model.Contents contents)
        {
            xDoc.Element("result").Add(
                new XElement("row",
                    new XElement("idx", contents.Idx),
                    new XElement("title", new XCData(contents.Title)),
                    new XElement("summary", new XCData(contents.Summary)),
                    new XElement("create_date", contents.CreateDt),
                    new XElement("create_user", new XCData(contents.CreateUserNm)),
                    new XElement("tags", new XCData(contents.Tags))
                ));
        }

        public List<Model.Contents> Read()
        {
            return xDoc.Descendants("row").Select(s => new Model.Contents()
            {
                Idx = Convert.ToInt32(s.Element("idx").Value),
                Title = s.Element("title").Value,
                Summary = s.Element("summary").Value,
                CreateDt = Convert.ToDateTime(s.Element("create_date").Value),
                CreateUserNm = s.Element("create_user").Value,
                Tags = s.Element("tags").Value
            }).ToList();
        }

        public void Save()
        {
            xDoc.Save(CrawlDirectoryPath + DirectorySeparator + DateTime.Now.ToString("yyyyMMddHHmmssff") + ".xml");
        }

        static char DirectorySeparator = System.IO.Path.DirectorySeparatorChar;
        static string CrawlDirectoryPath = Environment.CurrentDirectory + DirectorySeparator + "crawled";
        XDocument xDoc;
    }
}
