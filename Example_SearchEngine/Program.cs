

namespace Example_SearchEngine
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Manager.CrawlManager crawlManager = new Manager.CrawlManager();
            crawlManager.Run();

            Manager.IndexManager indexManager = new Manager.IndexManager();
            indexManager.Run();

            Manager.SearchManager searchManager = new Manager.SearchManager();
            searchManager.Run();
        }
    }
}
