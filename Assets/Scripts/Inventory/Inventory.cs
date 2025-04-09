using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public GameObject inventoryUI;
    public Image itemIcon;
    public TMP_Text itemName;
    public TMP_Text itemDescription;
    public GameObject itemDescriptionPanel;
    public GameObject craftButton;

    public List<ItemSlot> slots = new List<ItemSlot>();
    public Dictionary<uint, uint> items = new Dictionary<uint, uint>();
    public List<int> emptySlots = new List<int>();

    private Recipe currentRecipe;

    void Awake()
    {
        instance = this;
    }

    public void AddItem(Item item, uint amount = 1)
    {
        AddItem(item.ID, amount);
    }

    public void AddItem(uint itemID, uint amount = 1)
    {
        if (items.ContainsKey(itemID))
        {
            items[itemID] += amount;
        }
        else
        {
            items[itemID] = amount;
        }

        // �ҵ�һ���ղ���������Ʒ
        if (emptySlots.Count > 0)
        {
            int slotIndex = emptySlots[0];
            ItemSlot slot = slots[slotIndex];
            Item item = ItemFromID(itemID);
            slot.AddItem(item, amount);
            emptySlots.RemoveAt(0);
        }
    }

    public void RemoveItem(Item item, uint amount = 1)
    {
        RemoveItem(item.ID, amount);
    }

    public void RemoveItem(uint itemID, uint amount = 1)
    {
        if (!HasItem(itemID, amount)) return;

        uint amountToRemove = amount;
        for (int i = 0; i < slots.Count; i++)
        {
            ItemSlot slot = slots[i];
            if (slot.Item == null || slot.Item.ID != itemID) continue;

            // ��������λ����Ʒ�����㹻�Ƴ��������������Ƴ�
            if (slot.Amount > amountToRemove)
            {
                slot.RemoveItem(slot.Item, amountToRemove);
                amountToRemove = 0;
                break;
            }

            // �������������������λ������������һ��
            amountToRemove -= slot.Amount;
            slot.RemoveItem(slot.Item, slot.Amount);
            emptySlots.Add(i);
        }
        emptySlots.Sort();

        items[itemID] -= amount;
    }

    public bool HasItem(Item item, uint amount = 1)
    {
        return HasItem(item.ID, amount);
    }

    public bool HasItem(uint itemID, uint amount = 1)
    {
        if (!items.ContainsKey(itemID)) return false;
        if (items[itemID] < amount) return false;

        return true;
    }

    public void ShowItemDescription(Item item)
    {
        itemIcon.sprite = item.Icon;
        itemName.text = item.Name;
        itemDescription.text = item.Description;

        itemDescriptionPanel.SetActive(true);
        craftButton.SetActive(false);
    }

    public void ShowRecipeDescription(Recipe recipe)
    {
        currentRecipe = recipe;
        itemIcon.sprite = recipe.Result.Icon;
        itemName.text = recipe.Result.Name;
        itemDescription.text = recipe.Result.Description + "\n\n" + recipe.IngredientsToString();

        itemDescriptionPanel.SetActive(true);
        craftButton.SetActive(true);
    }

    public void CraftCurrentRecipe()
    {
        currentRecipe.Craft();
    }

    private void OpenInventory()
    {
        bool open = !inventoryUI.activeSelf;
        inventoryUI.SetActive(open);
        Cursor.lockState = open ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = open;

        Player.instance.gamePaused = open;

        itemDescriptionPanel.SetActive(false);
        craftButton.SetActive(false);
        currentRecipe = null;
    }

    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Tab)) return;

        OpenInventory();
    }

    public Item ItemFromID(uint itemID)
    {
        // �����������һ���洢������Ʒ���б� allItems
        // ����Ҫ����ʵ�����ʵ���������
        return null;
    }
}

