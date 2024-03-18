using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
    public BotPlayerController BotPlayerController;
    public List<Machine> Machines;
    public ItemContainer TrashCan;

    public Transform BotStartPoint;

    private void Start()
    {
        ItemContainer.OnItemContainerUpdated += ItemContainerUpdated;
    }

    private void OnDestroy()
    {
        ItemContainer.OnItemContainerUpdated -= ItemContainerUpdated;
    }

    private void ItemContainerUpdated()
    {
        //ItemContainer targetItemContainer = GetAvailableItemContainer();
        //Transform targetPoint = targetItemContainer.BotPoint;
        //BotPlayerController.SetTarget(targetPoint);
    }


    public ItemContainer GetAssetTransformerOutputContainer()
    {
        foreach (var machine in Machines)
        {
            if (machine.MachineType == MachineType.AssetTransformer)
            {
                ItemContainer outputItemContainer = machine.OutputItemContainer;
                return outputItemContainer;
            }
        }
        return null;
    }

    public ItemContainer GetAssetTransformerInputContainer()
    {
        foreach (var machine in Machines)
        {
            if (machine.MachineType == MachineType.AssetTransformer)
            {
                ItemContainer inputItemContainer = machine.GetComponent<AssetTransformerMachine>().InputItemContainer;
                return inputItemContainer;
            }
        }
        return null;
    }
    
    public ItemContainer GetSpawnerOutputContainer()
    {
        foreach (var machine in Machines)
        {
            if (machine.MachineType == MachineType.Spawner)
            {
                ItemContainer outputItemContainer = machine.OutputItemContainer;
                return outputItemContainer;
            }
        }
        return null;
    }


    public ItemContainer GetAvailableItemContainer()
    {
        ItemContainer assetTransformerOutputContainer = GetAssetTransformerOutputContainer();
        if(!assetTransformerOutputContainer.IsFull && assetTransformerOutputContainer.Items.Count > 0)
        {
            return assetTransformerOutputContainer;
        }

        ItemContainer spawnerOutputContainer = GetSpawnerOutputContainer();
        if(!spawnerOutputContainer.IsFull && spawnerOutputContainer.Items.Count > 0)
        {
            return spawnerOutputContainer;
        }

        return null;
    }


    public ItemContainer DecideTarget(string stackedItemsId)
    {
        if (stackedItemsId == "First")
        {
            return GetAssetTransformerInputContainer();
        }
        
        else if (stackedItemsId == "Second")
        {
            return TrashCan;
        }

        return null;

    }
}
