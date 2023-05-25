using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate_items : MonoBehaviour
{
    public float altura = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
      void Update ()
    {
        transform.Rotate (0,50*Time.deltaTime,0); //rotates 50 degrees per second around z axis
        float y = Mathf.PingPong(Time.time * 0.2f,  0.2f) + altura;
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }
}
