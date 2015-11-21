//#define SLOW

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Swk5.MediaAnnotator.BL
{
    internal class MediaManagerImpl : IMediaManager
    {
        private int CountMediaFiles(DirectoryInfo dir, string[] mediaExtensions)
        {
            int count = 0;

            try
            {
                foreach (FileInfo file in dir.GetFiles())
                    if (Array.Exists(mediaExtensions, ext => ext == file.Extension))
                        count++;
            }
            catch (UnauthorizedAccessException)
            {
            }

            return count;
        }

        private IEnumerable<MediaFolder> FindMediaFolders(DirectoryInfo dir, string[] mediaExtensions)
        {
            int count = CountMediaFiles(dir, mediaExtensions);
            if (count > 0)
                yield return new MediaFolder { Name = dir.Name, Url = dir.FullName, ElementCount = count };

            DirectoryInfo[] subDirs = { };
            try
            {
                subDirs = dir.GetDirectories();
            }
            catch (UnauthorizedAccessException)
            {
            }

            foreach (DirectoryInfo subDir in subDirs)
            {
                foreach (MediaFolder folder in FindMediaFolders(subDir, mediaExtensions))
                {
                    #if SLOW
                    System.Threading.Thread.Sleep(100);
                    #endif

                    yield return folder;
                }
            }
        }

        public MediaFolder GetMediaFolder(string url, string[] mediaExtensions)
        {
            DirectoryInfo dir = new DirectoryInfo(url);
            if (dir.Exists)
            {
                MediaFolder folder = new MediaFolder
                {
                    Name = dir.Name,
                    Url = dir.FullName,
                    ElementCount = CountMediaFiles(dir, mediaExtensions)
                };
                return folder;
            }
            else
                return null;
        }

        public IEnumerable<MediaFolder> GetMediaFolders(string baseUrl,
                                                             string[] mediaExtensions)
        {
            DirectoryInfo dir = new DirectoryInfo(baseUrl);
            if (!dir.Exists)
                throw new InvalidUrlException(baseUrl);

            var result = FindMediaFolders(dir, mediaExtensions);
            return result;
        }

        public IEnumerable<MediaItem> GetMediaItems(MediaFolder folder, string[] mediaExtensions)
        {
            DirectoryInfo dir = new DirectoryInfo(folder.Url);
            if (!dir.Exists)
                throw new InvalidUrlException(folder.Url);

            List<MediaAnnotation> annotations = LoadAnnotations(folder.Url);

            foreach (FileInfo file in dir.GetFiles())
            {
                if (Array.Exists(mediaExtensions, ext => ext.ToLower() == file.Extension.ToLower()))
                {
                    MediaAnnotation ma = annotations.Find(a => a.MediaName == file.Name);
                    string ann = ma == null ? null : ma.Annotation;
                    DateTime ct = ma == null ? file.CreationTime : ma.CreationTime;

                    MediaItem mediaItem = new MediaItem
                    {
                        Name = file.Name,
                        Url = file.FullName,
                        Annotation = ann,
                        CreationTime = ct
                    };

                    #if SLOW
                    System.Threading.Thread.Sleep(100);
                    #endif

                    yield return mediaItem;
                }
            }
        }

        public static void SplitUrl(string url, out string baseUrl, out string localUrl)
        {
            int i;
            for (i = url.Length - 1; i >= 0 && url[i] != '\\'; i--) ;
            if (i < 0)
            {
                baseUrl = "";
                localUrl = url;
            }
            else
            {
                baseUrl = url.Substring(0, i);
                localUrl = url.Length - i - 1 > 0 ? url.Substring(i + 1, url.Length - i - 1) : "";
            }
        }

        private List<MediaAnnotation> LoadAnnotations(string folderName)
        {
            return LoadAnnotations(new FileInfo(folderName + @"\annotations.xml"));
        }

        private List<MediaAnnotation> LoadAnnotations(FileInfo annotationFile)
        {
            if (!annotationFile.Exists)
                return new List<MediaAnnotation>();

            List<MediaAnnotation> annotations = new List<MediaAnnotation>();

            XmlSerializer ser = new XmlSerializer(typeof(List<MediaAnnotation>));
            using (Stream stream = annotationFile.OpenRead())
            {
                return (List<MediaAnnotation>)ser.Deserialize(stream);
            }
        }

        public void UpdateAnnotation(MediaItem media)
        {
            UpdateAnnotations(new List<MediaItem> { media });
        }

        private void UpdateAnnotations(IEnumerable<MediaItem> mediaItems)
        {
            IEnumerator<MediaItem> e = mediaItems.GetEnumerator();
            if (!e.MoveNext())
                return;

            string folderName, fileName;
            SplitUrl(e.Current.Url, out folderName, out fileName);
            FileInfo annotationFile = new FileInfo(folderName + @"\annotations.xml");

            // replace List by a Dictionary later.
            List<MediaAnnotation> annotations = LoadAnnotations(annotationFile);
            foreach (MediaItem media in mediaItems)
            {
                SplitUrl(media.Url, out folderName, out fileName);
                MergeAnnotation(annotations, new MediaAnnotation { MediaName = fileName, Annotation = media.Annotation, CreationTime = media.CreationTime });
            }

            FileStream stream;
            if (!annotationFile.Exists)
            {
                stream = annotationFile.Create();
                // annotationFile.Attributes |= FileAttributes.Hidden;
            }
            else
                stream = annotationFile.Open(FileMode.Create);

            XmlSerializer ser = new XmlSerializer(typeof(List<MediaAnnotation>));
            using (stream)
            {
                ser.Serialize(stream, annotations);
            }
        }

        private static void MergeAnnotation(List<MediaAnnotation> annotationList, MediaAnnotation annotation)
        {
            int aIdx = annotationList.FindIndex(a => a.MediaName == annotation.MediaName);
            if (aIdx >= 0)
                annotationList[aIdx] = annotation;
            else
                annotationList.Add(annotation);
        }
    }
}
