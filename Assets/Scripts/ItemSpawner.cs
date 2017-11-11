using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public List<GameObject> ItemPickups;    

    [Tooltip("Corresponds to 'ItemPickups' number of element")]
    [SerializeField]     
    public int NumberOfItemToSpawn;    

    private void Update ( )
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            SpawnItem ();
        }
    }

    private void SpawnItem()
    {
        if (NumberOfItemToSpawn < ItemPickups.Count)
        {
            Instantiate (ItemPickups [NumberOfItemToSpawn], new Vector3 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y, 0.0f), Quaternion.identity);
        }
        else
        {
            Debug.LogError ("NumberOfItemSpawn not included in ItemPickups List");
        }
    }
}
