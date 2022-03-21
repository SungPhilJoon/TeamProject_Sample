using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTest : MonoBehaviour
{
    #region Variables
    public InventoryObject inventoryObject;
    public ItemObjectDatabase databaseObject;

    #endregion Variables

    #region Helper Methods
    public void AddNewItem()
    {
        if (databaseObject.itemObjects.Length > 0)
        {
            ItemObject newItemObject = databaseObject.itemObjects[Random.Range(0, databaseObject.itemObjects.Length - 1)];
            Item newItem = new Item(newItemObject);

            inventoryObject.AddItem(newItem, 1);
        }
    }

    public void ClearInventory()
    {
        
    }

    #endregion Helper Methods
}
