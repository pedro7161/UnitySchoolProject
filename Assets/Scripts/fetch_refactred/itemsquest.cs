using System.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemsQuestStructore
{

     public Items Item = new Items();
    public int AmountRequired;
    public int CurrentAmount;
    
}
public class itemsquest : MonoBehaviour
{
    public List<ItemsQuestStructore> AllquestItems;

   void Start() {
        if (AllquestItems != null)
        {
            questionmanager.AllquestItems = AllquestItems;
        }
   }
}

