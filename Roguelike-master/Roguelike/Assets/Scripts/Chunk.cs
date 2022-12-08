using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[System.Serializable]
public class AccessPoint
{
    public enum Dir
    {
        RIGHT, LEFT, DOWN, UP
    };

    public Dir Direction;
    public Vector3Int position;

    [NonSerialized]
    public int Width;
    [NonSerialized]
    public int Height;
}

[XmlRoot("Chunk")]
public class Chunk
{
    public string Name { get; set; }

    public int Width { get; set; }
    public int Height { get; set; }

    [XmlArray( "Curios" )]
    public List<TileData> Curios { get; set; }
    [XmlArray( "Walls" )]
    public List<TileData> Walls { get; set; }
    [XmlArray( "Floors" )]
    public List<TileData> Floors { get; set; }
    [XmlArray( "AccessPoints" )]
    public List<AccessPoint> Entrance { get; set; }
}
