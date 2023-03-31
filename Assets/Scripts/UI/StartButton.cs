using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    //When start is clicked change to game scene
    
    public void LoadLevel()
    {
        SceneManager.LoadScene("Demo Level F");
    }
}