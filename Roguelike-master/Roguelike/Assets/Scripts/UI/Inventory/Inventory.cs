using UnityEngine;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour, IItemClickHandler, IItemClickOffHandler {

    public static bool IsItemSelected = false;

    public RectTransform selectedCell;
    private ItemStats itemBeingSelected;

    public delegate void OnEquipmentChangeHandler( ItemStats itemStats, bool adding );
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

    public void Hide() {
        if ( IsItemSelected ) {
            ItemStatBillboard.Hide();
            selectedCell.gameObject.SetActive( false );
            IsItemSelected = false;
        }
    }

    public void OnItemClick( UIItemOnClick t ) {

        if(t.GetComponent<ItemStats>() != itemBeingSelected)
        {
            IsItemSelected = false;
        }

        // To select the item
        if ( IsItemSelected == false ) 
        {
            IsItemSelected = true;
            itemBeingSelected = t.GetComponent<ItemStats>();
            selectedCell.gameObject.SetActive( true );
            selectedCell.position = itemBeingSelected.GetComponent<RectTransform>().position;
            ItemStatBillboard.Draw( itemBeingSelected );
        }
        else 
        {
            ItemStatBillboard.Hide();
            selectedCell.gameObject.SetActive( false );
            IsItemSelected = false;

            if (itemBeingSelected.Equipped == true)
            {
                Transform emptyInventorySlotTransform = inventorySlots.GetEmpty().Unequip();
                itemBeingSelected.transform.SetParent(emptyInventorySlotTransform);
                itemBeingSelected.Equipped = false;
                OnEquipmentChange.Invoke(itemBeingSelected, false);
            }
            else if (itemBeingSelected.Equipped == false)
            {
                if (itemBeingSelected.RequirementsMetAll == false)
                    return;

                GearSlot slot = gearSlots.GetEmpty(itemBeingSelected.ItemType);
                if (slot != null)
                {
                    slot.Equip(itemBeingSelected);
                    OnEquipmentChange.Invoke(itemBeingSelected, true);
                }
            }

            itemBeingSelected = null;
        }
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
        stringBuilder.AppendLine(string.Format("Level: {0}", playerEntity.Level));
        stringBuilder.AppendLine(string.Format("Experience: {0} / {1}", playerEntity.Experience_Current, playerEntity.Experience_Max));
        stringBuilder.AppendLine();
        stringBuilder.AppendLine(string.Format("Life: {0} / {1}", playerEntity.Life_Current, playerEntity.Life_Max));
        stringBuilder.AppendLine(string.Format("Mana: {0} / {1}", playerEntity.Mana_Current, playerEntity.Mana_Max));
        stringBuilder.AppendLine();
        stringBuilder.AppendLine(string.Format("Strength: {0}", playerEntity.Strength));
        stringBuilder.AppendLine(string.Format("Dexterity: {0}", playerEntity.Dexterity));
        stringBuilder.AppendLine(string.Format("Intelligence: {0}", playerEntity.Intelligence));
        stringBuilder.AppendLine(string.Format("Constitution: {0}", playerEntity.Constitution));
        stringBuilder.AppendLine();
        stringBuilder.AppendLine("<u>On Attack</u>");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine(string.Format("Attack Rating: {0}", playerEntity.AttackRating));
        stringBuilder.AppendLine(string.Format("Attack Damage: {0} - {1}", playerEntity.DmgPhysMin, playerEntity.DmgPhysMax));
        stringBuilder.AppendLine(string.Format("Elemental Dmg: {8}{0} - {1}{12}, {9}{2} - {3}{12}, {10}{4} - {5}{12}, {11}{6} - {7}{12}",
            playerEntity.DmgEleFireMin, playerEntity.DmgEleFireMax,
            playerEntity.DmgEleColdMin, playerEntity.DmgEleColdMax,
            playerEntity.DmgEleLightningMin, playerEntity.DmgEleLightningMax,
            playerEntity.DmgElePoisonMin, playerEntity.DmgElePoisonMax,
            hexFire, hexIce, hexLightning, hexPoison, hexEnd));
        stringBuilder.AppendLine(string.Format("Resources on Hit: {2}{0}{4}, {3}{1}{4}", playerEntity.OnHitLife, playerEntity.OnHitMana, hexLife, hexMana, hexEnd));
        stringBuilder.AppendLine(string.Format("Resources on Kill: {2}{0}{4}, {3}{1}{4}", playerEntity.OnKillLife, playerEntity.OnKillMana, hexLife, hexMana, hexEnd));
        stringBuilder.AppendLine();
        stringBuilder.AppendLine("<u>Defences</u>");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine(string.Format("Defence: {0}", playerEntity.Defense));
        stringBuilder.AppendLine(string.Format("Defence Rating: {0}", playerEntity.DefenseRating));
        stringBuilder.AppendLine(string.Format("Block Chance: {0}", playerEntity.ChanceToBlock));
        stringBuilder.AppendLine(string.Format("Block Recovery: {0}", Entity.BlockRecoveryBase - playerEntity.IncBlockRecovery));
        stringBuilder.AppendLine(string.Format("Stagger Recovery: {0}", Entity.StaggerRecoveryBase - playerEntity.IncStaggerRecovery));
        stringBuilder.AppendLine(string.Format("Resistances: {4}{0}{8}, {5}{1}{8}, {6}{2}{8}, {7}{3}{8}",
            playerEntity.DefResFire, playerEntity.DefResCold, 
            playerEntity.DefResLightning, playerEntity.DefResPoison,
            hexFire, hexIce, hexLightning, hexPoison, hexEnd));
        stringBuilder.AppendLine();
        stringBuilder.AppendLine("<u>Other</u>");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine(string.Format("Movement rating: {0}", playerEntity.Speed));
        stringBuilder.AppendLine(string.Format("Resource regen: {2}{0}{4}, {3}{1}{4}", playerEntity.RegenLife, playerEntity.RegenMana, hexLife, hexMana, hexEnd));

        characterStatsLabel.text = stringBuilder.ToString();
    }
}

namespace UnityEngine.EventSystems {

    public interface IItemClickHandler : IEventSystemHandler {

        void OnItemClick( UIItemOnClick t );
    }

    public interface IItemClickOffHandler :IEventSystemHandler
    {
        void OnItemClickOff();
    }
}