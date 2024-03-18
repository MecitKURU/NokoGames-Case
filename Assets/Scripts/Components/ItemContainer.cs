using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class ItemContainer : MonoBehaviour
{
    public static Action OnItemContainerUpdated;
    public static Action OnItemsUpdated;
    public string StackedItemId;
    public bool IsFull;
    public ContainerType ContainerType;
    public GameObject SpawnedObject;
    public Transform SpawnPoint;
    public Transform ItemsParent;
    public Vector3 ItemGrid;
    public Vector3 ItemsDifference;
    public Transform BotPoint;
    public List<Transform> BotWaitingPoints;

    public Machine Machine;

    public List<Item> Items = new List<Item>();
    private int _maxItemCount;

    private void Start()
    {
        _maxItemCount = (int)(ItemGrid.x * ItemGrid.y * ItemGrid.z);
    }

    public Transform GetWaitingPoint(Vector3 position)
    {
        float minDistance = 999;
        Transform nearestPoint = null;

        for (int i = 0; i < BotWaitingPoints.Count; i++)
        {
            float distance = Mathf.Abs(Vector3.Distance(position, BotWaitingPoints[i].transform.position));
            if (distance < minDistance)
            {
                nearestPoint = BotWaitingPoints[i];
                minDistance = distance;
            }
        }

        return nearestPoint;
    }

    public bool IsAvailable()
    {
        return Items.Count < _maxItemCount;
    }

    public Vector3 GetAvailablePosition()
    {
        int index = Items.Count;
        int xIndex = index % (int)(ItemGrid.x);
        int yIndex = index / (int)(ItemGrid.x * ItemGrid.z);
        int zIndex = (index / (int)ItemGrid.x) % (int)ItemGrid.z;

        float xPosition = xIndex * ItemsDifference.x;
        float yPosition = yIndex * ItemsDifference.y;
        float zPosition = zIndex * ItemsDifference.z;

        return new Vector3(xPosition, yPosition, zPosition);
    }

    public Item GiveItem()
    {
        Item spawnedItem = null;

        int itemCount = Items.Count;

        if(itemCount > 0)
        {
            Item destroyingItem = Items[itemCount - 1];
            spawnedItem = ObjectPool.Instance.GetPooledObject(StackedItemId);
            if (spawnedItem != null)
            {
                spawnedItem.transform.position = destroyingItem.transform.position;
                spawnedItem.Show(StackedItemId);
            }
            destroyingItem.Hide();
            Items.RemoveAt(itemCount - 1);
        }
        return spawnedItem;
    }

    public void TakeItem(Item item)
    {
        if(ContainerType == ContainerType.TakeItem)
        {
            item.transform.SetParent(ItemsParent);
            item.transform.DOLocalRotate(Vector3.zero, 0.3f);
            item.JumpTarget(GetAvailablePosition());
            Items.Add(item);
        }

        else if(ContainerType == ContainerType.TrashCan)
        {
            item.transform.SetParent(ItemsParent);
            item.transform.DOLocalRotate(Vector3.zero, 0.3f);
            item.JumpTarget(Vector3.zero, ()=> {
                item.Hide();
            });
        }
    }
}


public enum ContainerType
{
    None,
    GiveItem,
    TakeItem,
    TrashCan
}
