using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadmap : MonoBehaviour
{
    public string mapname;
    public void loadmap1()
    {
        SceneManager.LoadScene(mapname);
    }
     public void button_exit()
    {
    
        Application.Quit();

    }
    
}
