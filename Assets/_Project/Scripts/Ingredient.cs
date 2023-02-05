using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IngredientType
{
    Lopian,
    Rumianek,
    Wildrose,
    Horsetail,
    Nettle
}

public class Ingredient : MonoBehaviour
{
    [SerializeField] private IngredientType _ingredientType;

    public IngredientType IngredientType
    {
        get => _ingredientType;
        set => _ingredientType = value;
    }
}