using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetTransformerMachine : Machine
{
    public ItemContainer InputItemContainer;
    public Transform InputItemPoint;

    private void Update()
    {
        if (InputItemContainer.Items.Count > 0 && OutputItemContainer.IsAvailable())
        {
            timer += Time.deltaTime;
            if (timer >= GenerationTime)
            {
                ChangeItem();
                timer = 0;
            }
        }
    }

    public void ChangeItem()
    {
        Item item = InputItemContainer.GiveItem();

        if (item != null)
        {
            InputItemContainer.Items.Remove(item);
            item.transform.SetParent(InputItemPoint);
            item.JumpTarget(Vector3.zero, () =>
            {
                item.Hide();
                GenerateItem();
            });
        }
    }
}
