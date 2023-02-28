using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("Data")]
public class MapData
{
    public int MapNumber;
    public List<string> EnemyList = new List<string>();
}
