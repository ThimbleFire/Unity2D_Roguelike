using UnityEngine;
using UnityEngine.EventSystems;

namespace AlwaysEast
{
    public class Inventory : MonoBehaviour, EventSystems.IItemClickHandler, EventSystems.IItemClickOffHandler
    {

        public static bool IsItemSelected = false;

        public RectTransform selectedCell;
        private ItemStats itemBeingSelected;

        public delegate void OnEquipmentChangeHandler(ItemStats itemStats, bool adding);
        public static event OnEquipmentChangeHandler OnEquipmentChange;

        private static TMPro.TextMeshProUGUI characterStatsLabel;
        private static GameObject baseItem;
        private static InventorySlots inventorySlots;
        private static GearSlots gearSlots;

        private void Awake()
        {
            characterStatsLabel = GetComponentInChildren<TMPro.TextMeshProUGUI>();
            baseItem = Resources.Load("Prefabs/BaseItem") as GameObject;
            inventorySlots = GetComponentInChildren<InventorySlots>();
            gearSlots = GetComponentInChildren<GearSlots>();
            gameObject.SetActive(false);
        }

        public void Hide()
        {
            if (IsItemSelected)
            {
                ItemStatBillboard.Hide();
                selectedCell.gameObject.SetActive(false);
                IsItemSelected = false;
            }
        }

        public void OnItemClick(UIItemOnClick t)
        {
            if (t.GetComponent<ItemStats>() != itemBeingSelected)
            {
                IsItemSelected = false;
            }

            // To select the item
            if (IsItemSelected == false)
            {
                IsItemSelected = true;
                itemBeingSelected = t.GetComponent<ItemStats>();
                selectedCell.gameObject.SetActive(true);
                selectedCell.position = itemBeingSelected.GetComponent<RectTransform>().position;
                ItemStatBillboard.Draw(itemBeingSelected);
            }
            else
            {
                ItemStatBillboard.Hide();
                selectedCell.gameObject.SetActive(false);
                IsItemSelected = false;

                if (itemBeingSelected.itemLocation == ItemStats.ItemLocation.EQUIPPED)
                {
                    // remove the item from equipment
                    gearSlots.GetOccupied(itemBeingSelected.ItemType).Unequip();

                    // place the item in the inventory
                    GearSlot emptyInventorySlot = inventorySlots.GetEmpty();
                    emptyInventorySlot.EquipInventory(itemBeingSelected);

                    // tell the player characters stats to change
                    OnGearChange(itemBeingSelected, false);
                }
                else if (itemBeingSelected.itemLocation == ItemStats.ItemLocation.INVENTORY)
                {
                    if (itemBeingSelected.RequirementsMetAll == false)
                        return;

                    GearSlot slot = gearSlots.GetEmpty(itemBeingSelected.ItemType);
                    if (slot == null)
                        return;

                    // remove the item from the inventory
                    itemBeingSelected.gearSlot.Unequip();
                    // we need a way to get the inventory slot that the item belongs to, in order to call unequip (unequip the item from the inventory slot)
                     
                    // place the item in the equipment
                    slot.Equip(itemBeingSelected);
                    OnGearChange(itemBeingSelected, true);
                }

                itemBeingSelected = null;
            }
        }

        public static void OnGearChange(ItemStats itemStats, bool equipping)
        {
            OnEquipmentChange.Invoke(itemStats, equipping);
        }

        public void OnItemClickOff()
        {
            ItemStatBillboard.Hide();
            selectedCell.gameObject.SetActive(false);
            IsItemSelected = false;
        }

        public static void Pickup(string itemName)
        {
            Transform inventorySlotTransform = inventorySlots.GetEmpty().transform;
            ItemStats itemStats = Instantiate(baseItem, inventorySlotTransform).GetComponent<ItemStats>();
            itemStats.Load(itemName);
        }

        public static ItemStats Pickup(ItemProfile itemProfile)
        {
            ItemStats itemStats = Instantiate(baseItem, null).GetComponent<ItemStats>();
            itemStats.Load(itemProfile.ItemPath);
            return itemStats;
        }

        public static void RefreshCharacterStats(Entity playerEntity)
        {
            string hexFire = "<color=#FF0000>";
            string hexIce = "<color=#70D1FF>";
            string hexLightning = "<color=#D3AA3A>";
            string hexPoison = "<color=#34D14B>";
            string hexMana = "<color=#3434D1>";
            string hexLife = "<color=#D13434>";
            string hexEnd = "</color>";

            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            stringBuilder.AppendLine(string.Format("Level: {0}", playerEntity._base.baseStats.Level));
            stringBuilder.AppendLine(string.Format("Experience: {0} / {1}", playerEntity._base.baseStats.Experience, playerEntity._base.baseStats.Experience));
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(string.Format("Life: {0} / {1}", playerEntity._base.baseStats.LifeCurrent, playerEntity.TotalLifeMax));
            stringBuilder.AppendLine(string.Format("Mana: {0} / {1}", playerEntity._base.baseStats.ManaCurrent, playerEntity.TotalManaMax));
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(string.Format("Strength: {0}", playerEntity.TotalStrength));
            stringBuilder.AppendLine(string.Format("Dexterity: {0}", playerEntity.TotalDexterity));
            stringBuilder.AppendLine(string.Format("Intelligence: {0}", playerEntity.TotalIntelligence));
            stringBuilder.AppendLine(string.Format("Constitution: {0}", playerEntity.TotalConstitution));
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("<u>On Attack</u>");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(string.Format("Attack Rating: {0}", playerEntity.TotalAttackRating));
            stringBuilder.AppendLine(string.Format("Attack Damage: {0} - {1}", playerEntity.TotalDmgPhysMin, playerEntity.TotalDmgPhysMax));
            stringBuilder.AppendLine(string.Format("Elemental Dmg: {8}{0} - {1}{12}, {9}{2} - {3}{12}, {10}{4} - {5}{12}, {11}{6} - {7}{12}",
                playerEntity.TotalDmgEleFireMin, playerEntity.TotalDmgEleFireMax,
                playerEntity.TotalDmgEleColdMin, playerEntity.TotalDmgEleColdMax,
                playerEntity.TotalDmgEleLightningMin, playerEntity.TotalDmgEleLightningMax,
                playerEntity.TotalDmgElePoisonMin, playerEntity.TotalDmgElePoisonMax,
                hexFire, hexIce, hexLightning, hexPoison, hexEnd));
            stringBuilder.AppendLine(string.Format("Resources on Hit: {2}{0}{4}, {3}{1}{4}", playerEntity.TotalOnHitLife, playerEntity.TotalOnHitMana, hexLife, hexMana, hexEnd));
            stringBuilder.AppendLine(string.Format("Resources on Kill: {2}{0}{4}, {3}{1}{4}", playerEntity.TotalOnKillLife, playerEntity.TotalOnKillMana, hexLife, hexMana, hexEnd));
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("<u>Defences</u>");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(string.Format("Defence: {0}", playerEntity.TotalDefense));
            stringBuilder.AppendLine(string.Format("Block Chance: {0}", playerEntity.TotalBlockRate));
            stringBuilder.AppendLine(string.Format("Block Recovery: {0}", Entity.BlockRecoveryBase - playerEntity.TotalBlockRecovery));
            stringBuilder.AppendLine(string.Format("Stagger Recovery: {0}", Entity.StaggerRecoveryBase - playerEntity.TotalStaggerRecovery));
            stringBuilder.AppendLine(string.Format("Resistances: {4}{0}{8}, {5}{1}{8}, {6}{2}{8}, {7}{3}{8}",
                playerEntity.TotalDefResFire, playerEntity.TotalDefResCold,
                playerEntity.TotalDefResLightning, playerEntity.TotalDefResPoison,
                hexFire, hexIce, hexLightning, hexPoison, hexEnd));
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("<u>Other</u>");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(string.Format("Movement rating: {0}", playerEntity.TotalSpeed));
            stringBuilder.AppendLine(string.Format("Resource regen: {2}{0}{4}, {3}{1}{4}", playerEntity.TotalRegenLife, playerEntity.TotalRegenMana, hexLife, hexMana, hexEnd));

            characterStatsLabel.text = stringBuilder.ToString();
        }
    }
}

namespace EventSystems
{
    public interface IItemClickHandler : IEventSystemHandler
    {
        void OnItemClick(AlwaysEast.UIItemOnClick t);
    }

    public interface IItemClickOffHandler : IEventSystemHandler
    {
        void OnItemClickOff();
    }
}