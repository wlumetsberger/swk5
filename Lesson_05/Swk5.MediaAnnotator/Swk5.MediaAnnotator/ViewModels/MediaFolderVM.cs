using System.Collections.Generic;
using System.Collections.ObjectModel;
using Swk5.MediaAnnotator.BL;

namespace Swk5.MediaAnnotator.ViewModels
{
    public class MediaFolderVM : ViewModelBase
    {
        private IMediaManager mediaManager;
        private MediaFolder folder;
        private MediaItemVM currentItem;

        public MediaFolderVM(MediaFolder folder, IMediaManager mediaManager)
        {
            this.folder = folder;
            this.mediaManager = mediaManager;
            Items = new ObservableCollection<MediaItemVM>();
            InitPreview();
        }

        public string Name
        {
            get { return folder.Name; }
            set
            {
                if (folder.Name != value)
                {
                    folder.Name = value;
                    RaisePropertyChangedEvent();
                }
            }
        }

        public string Url
        {
            get { return folder.Url; }
            set
            {
                if (folder.Url != value)
                {
                    folder.Url = value;
                    RaisePropertyChangedEvent();
                }
            }
        }

        public int ElementCount
        {
            get { return folder.ElementCount; }
            set
            {
                if (folder.ElementCount != value)
                {
                    folder.ElementCount = value;
                    RaisePropertyChangedEvent();
                }
            }
        }

        public ObservableCollection<MediaItemVM> Items { get; private set; }

        public ObservableCollection<MediaItemVM> PreviewItems { get; private set; }

        public MediaItemVM CurrentItem
        {
            get { return currentItem; }
            set
            {
                if (currentItem != value)
                {
                    currentItem = value;
                    RaisePropertyChangedEvent();
                }
            }
        }

        public void InitPreview()
        {
            PreviewItems = new ObservableCollection<MediaItemVM>();
            int i = 0;
            foreach (MediaItem item in mediaManager.GetMediaItems(folder, Constants.MediaExt))
            {
                if (++i > Constants.MaxPreviewItems)
                    break;
                PreviewItems.Add(new MediaItemVM(item, this.mediaManager));
            }
        }

        public void LoadItems()
        {
            IEnumerable<MediaItem> mediaItems =
               mediaManager.GetMediaItems(folder, Constants.MediaExt);
            foreach (MediaItem item in mediaItems)
            {
                Items.Add(new MediaItemVM(item, this.mediaManager));
            }
        }
    }
}