using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameplayUI GameplayUI;
    public PathController PathController;
    public UserController UserController;

    public static GameController Instance;
    private void Awake()
    {
        Instance = this;
    }
}
