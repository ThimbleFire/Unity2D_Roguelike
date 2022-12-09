using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[Serializable]
public struct AccessPoint
{
    public enum Dir
    {
        RIGHT, LEFT, DOWN, UP
    };

    [HideInInspector]
    public Dir Direction;
    [HideInInspector]
    public Vector3Int position;
}

[XmlRoot("Chunk")]
[Serializable]
public struct Chunk
{
    [SerializeField]
    public string Name;

    public int Width { get; set; }
    public int Height { get; set; }

    [NonSerialized]
    public Vector3Int Origin;

    [XmlArray( "Curios" )]
    public List<TileData> Curios { get; set; }
    [XmlArray( "Walls" )]
    public List<TileData> Walls { get; set; }
    [XmlArray( "Floors" )]
    public List<TileData> Floors { get; set; }
    [XmlArray( "AccessPoints" )]
    [SerializeField]
    public List<AccessPoint> Entrance;
}
