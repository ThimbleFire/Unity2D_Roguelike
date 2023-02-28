using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[XmlRoot("Entity")]
public class EntityReplacement : ICloneable
{
    public EntityReplacement()
    {
        baseStats = new EntityBaseStats();
    }

    [SerializeField]
    public EntityBaseStats baseStats;
    public string animatorOverrideControllerFileName;
    [XmlArray("SCOnAttack")] public List<string> soundClipFileNamesOnAttack;
    [XmlArray("SCOnHit")] public List<string> soundClipFileNamesOnHit;
    [XmlArray("SCOnDeath")] public List<string> soundClipFileNamesOnDeath;
    [XmlArray("SCOnAggro")] public List<string> soundClipFileNamesOnAggro;
    [XmlArray("SCOnIdle")] public List<string> soundClipFileNamesOnIdle;

    public int spawnGroupSize_min;
    public int spawnGroupSize_max;

    public object Clone()
    {
        return new EntityReplacement()
        {
            baseStats = (EntityBaseStats)baseStats.Clone(),
            animatorOverrideControllerFileName = animatorOverrideControllerFileName,
            soundClipFileNamesOnAttack = soundClipFileNamesOnAttack,
            soundClipFileNamesOnHit = soundClipFileNamesOnHit,
            soundClipFileNamesOnDeath = soundClipFileNamesOnDeath,
            soundClipFileNamesOnAggro = soundClipFileNamesOnAggro,
            soundClipFileNamesOnIdle = soundClipFileNamesOnIdle,
            spawnGroupSize_min = spawnGroupSize_min,
            spawnGroupSize_max = spawnGroupSize_max,
        };
    }
}
