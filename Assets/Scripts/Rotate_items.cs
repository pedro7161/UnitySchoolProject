using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate_items : MonoBehaviour
{
    private float initialPosition = 0;
    // Start is called before the first frame update
    void Start()
    {
      initialPosition = transform.position.y;  
    }

    // Update is called once per frame
      void Update ()
    {
        transform.Rotate (0,50 * Time.deltaTime, 0); //rotates 50 degrees per second around z axis

        // generate a small sine wave from initialPosition
        var sineWave = Mathf.Sin(Time.time * 2) * 0.2f;
        transform.position = new Vector3(transform.position.x, initialPosition + sineWave, transform.position.z);
    }
}
