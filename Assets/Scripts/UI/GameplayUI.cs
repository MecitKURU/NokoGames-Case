using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayUI : MonoBehaviour
{
    public static Action<bool> OnToggleChanged;

    public Canvas Canvas;
    public TMP_Text MoneyText;

    private UserController _userController;
    private void Start()
    {
        _userController = GameController.Instance.UserController;
        UpdateMoney();
    }

    private void OnEnable()
    {
        UserController.OnDataSaved += UpdateMoney;
    }

    private void OnDestroy()
    {
        UserController.OnDataSaved -= UpdateMoney;
    }

    public void ToggleChange(bool state)
    {
        OnToggleChanged?.Invoke(state);
    }

    private void UpdateMoney()
    {
        MoneyText.text = _userController.GetMoney().ToString();
    }
}
