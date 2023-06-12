using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
   public Canvas menu;
   private void Start() {
        menu.enabled = false;
   }
   private void FixedUpdate() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
             menu.enabled = !menu.enabled;
        }
   }
}
