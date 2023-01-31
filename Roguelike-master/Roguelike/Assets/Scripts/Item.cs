using System.Collections.Generic;

public class Item
{
    public string Name;
    public ItemStats.Type ItemType;
    public UnityEngine.Sprite SpriteUI;
    public string SpriteUIFilename;
    public UnityEngine.AnimatorOverrideController animatorOverrideController;
    public int ItemLevel;
    public int DmgMin, DmgMax;
    public int DefMin, DefMax;
    public int Blockrate;
    public List<ItemStats.Implicit> Implicits;
    public bool Unique;
    public List<ItemStats.Prefix> Prefixes;
    public List<ItemStats.Suffix> Suffixes;
    public string Description;
    public int ReqStr, ReqInt, ReqDex, ReqCons, ReqLvl;
}
