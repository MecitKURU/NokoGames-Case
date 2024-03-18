using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerMachine : Machine
{
    public bool CanWork = true;
    private void Update()
    {
        if (CanWork && OutputItemContainer.IsAvailable())
        {
            timer += Time.deltaTime;
            if (timer >= GenerationTime)
            {
                GenerateItem();
                timer = 0;
            }
        }
    }
}
