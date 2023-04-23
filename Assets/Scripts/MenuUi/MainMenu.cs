using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public void PlayGame() => StartCoroutine(LoadGame());

    IEnumerator LoadGame() {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public void LoadOptionsMenu() {
        Debug.Log("Loading options menu!");
    }
    
    public void QuitGame()
    {
        FindObjectOfType<MenuAudioManager>().StopMusic();
        Application.Quit();
    }

}
