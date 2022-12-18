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
    
    //Axis is the direction the arrows run along. For example 3 arrows to the right of one another would be horizontal. 3 arrows underneath eachother would be vertical.
    public enum Axis
    {
        VERTICAL, HORIZONTAL
    };

    [HideInInspector]
    public Dir Direction;
    [HideInInspector]
    //Axis is the direction the arrows run along. For example 3 arrows to the right of one another would be horizontal. 3 arrows underneath eachother would be vertical.
    public Axis axis;
    [HideInInspector]
    public int size = 3;
    
    public AccessPoint Clone()
    {
        var obj = new AccessPoint
        {
            Direction = this.Direction,
            axis = this.axis,
            size = this.size
        };
        
        return obj;
    }

    public static Dir Flip( Dir direction )
    {
        switch ( direction )
        {
            case Dir.RIGHT:
                return Dir.LEFT;
            case Dir.LEFT:
                return Dir.RIGHT;
            case Dir.DOWN:
                return Dir.UP;
            case Dir.UP:
                return Dir.DOWN;
        }

        return Dir.DOWN;
    }
    public static AccessPoint Flip( AccessPoint accessPoint )
    {
        switch ( accessPoint.Direction )
        {
            case Dir.RIGHT:
                accessPoint.Direction = Dir.LEFT;
                return accessPoint;
            case Dir.LEFT:
                accessPoint.Direction = Dir.RIGHT;
                return accessPoint;
            case Dir.DOWN:
                accessPoint.Direction = Dir.UP;
                return accessPoint;
            case Dir.UP:
                accessPoint.Direction = Dir.DOWN;
                return accessPoint;
        }

        return accessPoint;
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
        obj.Height = this.Height;
        obj.Origin = this.Origin;
        obj.Curios = this.Curios;
        obj.Walls = this.Walls;
        obj.Floors = this.Floors;        
        obj.Entrance = new List<AccessPoint>();
        
        foreach(AccessPoint entrance in this.Entrance)
        {
            obj.Entrance.Add(entrance.Clone());
        }
        
        return obj;
    }
    
    [SerializeField]
    public string Name;

    public int Width { get; set; }
    public int Height { get; set; }

    public int radius_x { get { return ( Width - 1 ) / 2; } }
    public int radius_y { get { return ( Height - 1 ) / 2; } }

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
