using System.Xml;
using System.Xml.Serialization;

namespace WDSSRP
{
    public class DesktopProfile
    {
        [XmlElementAttribute(IsNullable = true)]
        public int? FFlags { get; set; }
        [XmlElementAttribute(IsNullable = true)]
        public int? GroupByDirection { get; set; }
        [XmlAttribute]
        public string GroupByKey_FMTID { get; set; }
        [XmlElementAttribute(IsNullable = true)]
        public int? GroupByKey_PID { get; set; }
        [XmlElementAttribute(IsNullable = true)]
        public int? GroupView { get; set; }
        [XmlArrayAttribute]
        public byte[] IconLayouts { get; set; }
        [XmlElementAttribute(IsNullable = true)]
        public int? IconSize { get; set; }
        [XmlElementAttribute(IsNullable = true)]
        public int? LogicalViewMode { get; set; }
        [XmlElementAttribute(IsNullable = true)]
        public int? Mode { get; set; }
        [XmlArrayAttribute]
        public byte[] Sort { get; set; }
    }
}
