using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeSlot : MonoBehaviour {
    
    [SerializeField] private Recipe recipe;
    [SerializeField] private Image icon;

    public void Setup(Recipe recipe) {

        this.recipe = recipe;
        icon.sprite = recipe.Result.Icon;

    }

    public void ShowDescription() {

        Inventory.instance.ShowRecipeDescription(recipe);

    }

    public Recipe Recipe { get { return recipe;} }

}