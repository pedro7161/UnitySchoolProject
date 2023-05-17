using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class questionmanager : MonoBehaviour
{
    public Dictionary<string, string> AllQuests = new Dictionary<string, string>();
    private static quest CurrentQuest = new quest();
    public static List<ItemsStructore> allItems = new List<ItemsStructore>();
    public static List<QuestStructure> AllItemsNeeded = new List<QuestStructure>();
   
    public static List<ItemsQuestStructore> AllquestItems = new List<ItemsQuestStructore>();

    

    // Start is called before the first frame update
    void Start()
    {

        foreach (var item in AllquestItems)
        {
            if (item.CurrentAmount >= item.AmountRequired)
            {
                CurrentQuest.Isfinished = true;
            }
        }

        if (CurrentQuest.Isfinished)
        {
            AllQuests.Add(CurrentQuest.Quest_name, CurrentQuest.Quest_description + "Finished");
        }
        else
        {
            AllQuests.Add(CurrentQuest.Quest_name, CurrentQuest.Quest_description + "UnFinished");
        }

        
    }

    // Update is called once per frame
    void Update()
    {

        
    }
    
  
}
