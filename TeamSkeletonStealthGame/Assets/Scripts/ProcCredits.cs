using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class ProcCredits : MonoBehaviour
{
    void Update() {
        if (Input.GetKeyDown("return")) {
            SceneManager.LoadScene("End Credits");
        }
    }
}
