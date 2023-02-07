using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[XmlRoot("Entity")]
public class EntityReplacement
{
    public EntityBaseStats baseStats;
    public string animationName;
    public int spawnGroupSize;
}
