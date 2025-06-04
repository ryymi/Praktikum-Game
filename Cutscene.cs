using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Cutscene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }
}
