using System;
using System.Xml.Serialization;

namespace Swk5.MediaAnnotator.BL
{
    [Serializable]
    public class MediaAnnotation
    {
        [XmlAttribute]
        public string MediaName { get; set; }
        public string Annotation { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
