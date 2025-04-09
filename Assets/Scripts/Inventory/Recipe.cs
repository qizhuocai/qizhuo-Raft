using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Recipe")]
public class Recipe : ScriptableObject {
    
    [SerializeField] private Item result;
    [SerializeField] private uint amount = 1;
    public List<RecipeIngredient> ingredients = new List<RecipeIngredient>();

    public void Craft() {

        foreach (RecipeIngredient ingredient in ingredients) {

            if (!Inventory.instance.HasItem(ingredient.Item.ID, ingredient.Amount)) return;

        }
        foreach (RecipeIngredient ingredient in ingredients) {

            Inventory.instance.RemoveItem(ingredient.Item.ID, ingredient.Amount);

        }

        Inventory.instance.AddItem(result.ID, amount);

    }

    public string IngredientsToString() {

        string ret = "Ingredients: ";
        foreach (RecipeIngredient ingredient in ingredients) {

            ret += ingredient.Amount.ToString() + "x " + ingredient.Item.Name + ", ";

        }

        return ret;

    }

    public Item Result { get { return result; } }
    public uint Amount { get { return amount; } }

    [Serializable]
    public class RecipeIngredient {

        [SerializeField] private Item item;
        [SerializeField] private uint amount = 1;

        public Item Item { get { return item; } }
        public uint Amount { get { return amount;} }

    }    

}