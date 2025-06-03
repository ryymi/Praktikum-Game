using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
public string mainSceneName = "MainScene";
public string mainMenuName = "MainMenu";
public void Play()
{
SceneManager.LoadScene(mainSceneName);
}

public void MainMenus()
{
SceneManager.LoadScene(mainMenuName);
}

public void Quit()
{
Debug.Log("Quit game requested.");
// Jika dijalankan sebagai build aplikasi, maka aplikasi akan keluar
Application.Quit();
}
}
