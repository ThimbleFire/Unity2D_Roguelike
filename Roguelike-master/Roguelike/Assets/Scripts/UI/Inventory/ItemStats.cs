using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlwaysEast
{
    public class ItemStats : MonoBehaviour
    {
        public bool Equipped = false;

        private Item item;
        public int QLvl { get { return item.qlvl; } }

        public Item.Type ItemType { get { return item.ItemType; } }
        public List<Item.Implicit> Implicits { get { return item.Implicits; } }
        public List<Item.Prefix> Prefixes { get { return item.Prefixes; } }
        public List<Item.Suffix> Suffixes { get { return item.Suffixes; } }

        public bool RequirementsMetStrength { get { return Entities.GetPCS.TotalStrength >= item.ReqStr; } }
        public bool RequirementsMetDexterity { get { return Entities.GetPCS.TotalDexterity >= item.ReqDex; } }
        public bool RequirementsMetIntelligence { get { return Entities.GetPCS.TotalIntelligence >= item.ReqInt; } }
        public bool RequirementsMetLevel { get { return Entities.GetPCS._base.baseStats.Level >= item.ReqLvl; } }
        public bool RequirementsMetConstitution { get { return Entities.GetPCS.TotalConstitution >= item.ReqCons; } }
        public bool RequirementsMetAll { get { return RequirementsMetStrength && RequirementsMetDexterity && RequirementsMetIntelligence && RequirementsMetLevel && RequirementsMetConstitution; } }

        public byte Rarity { get { return (byte)(item.Prefixes.Count + item.Suffixes.Count); } }

        // We want to reduce the number of tooltip calls by defining it on item creation.

        public bool HasAffix(Enums.StatID affix)
        {
            if (item.Implicits.Find(x => (byte)x.type == (byte)affix) != null) return true;
            if (item.Prefixes.Find(x => (byte)x.type == (byte)affix) != null) return true;
            if (item.Suffixes.Find(x => (byte)x.type == (byte)affix) != null) return true;

            return false;
        }
        public int MinDamage
        {
            get
            {
                Item.Suffix s = item.Suffixes.Find(x => x.type == Item.Suffix.SType.Dmg_Phys_Min);
                Item.Prefix p = item.Prefixes.Find(x => x.type == Item.Prefix.PType.Dmg_Phys_Percent);

                if (s != null && p != null) return (int)((item.DmgMin + s.value) * (p.value / 100.0f + 1));
                if (s != null) return item.DmgMin + s.value;
                if (p != null) return (int)(item.DmgMin * (p.value / 100.0f + 1));

                return item.DmgMin;
            }
        }
        public int MaxDamage
        {
            get
            {
                Item.Suffix s = item.Suffixes.Find(x => x.type == Item.Suffix.SType.Dmg_Phys_Max);
                Item.Prefix p = item.Prefixes.Find(x => x.type == Item.Prefix.PType.Dmg_Phys_Percent);

                if (s != null && p != null) return (int)((item.DmgMax + s.value) * (p.value / 100.0f + 1));
                if (s != null) return item.DmgMax + s.value;
                if (p != null) return (int)(item.DmgMax * (p.value / 100.0f + 1));

                return item.DmgMax;
            }
        }
        public int Defense
        {
            get
            {
                Item.Suffix s = item.Suffixes.Find(x => x.type == Item.Suffix.SType.Def_Phys_Flat);
                Item.Prefix p = item.Prefixes.Find(x => x.type == Item.Prefix.PType.Def_Phys_Percent);

                if (s != null && p != null) return (int)((item.DefMin + s.value) * (p.value / 100.0f + 1));
                if (s != null) return item.DefMin + s.value;
                if (p != null) return (int)(item.DefMin * (p.value / 100.0f + 1));

                return item.DefMin;
            }
        }
        public int Blockrate
        {
            get
            {
                Item.Suffix s = item.Suffixes.Find(x => x.type == Item.Suffix.SType.Plus_Blockrate);
                Item.Implicit i = item.Implicits.Find(x => x.type == Item.Implicit.IType.Plus_Blockrate);

                if (s != null && i != null) return item.Blockrate + s.value + i.value;
                if (s != null) return item.Blockrate + s.value;
                if (i != null) return item.Blockrate + i.value;

                return item.Blockrate;
            }
        }

        public AudioClip soundEndDrag;

        public string Tooltip
        {
            get
            {
                System.Text.StringBuilder t = new System.Text.StringBuilder(
                    item.Unique ? Helper.hexUnique : Helper.hexWhite);
                t.Append(item.Name);
                t.Append(Helper.hexEnd);

                t.AppendLine(Helper.ItemTypeNames[(byte)item.ItemType]);

                if (item.DmgMin > 0)        t.AppendLine(Helper.hexGray + Helper.LBL_ONE_HAND_DAMAGE + Helper.hexEnd + Helper.hexMagic + MinDamage + " to " + MaxDamage + Helper.hexEnd);
                if (item.Blockrate > 0)     t.AppendLine(Helper.hexGray + Helper.LBL_CHANCE_TO_BLOCK + Helper.hexEnd + Helper.hexMagic + Blockrate + Helper.hexEnd);
                if (item.DefMin > 0)        t.AppendLine(Helper.hexGray + Helper.LBL_DEFENSE + Helper.hexEnd + Helper.hexMagic + Defense + Helper.hexEnd);
                if (item.Durability > 0)    t.AppendLine(Helper.hexGray + Helper.LBL_DURABILITY + Helper.hexEnd + item.Durability);
                if (item.ReqStr > 0)        t.AppendLine(string.Format(Helper.LBL_REQUIRED_STRENGTH, item.ReqStr, Helper.hexEnd, RequirementsMetStrength ? Helper.hexGray : Helper.hexRed));
                if (item.ReqDex > 0)        t.AppendLine(string.Format(Helper.LBL_REQUIRED_DEXTERITY, item.ReqDex, Helper.hexEnd, RequirementsMetStrength ? Helper.hexGray : Helper.hexRed));
                if (item.ReqInt > 0)        t.AppendLine(string.Format(Helper.LBL_REQUIRED_INTELLIGENCE, item.ReqInt, Helper.hexEnd, RequirementsMetStrength ? Helper.hexGray : Helper.hexRed));                
                if (item.ReqCons > 0)       t.AppendLine(string.Format(Helper.LBL_REQUIRED_CONSTITUTION, item.ReqCons, Helper.hexEnd, RequirementsMetStrength ? Helper.hexGray : Helper.hexRed));
                if (item.ReqLvl > 0)        t.AppendLine(string.Format(Helper.LBL_REQUIRED_LEVEL, item.ReqLvl, Helper.hexEnd, RequirementsMetStrength ? Helper.hexGray : Helper.hexRed));
                if (Rarity > 0)             t.AppendLine();

                item.Implicits.ForEach(p => t.AppendLine(Helper.hexMagic + string.Format(Item.Affix_Text[(byte)p.type], p.value) + Helper.hexEnd));
                item.Prefixes.ForEach(p => t.AppendLine(Helper.hexMagic + string.Format(Item.Affix_Text[(byte)p.type], p.value) + Helper.hexEnd));
                item.Suffixes.ForEach(p => t.AppendLine(Helper.hexMagic + string.Format(Item.Affix_Text[(byte)p.type], p.value) + Helper.hexEnd));

                if (item.Description != string.Empty)
                    t.AppendLine().AppendLine("<i>" + item.Description + "</i>");

                return t.ToString();
            }
        }

        public void Load(string itemName)
        {
            item = XMLUtility.Load<Item>("Items/" + itemName);
            GetComponent<Image>().sprite = Resources.Load<Sprite>(item.SpriteUIFilename);
        }
    }
}