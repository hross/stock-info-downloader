
namespace StockInfoDownloader.Utility
{
    interface IDownloader
    {
        void Download();

        void Update();

        void Cleanup();
    }
}
