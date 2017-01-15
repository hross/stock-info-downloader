
namespace StockInfoCommons.Utility
{
    public interface IDownloader
    {
        void Download();

        void Update();

        void Cleanup();
    }
}
