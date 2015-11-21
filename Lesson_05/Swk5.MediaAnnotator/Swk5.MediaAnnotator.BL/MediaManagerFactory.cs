
namespace Swk5.MediaAnnotator.BL
{
    public static class MediaManagerFactory
    {
        private static IMediaManager manager;

        public static IMediaManager GetMediaManager()
        {       
            if (manager == null)
                manager = new MediaManagerImpl();
            return manager;
        }
    }
}
