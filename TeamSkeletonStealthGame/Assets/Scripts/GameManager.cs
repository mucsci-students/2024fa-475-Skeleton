

using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject pauseMenu; // Private variable that shows up in editor
    private Player player;
    public static bool isPaused; // Represents whether the game is paused or not
    private int playerClearance;
    public Text UITextClearance;
    public int Clearance{
        get{return playerClearance;}
        set{playerClearance = value; UITextClearance.GetComponent<Text>().text = "LV. " + playerClearance + " CLEARANCE";}
    }
    private int playerAmmo;
    public Text UITextAmmo;
    public int Ammo{
        get{return playerAmmo;}
        set{playerAmmo = value; UITextClearance.GetComponent<Text>().text = "Bullets: " + playerAmmo;}
    }
    // Start is called before the first frame update
    void Start()
    {
                //Finds Game object tagged as player in the scene, THEN gets player script from it.
        GameObject p = GameObject.FindWithTag("Player");
        if(p!=null){
        player = p.GetComponent<Player>(); 
        Clearance = player.securityClearance;
        }
    }

    // Update is called once per frame to check if we paused with 'Esc'
    void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex!=0){
            if(Input.GetKeyDown(KeyCode.Escape)){
                if(isPaused){
                    Resume();
                }
                else{
                    Pause();
                }
            }
        }
    }

    public void ReturnToMenu(){
        SceneManager.LoadScene("Menu"); 
    }

    
    public void LoadSceneByName(String name){
        if(SceneManager.GetActiveScene().buildIndex==0){
        SceneManager.LoadScene(name); }
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
        isPaused = true;
    }
    public void Resume()
    {        
        // This function is called when someone clicks the play button
        // It should get rid of the pause menu and unfreeze the game
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
    public void Home()
    {
        // This function is called in the pause menu to return to the main menu
        // It should resume time and use the SceneManager to load the main menu
        Time.timeScale = 1f;
        isPaused = false;
        pauseMenu.SetActive(false);
        Invoke("ReturnToMenu", 1f);
        
    }

    public void QuitToDesktop()
    {
        // This function is called in the pause/main menu to return to desktop
        Application.Quit();
    }

}
