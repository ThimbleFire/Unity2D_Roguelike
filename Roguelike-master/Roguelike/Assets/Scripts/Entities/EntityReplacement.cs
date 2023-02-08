using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[XmlRoot("Entity")]
public class EntityReplacement
{
    public EntityBaseStats baseStats;
    public string animatorOverrideControllerFileName;
    [XmlArray("SC On Attack")] public List<string> soundClipFileNamesOnAttack = new List<string>();
    [XmlArray("SC On Hit")] public List<string> soundClipFileNamesOnHit = new List<string>();
    [XmlArray("SC On Death")] public List<string> soundClipFileNamesOnDeath = new List<string>();
    [XmlArray("SC On Aggro")] public List<string> soundClipFileNamesOnAggro = new List<string>();
    [XmlArray("SC On Idle")] public List<string> soundClipFileNamesOnIdle = new List<string>();
    public int spawnGroupSize;
}
