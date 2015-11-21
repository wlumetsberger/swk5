using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swk5.MediaAnnotator.BL;

namespace Swk5.MediaAnnotator.ViewModels
{
    public class MediaFolderCollectionVM : ViewModelBase
    {
        private IMediaManager mediaManager;
        private MediaFolderVM currentFolder;

        public MediaFolderCollectionVM(IMediaManager mediaManager)
        {
            this.mediaManager = mediaManager;
            Folders = new ObservableCollection<MediaFolderVM>();
            LoadFolders();
        }

        public ObservableCollection<MediaFolderVM> Folders { get; set; }

        public MediaFolderVM CurrentFolder
        {
            get { return currentFolder; }
            set
            {
                if (currentFolder != value)
                {
                    currentFolder = value;
                    currentFolder.LoadItems();
                    RaisePropertyChangedEvent("CurrentFolder");
                }
            }
        }

        public void LoadFolders()
        {
            Folders.Clear();
            IEnumerable<MediaFolder> folders = mediaManager.GetMediaFolders(Constants.BaseMediaFolder, Constants.MediaExt);
            foreach (MediaFolder folder in folders)
            {
                Folders.Add(new MediaFolderVM(folder, mediaManager));
            }
        }
    }
}
