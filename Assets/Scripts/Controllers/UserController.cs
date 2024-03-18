using UnityEngine;
using System;

public class UserController : MonoBehaviour
{
    public static Action OnDataSaved;

    private User _user = new User();

    private void Awake()
    {
        _user = LoadUserData();
        string dataJson = JsonUtility.ToJson(_user);
        Debug.Log("Loaded user: " + dataJson);
    }

    public int GetMoney()
    {
        return _user.Money;
    }

    public void SetMoney(int money)
    {
        _user.Money = money;
        SaveUserData();
    }


    public void SaveUserData()
    {
        string dataJson = JsonUtility.ToJson(_user);
        PlayerPrefs.SetString("user_data", dataJson);
        Debug.Log("User data saved.");
        OnDataSaved?.Invoke();
    }

    public User LoadUserData()
    {
        if (PlayerPrefs.HasKey("user_data"))
        {
            string data = PlayerPrefs.GetString("user_data");
            User user = JsonUtility.FromJson<User>(data);
            return user;
        }
        else
        {
            User user = new User();
            return user;
        }
    }
}

[Serializable]
public class User
{
    public int Money;
}

