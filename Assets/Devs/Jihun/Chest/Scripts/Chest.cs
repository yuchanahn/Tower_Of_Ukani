using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Chest : InteractiveObj
{
    //아이템 생성 관련
    [SerializeField] Transform itemSpawnPoint;

    //UI관련
    public Transform chestUI;
    [SerializeField] ChestItemSlot slotPrefab;
    public Transform slotRoot;

    //상자 고유
    List<ChestItemSlot> slots = new List<ChestItemSlot>();
    [HideInInspector] ChestItem selectedItem = null;

    private void Awake()
    {
        if (gameObject.scene.name == "LevelEditor") enabled = false;
    }

    private void Start()
    {
        Init();
    }

    void Init()
    {
        selectedItem = null;
        SetNewItem(new ChestItem(GetItem<MachineGunItem>(), 1, 2), new ChestItem(GetItem<WoodenShortbowItem>()));
    }
    Item GetItem<T>() where T : Item
    {
        return ItemDB.Inst.GetItemPrefab<T>();
    }

    //슬롯 오브젝트를 레이아웃 그룹에 생성하고 프로퍼티 설정
    void SetNewItem(params ChestItem[] item)
    {
        ChestItemSlot slot;

        for(int i = 0; i < item.Length; i++)
        {
            slot = Instantiate<ChestItemSlot>(slotPrefab);

            slots.Add(slot);

            slot.transform.SetParent(slotRoot);
            slot.transform.localScale = Vector3.one;

            slot.SetItem(item[i], this);
        }

    }

    public void SelectItem(ChestItemSlot select)
    {
        foreach (ChestItemSlot slot in slots)
        {
            slot.transform.localScale = Vector3.one;
        }
        select.transform.localScale = Vector3.one * 1.1f;
        selectedItem = select.item;
    }

    public void OpenChest()
    {
        chestUI.gameObject.SetActive(true);
    }

    public void CloseChest()
    {
        chestUI.gameObject.SetActive(false);
    }

    public override InteractiveObj Interact()
    {
        if (chestUI.gameObject.activeSelf)
        {
            CloseChest();
            return null;
        }
        else
        {
            OpenChest();
            return this;
        }
    }


    public void SpawnItem()
    {
        if (selectedItem == null) return;

        UpgradableItem temp = ItemDB.Inst.SpawnDroppedItem(selectedItem.item, itemSpawnPoint.position, selectedItem.count).Item as UpgradableItem;
        temp.AddLevel(selectedItem.level);
        
        isInteractive = false;
        ChestManager.Inst.StopInteract();
        CloseChest();

        gameObject.SetActive(false);
    }
}

[System.Serializable]
public class ChestItem
{
    public Item item;
    public int level;
    public int count;

    public ChestItem() { }
    public ChestItem(Item item, int count = 1, int level = 0)
    {
        this.item = item;
        this.level = level;
        this.count = count;
    }
}