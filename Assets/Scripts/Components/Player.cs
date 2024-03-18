using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    public Action OnItemsUpdated;

    public AnimationCurve Curve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 1.0f);
    public Animator Animator;
    public Transform ItemsParent;

    public int ItemLimit;
    public float MovementSpeed;
    public float RotationSpeed;
    public float TakeItemTime;
    public float GiveItemTime;
    public float StackOffset;
    public float TailFollowSpeed;

    internal ItemContainer _currentItemContainer = null;
    internal List<Item> items = new List<Item>();
    internal float currentSpeed;

    private float _timer = 0;
    private UserController _userController;

    public virtual void Start()
    {
        _userController = GameController.Instance.UserController;
    }
    public virtual void Update()
    {
        if (items.Count > 1)
        {
            Wobble();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ItemContainer>() != null)
        {
            ItemContainer itemContainer = other.GetComponent<ItemContainer>();
            itemContainer.IsFull = true;
            ItemContainer.OnItemContainerUpdated?.Invoke();
            _timer = 0;
            if (itemContainer.Machine != null)
            {
                if (itemContainer.Machine.MachineType == MachineType.Spawner)
                {
                    itemContainer.Machine.GetComponent<SpawnerMachine>().CanWork = false;
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<ItemContainer>() != null)
        {
            ItemContainer itemContainer = other.GetComponent<ItemContainer>();

            if (itemContainer.ContainerType == ContainerType.GiveItem)
            {
                TakeItem(itemContainer);
            }

            else
            {
                GiveItem(itemContainer);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<ItemContainer>() != null)
        {
            ItemContainer itemContainer = other.GetComponent<ItemContainer>();
            itemContainer.IsFull = false;
            ItemContainer.OnItemContainerUpdated?.Invoke();
            _currentItemContainer = null;
            if (itemContainer.Machine != null)
            {
                if (itemContainer.Machine.MachineType == MachineType.Spawner)
                {
                    itemContainer.Machine.GetComponent<SpawnerMachine>().CanWork = true;
                }
            }
        }
    }
    private void TakeItem(ItemContainer itemContainer)
    {
        bool isContainerAvailable = IsContainerAvailable(itemContainer.StackedItemId);

        if (items.Count < ItemLimit && isContainerAvailable)
        {
            _timer += Time.deltaTime;

            if (_timer >= TakeItemTime)
            {
                Item spawnedItem = itemContainer.GiveItem();
                if (spawnedItem != null)
                {
                    spawnedItem.transform.SetParent(ItemsParent);

                    Vector3 targetPosition = new Vector3(0, items.Count * StackOffset, 0);
                    spawnedItem.JumpTarget(targetPosition);

                    Vector3 targetRotation = Vector3.zero;
                    spawnedItem.RotateTarget(targetRotation);

                    items.Add(spawnedItem);
                    _currentItemContainer = itemContainer;
                    ItemContainer.OnItemsUpdated?.Invoke();
                }
                _timer = 0;
            }
        }
    }

    private void GiveItem(ItemContainer itemContainer)
    {
        if (items.Count > 0)
        {
            _timer += Time.deltaTime;

            if (_timer >= GiveItemTime)
            {
                Item item = items[items.Count - 1];

                bool shouldTake = true;

                if (itemContainer.ContainerType == ContainerType.TakeItem)
                {
                    shouldTake = item.GetItemId() == itemContainer.StackedItemId;
                }
                else if (itemContainer.ContainerType == ContainerType.TrashCan)
                {
                    if (itemContainer.StackedItemId == "Second") _userController.SetMoney(_userController.GetMoney() + 5);
                }

                if (shouldTake)
                {
                    GiveItem(itemContainer, item);
                    items.Remove(item);
                    _currentItemContainer = itemContainer;
                    ItemContainer.OnItemsUpdated?.Invoke();
                }
                _timer = 0;
            }
        }
    }
    private void GiveItem(ItemContainer itemContainer, Item item)
    {
        if (itemContainer.IsAvailable())
        {
            itemContainer.TakeItem(item);
        }
    }

    internal bool IsReachedMax()
    {
        return items.Count >= ItemLimit;
    }
    private bool IsContainerAvailable(string stackedItemId)
    {
        bool isContainerAvailable = true;
        if (items.Count > 0)
        {
            string itemId = items[0].GetItemId();
            if (itemId != stackedItemId) isContainerAvailable = false;
        }
        return isContainerAvailable;
    }


    private void Wobble() // Tail effect
    {
        int height = items.Count;

        for (int i = 0; i < height; ++i)
        {
            float curveKey = i / (float)ItemLimit;
            float curveValue = Curve.Evaluate(curveKey);
            float updatedPosX = curveValue * currentSpeed;
            float updatedRotZ = curveValue * currentSpeed * 10;

            Vector3 updatedPos = new Vector3(updatedPosX, items[i].transform.localPosition.y, items[i].transform.localPosition.z);
            items[i].transform.localPosition = Vector3.Lerp(items[i].transform.localPosition, updatedPos, TailFollowSpeed * Time.deltaTime);

            Vector3 updatedRot = new Vector3(items[i].transform.localEulerAngles.x, items[i].transform.localEulerAngles.y, updatedRotZ);
            items[i].transform.localEulerAngles = Vector3.Lerp(items[i].transform.localEulerAngles, updatedRot, TailFollowSpeed * Time.deltaTime);
        }
    }
}
