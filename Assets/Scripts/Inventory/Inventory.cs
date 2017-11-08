using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public List<Item> Items = new List<Item> ();

    private void Awake ( )
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public delegate void OnItemChanged ( );
    public OnItemChanged OnItemChangedCallback;

    public void Add(Item item)
    {
        Items.Add (item);

        if (OnItemChangedCallback != null)
        {
            OnItemChangedCallback.Invoke ();
        }
    }

    public void Remove(Item item)
    {
        Items.Remove (item);

        if (OnItemChangedCallback != null)
        {
            OnItemChangedCallback.Invoke ();
        }
    }
}
