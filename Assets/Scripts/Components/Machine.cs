using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Machine : MonoBehaviour
{
    public string GeneratedItemId;
    public MachineType MachineType;
    public GameObject SpawnedObject;
    public ItemContainer OutputItemContainer;
    public float GenerationTime;
    internal float timer;

    internal void GenerateItem()
    {
        Item newObject = ObjectPool.Instance.GetPooledObject(GeneratedItemId);
        if (newObject != null)
        {
            newObject.transform.SetParent(OutputItemContainer.ItemsParent);
            newObject.transform.position = OutputItemContainer.SpawnPoint.position;
            newObject.Show(GeneratedItemId);
            newObject.JumpTarget(OutputItemContainer.GetAvailablePosition());

            Vector3 rotation = GetRotation(GeneratedItemId);
            newObject.RotateTarget(rotation);

            OutputItemContainer.Items.Add(newObject);
        }
    }

    private Vector3 GetRotation(string itemId)
    {
        Vector3 rotation = Vector3.zero;

        if (itemId == "First")
            rotation = new Vector3(0, 0, 0);
        else if (itemId == "Second")
            rotation = new Vector3(0, 90, 0);

        return rotation;
    }
}

public enum MachineType
{
    None,
    Spawner,
    AssetTransformer
}