using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
public class HintsScript : MonoBehaviour
{
    [SerializeField]
    protected string sceneName;
    [SerializeField]
    protected string HintBoxName;

    void Start() {
        GameObject camera = GameObject.Find("Hint Camera");
        GameObject player = GameObject.Find("Player");

        camera.transform.position = player.transform.position;
        camera.transform.position += new Vector3(0, 0, -0.42f);
    }
    
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            SceneManager.UnloadSceneAsync(HintBoxName, UnloadSceneOptions.None); 
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
            Scene scene = SceneManager.GetActiveScene();
            
            Debug.Log("Clicked Left Click to leave the scene");
        }
    }

}
