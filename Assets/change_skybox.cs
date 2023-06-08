using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class change_skybox : MonoBehaviour
{
    public Material defaultskybox;
    public Material daymode;
    public Material afternoonmode;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void Change_Skybox(int time)
    {
        var instance = GameObject.Find("enviorment").GetComponent<change_skybox>();
     
        if (time == 0)
        {
            RenderSettings.skybox = instance.daymode;
        }
        else if (time == 1)
        {
            RenderSettings.skybox = instance.afternoonmode;
        }
        else
        {
            RenderSettings.skybox = instance.defaultskybox;
        }
    }
}
