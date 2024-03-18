using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Item : MonoBehaviour
{
    public List<GameObject> Items = new List<GameObject>();
    private bool _isPassive;
    private string _itemId;
    public bool IsPassive()
    {
        return _isPassive;
    }

    public void Hide()
    {
        foreach (var item in Items)
        {
            item.SetActive(false);
        }
        _isPassive = true;
        transform.SetParent(ObjectPool.Instance.ItemsParent);
    }

    public string GetItemId()
    {
        return _itemId;
    }

    public void Show(string itemId)
    {
        _isPassive = false;
        _itemId = itemId;

        foreach (var item in Items)
        {
            item.SetActive(false);
        }
        transform.eulerAngles = Vector3.zero;
        GetItem(itemId).SetActive(true);
    }

    public void JumpTarget(Vector3 targetPoint, Action onComplete = null)
    {
        transform.DOLocalJump(targetPoint, 1, 1, 0.3f).OnComplete(() => onComplete?.Invoke());
    }
    
    public void RotateTarget(Vector3 targetRotation, Action onComplete = null)
    {
        transform.DOLocalRotate(targetRotation, 0.3f).OnComplete(() => onComplete?.Invoke());
    }

    private GameObject GetItem(string itemId)
    {
        switch(itemId)
        {
            case "First":
                return Items[0];
            case "Second":
                return Items[1];
            case "Third":
                return Items[2];
        }
        return null;
    }
}
