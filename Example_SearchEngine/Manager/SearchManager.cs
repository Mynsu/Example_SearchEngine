using System;
using Lucene.Net.QueryParsers;

namespace Example_SearchEngine.Manager
{
    internal class SearchManager
    {
        public SearchManager()
        {
            directory = Lucene.Net.Store.FSDirectory.Open(new System.IO.DirectoryInfo(indexDirectoryPath));
            analyzer = new Lucene.Net.Analysis.Standard.StandardAnalyzer(luceneVersion);
        }

        public void Run()
        {
            Lucene.Net.Index.IndexReader indexReader = Lucene.Net.Index.IndexReader.Open(directory, true);
            Lucene.Net.Search.Searcher indexSearcher = new Lucene.Net.Search.IndexSearcher(indexReader);
            var queryParser = new QueryParser(luceneVersion, "TAGS", analyzer);
            Console.Write("검색어를 입력해주세요 :");
            string query = Console.ReadLine();
            var parsedQuery = queryParser.Parse(query);
            Console.WriteLine("[검색어] {0}", query);
            Lucene.Net.Search.TopDocs resultDocs = indexSearcher.Search(parsedQuery, indexReader.MaxDoc);
            var hits = resultDocs.ScoreDocs;
            int currentRow = 0;
            foreach(var hit in hits)
            {
                var documentFromSearch = indexSearcher.Doc(hit.Doc);
                Console.WriteLine("* Result {0}", ++currentRow);
                Console.WriteLine("\t-제목 : {0}", documentFromSearch.Get("TITLE"));
                Console.WriteLine("\t-내용 : {0}", documentFromSearch.Get("SUMMARY"));
                Console.WriteLine("\t-태그 : {0}", documentFromSearch.Get("TAGS"));
            }
            Console.WriteLine();
        }

        static char directorySeparator = System.IO.Path.DirectorySeparatorChar;
        string indexDirectoryPath = Environment.CurrentDirectory + directorySeparator + "indexer";
        Lucene.Net.Store.Directory directory;
        Lucene.Net.Util.Version luceneVersion = Lucene.Net.Util.Version.LUCENE_30;
        Lucene.Net.Analysis.Analyzer analyzer;
    }
}
