using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inventory : MonoBehaviour
{
    public static inventory instance;

    private List<int> items = new List<int>();  // List of item IDs the player possesses

    private void Awake()
    {
        // Set up the singleton instance
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public bool HasItem(int itemId)
    {
        return items.Contains(itemId);
    }

    public void AddItem(int itemId)
    {
        items.Add(itemId);
    }

    public void RemoveItem(int itemId)
    {
        items.Remove(itemId);
    }
}
