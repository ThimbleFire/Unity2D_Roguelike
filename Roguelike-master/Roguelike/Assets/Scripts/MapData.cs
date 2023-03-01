using System.Collections.Generic;
using System.Xml.Serialization;

namespace AlwaysEast
{
    [XmlRoot("Data")]
    public class MapData
    {
        public int MapNumber;
        public List<string> EnemyList = new List<string>();
    }
}