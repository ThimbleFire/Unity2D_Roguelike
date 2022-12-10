using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[Serializable]
public class AccessPoint
{
    public enum Dir
    {
        RIGHT, LEFT, DOWN, UP
    };

    [HideInInspector]
    public Dir Direction;
    [HideInInspector]
    public Vector3Int position;
    
    public AccessPoint Clone()
    {
        var obj = new AccessPoint
        {
            Direction = this.Direction,
            position = this.position
        };
        
        return obj;
    }
}

[XmlRoot("Chunk")]
[Serializable]
public class Chunk
{
    public Chunk Clone()
    {
        var obj = new Chunk();
        
        obj.Name = this.Name;
        obj.Width = this.Width;
        obj.Origin = this.Origin;
        obj.Curios = this.Curios;
        obj.Walls = this.Walls;
        obj.Floors = this.Floors;        
        obj.Entrance = new List<AccessPoint>();
        
        foreach(AccessPoint entrance in this.Entrance)
        {
            obj.Entrance.Add(entrance.Clone());
        }
    }
    
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
