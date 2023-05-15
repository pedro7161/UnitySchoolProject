using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.UI;
public class GetItems : MonoBehaviour
{ 

    public Image imageComponent; // Assign the Image component in the Inspector
    public Sprite newSprite; // Assign the new sprite in the Inspector

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            // Change the sprite of the Image component
            imageComponent.sprite = newSprite;
            Destroy(this.gameObject);

        }
    }
}
