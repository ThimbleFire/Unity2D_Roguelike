using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[XmlRoot("Entity")]
public class EntityReplacement
{
    public EntityBaseStats baseStats;
    public string animatorOverrideControllerFileName;
    public List<string> soundClipFileNamesOnAttack;
    public List<string> soundClipFileNamesOnHit;
    public List<string> soundClipFileNamesOnDeath;
    public List<string> soundClipFileNamesOnAggro;
    public List<string> soundClipFileNamesOnIdle;
    public int spawnGroupSize;
}
