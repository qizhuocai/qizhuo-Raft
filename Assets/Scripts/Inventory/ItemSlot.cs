using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour {
    
    [SerializeField] private Item item;
    [SerializeField] private uint amount = 0;

    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text amountTxt;

    public void AddItem(Item item, uint amount) {

        this.item = item;
        this.amount += amount;

        icon.sprite = this.item.Icon;
        amountTxt.text = this.amount.ToString();
        
        if (icon.gameObject.activeSelf) return;
        icon.gameObject.SetActive(true);

    }
    public void RemoveItem(Item item, uint amount) {

        this.amount -= amount;
        if (this.amount < 1) {

            this.item = null;
            this.amount = 0;
            icon.gameObject.SetActive(false);

        }
        amountTxt.text = this.amount.ToString();

    }

    public void Use() {

        item.Use();

    }

    public Item Item { get { return item;} }
    public uint Amount { get { return amount;} }

}