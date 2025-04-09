using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item")]
public class Item : ScriptableObject {
    
    [SerializeField] private string name = "Item";
    [SerializeField] private string description = "";
    [SerializeField] private uint id = 0;
    [SerializeField] private uint stackLimit = 10;

    [Header("Visual")]
    [SerializeField] private Sprite icon;
    [SerializeField] private Mesh mesh;
    [SerializeField] private List<Material> materials = new List<Material>();

    public void Use() {

        Inventory.instance.ShowItemDescription(this);

    }

    public string Name { get { return name; } }
    public string Description { get { return description; } }
    public uint ID { get { return id; } }
    public uint StackLimit { get { return stackLimit;} }
    public Sprite Icon { get { return icon; } }
    public Mesh Mesh { get { return mesh; } }
    public List<Material> Materials { get { return materials; } }

}