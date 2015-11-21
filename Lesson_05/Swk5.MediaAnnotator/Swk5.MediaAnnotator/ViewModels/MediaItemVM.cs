using Swk5.MediaAnnotator.BL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Swk5.MediaAnnotator.ViewModels
{
    public class MediaItemVM : ViewModelBase
    {
        private MediaItem mediaItem;

        public ICommand SaveCommand { get; set; }

        public MediaItemVM(MediaItem item, IMediaManager mediaManager)
        {
            this.mediaItem = item;
            this.SaveCommand = new RelayCommand(o => mediaManager.UpdateAnnotation(mediaItem));
        }

        public string Name
        {
            get{
                return this.mediaItem.Name;

            }
        }
        public string Url
        {
            
                get{
                return this.mediaItem.Url;

                }
        }

        public string Annotation
        {
            get
            {
                return this.mediaItem.Annotation;
            }
            set
            {
                if(this.mediaItem.Annotation != value)
                {
                    this.mediaItem.Annotation = value;
                    //RaisePropertyChangedEvent(nameof(Annotation));
                    RaisePropertyChangedEvent();
                }
                
            }
        }
            
    }
}
