using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemsStructore
{

    public GameObject Item;
    public string Item_Name;
    public string Item_Description;
    
}
public class Items : MonoBehaviour
{
   public List<ItemsStructore> AllItems;

   void Start() {
        if (AllItems != null)
        {
            questionmanager.allItems = AllItems;
        }
   }
} 
