using System.Xml.Serialization;
using UnityEngine;

[XmlRoot( "Droprate" )]
public class TileData
{
    public Vector3Int position;
    public string name;
}