using System.Collections.Generic;

namespace Swk5.MediaAnnotator.BL
{
    public interface IMediaManager
    {
        IEnumerable<MediaFolder> GetMediaFolders(string baseUrl, string[] mediaExtensions);
        MediaFolder GetMediaFolder(string url, string[] mediaExtensions);
        IEnumerable<MediaItem> GetMediaItems(MediaFolder folder, string[] mediaExtensions);
        void UpdateAnnotation(MediaItem mediaElement);
    }
}
