using System;
using System.Collections.Generic;

namespace Example_SearchEngine.Manager
{
    internal class IndexManager
    {
        public IndexManager()
        {
            xmlManager = new Manager.XmlManager(Manager.XmlManager.USE_TYPE.READ);
        }

        public void Run()
        {
            List<Model.Contents> contentsList = xmlManager.Read();
            docs = new List<Lucene.Net.Documents.Document>();
            foreach(var contents in contentsList)
            {
                var data = new Lucene.Net.Documents.Document();
                data.Add(new Lucene.Net.Documents.Field("IDX", contents.Idx.ToString(), Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED));
                data.Add(new Lucene.Net.Documents.Field("TITLE", contents.Title, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED));
                data.Add(new Lucene.Net.Documents.Field("SUMMARY", contents.Summary, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED));
                data.Add(new Lucene.Net.Documents.Field("CREATE_DT", contents.CreateDt.ToString(), Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED));
                data.Add(new Lucene.Net.Documents.Field("CREATE_USER_NM", contents.CreateUserNm, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED));
                data.Add(new Lucene.Net.Documents.Field("TAGS", contents.Tags, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.ANALYZED));
                docs.Add(data);
            }
            directory = Lucene.Net.Store.FSDirectory.Open(new System.IO.DirectoryInfo(indexDirectoryPath));
            analyzer = new Lucene.Net.Analysis.Standard.StandardAnalyzer(luceneVersion);
            using (var writer = new Lucene.Net.Index.IndexWriter(directory, analyzer, true, Lucene.Net.Index.IndexWriter.MaxFieldLength.LIMITED))
            {
                foreach(var doc in docs)
                {
                    writer.AddDocument(doc);
                }
                writer.Optimize();
            }
        }
        static readonly char directorySeparator = System.IO.Path.DirectorySeparatorChar;
        static readonly string indexDirectoryPath = Environment.CurrentDirectory + directorySeparator + "indexer";
        Lucene.Net.Util.Version luceneVersion = Lucene.Net.Util.Version.LUCENE_30;
        List<Lucene.Net.Documents.Document> docs;
        Lucene.Net.Store.Directory directory;
        Lucene.Net.Analysis.Analyzer analyzer;
        Manager.XmlManager xmlManager;
    }
}
