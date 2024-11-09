

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu; // Private variable that shows up in editor
    private int playerClearance;
    public Text UITextClearance;
    public int Clearance{
        get{return playerClearance;}
        set{playerClearance = value; UITextClearance.GetComponent<Text>().text = "Security Clearance: Level " + playerClearance;}
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnToMenu(){
        SceneManager.LoadScene("Main_Menu"); 
    }

    // Default method to get us to first level
    public void LoadLevel(){
        SceneManager.LoadScene(1);
    }

    // Overwrite method to get us to further levels
    public void LoadLevel(int level){
        SceneManager.LoadScene(1 + level);
    }

    // Returns player to main menu
    public void GameOver(){
        Invoke("ReturnToMenu", 5f);
    }

    public void Pause()
    {
        // This function is called when someone clicks the pause button
        // It should bring up the pause menu and freeze the game
        pauseMenu.SetActive(true);
        Time.timeScale = 0f; 
    }
    public void Resume()
    {        
        // This function is called when someone clicks the play button
        // It should get rid of the pause menu and unfreeze the game
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        
    }
    public void Home()
    {
        // This function is called in the pause menu to return to the main menu
        // It should resume time and use the SceneManager to load the main menu
        Time.timeScale = 1f;
        Invoke("ReturnToMenu", 1f);
        
    }

    public void QuitToDesktop()
    {
        // This function is called in the pause/main menu to return to desktop
        Application.Quit();
    }

}
