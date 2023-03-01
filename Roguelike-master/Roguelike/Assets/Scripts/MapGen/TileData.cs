using System.Xml.Serialization;
using UnityEngine;

namespace AlwaysEast
{
    [XmlRoot("Droprate")]
    public class TileData
    {
        public Vector3Int position;
        public string name;
    }
}