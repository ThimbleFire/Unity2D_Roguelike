using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[XmlRoot("Entity")]
public class EntityReplacement
{
    public EntityBaseStats baseStats;
    public string animatorOverrideControllerFileName;
    public List<string> soundClipFileNamesOnAttack = new List<string>();
    public List<string> soundClipFileNamesOnHit = new List<string>();
    public List<string> soundClipFileNamesOnDeath = new List<string>();
    public List<string> soundClipFileNamesOnAggro = new List<string>();
    public List<string> soundClipFileNamesOnIdle = new List<string>();
    public int spawnGroupSize;
}
