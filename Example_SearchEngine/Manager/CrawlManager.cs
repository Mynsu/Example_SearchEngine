using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using WebExample.Manager;
using WebExample.Model;

namespace Example_SearchEngine.Manager
{
    internal class CrawlManager
    {
        public CrawlManager()
        {
            SetDatabaseInfo();
            SetMsSqlManager();
        }

        public void Run()
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            List<Model.Contents> contents = Scan();
            WriteToDocs(contents);
            stopwatch.Stop();
            Console.WriteLine("총 " + contents.Count + "건 수집 / 소요시간: " + stopwatch.Elapsed.ToString());
        }

        private List<Model.Contents> Scan()
        {
            msSql.Open(dbInfo);
            DataTable dataTable
                = msSql.Select("SELECT IDX, TITLE, SUMMARY, CREATE_DT, CREATE_USER_NM, TAGS, LIKE_CNT, CATEGORY_IDX FROM TB_CONTENTS");
            var contents = from dataRow in dataTable.AsEnumerable()
                           select new Model.Contents()
                           {
                               Idx = Convert.ToInt32(dataRow["IDX"]),
                               Title = dataRow["TITLE"].ToString(),
                               Summary = dataRow["SUMMARY"].ToString(),
                               CreateDt = Convert.ToDateTime(dataRow["CREATE_DT"]),
                               CreateUserNm = dataRow["CREATE_USER_NM"].ToString(),
                               Tags = dataRow["TAGS"].ToString()
                           };
            msSql.Close();
            return contents.ToList();
        }

        private void WriteToDocs(List<Model.Contents> contents)
        {
            XmlManager xmlManager = new XmlManager(XmlManager.USE_TYPE.WRITE);
            contents.ForEach(c =>
            {
                xmlManager.Write(c);
            });
            xmlManager.Save();
        }

        private void SetDatabaseInfo()
        {
            dbInfo = new DatabaseInfo();
            dbInfo.Name = "RoadbookDB";
            dbInfo.Ip = "127.0.0.1";
            dbInfo.Port = 1433;
            dbInfo.UserId = "sa";
            dbInfo.UserPassword = "sasa";
        }

        private void SetMsSqlManager()
        {
            msSql = new MsSqlManager();
        }

        DatabaseInfo dbInfo;
        MsSqlManager msSql;
    }
}
