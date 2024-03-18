using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public class ToolEditor : Editor
{
    [MenuItem("Tools/Clear User Data")]
    public static void ClearPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("User data cleared successfully!");
    }
}