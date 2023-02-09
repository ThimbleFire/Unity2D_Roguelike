using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[XmlRoot("Entity")]
public class EntityReplacement
{
    public EntityReplacement()
    {
        baseStats = new EntityBaseStats();
    }

    public EntityBaseStats baseStats;
    public string animatorOverrideControllerFileName;
    [XmlArray("SCOnAttack")] public List<string> soundClipFileNamesOnAttack = new List<string>();
    [XmlArray("SCOnHit")] public List<string> soundClipFileNamesOnHit = new List<string>();
    [XmlArray("SCOnDeath")] public List<string> soundClipFileNamesOnDeath = new List<string>();
    [XmlArray("SCOnAggro")] public List<string> soundClipFileNamesOnAggro = new List<string>();
    [XmlArray("SCOnIdle")] public List<string> soundClipFileNamesOnIdle = new List<string>();
    public int spawnGroupSize;
}
