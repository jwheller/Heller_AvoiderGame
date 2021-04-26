using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    public MenuController menu;
    public AudioClip biteClip; //The bite sound, or a "soundbite" if you will

    public enum KeyType
    {
        Burger,
        Fries,
        dasWürstchen
    }

    public KeyType keyType;

    private float floatingTimer = 0f;
    private float floatingMax = 1f;
    private float floatingDir = 0.01f;

    private void FixedUpdate()
    {
        if (floatingTimer < floatingMax)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + floatingDir); //Move the key in the float direction
            //on the y-axis
            floatingTimer += Time.fixedDeltaTime;
        }
        else
        {
            floatingDir *= -1; //Change direction
            floatingTimer = 0f; //Reset timer
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9) //If the key collides with the player...
        {
            if (keyType == KeyType.Burger) //If the key is of the type burger...
            {
                Debug.Log("You got the burger!");
            }
        }
        AudioSource.PlayClipAtPoint(biteClip, transform.position); //Play sound
        menu.winScreen();
        Destroy(this.gameObject);
    }


}
