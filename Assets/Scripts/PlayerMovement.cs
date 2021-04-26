using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed = 5f;
    public bool hasDoubleSpeed = false;
    public Animator animator;

    private bool m_FacingRight = true;  // For determining which way the player is currently facing.

    private float lastClickTimeMax = 1f; //The amount of time that can pass between mouse clicks to be considered a "double click"
    private float lastClickTimeCur = 0f;
    //private float doubleSpeedTimeMax = 1.5f;
    //private float doubleSpeedTimeCur = 0f;
    private Vector3 startPos;
    private bool hasClickedOnce = false;

    void Start()
    {
        startPos = transform.position; //startPos is where the player's starting location
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        if (hasDoubleSpeed)
        {
            speed = 10f;
        }
        else
        {
            speed = 5f;
        }

        Vector3 mouseInSpace = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && hasClickedOnce == false) //If the mouse is clicked and it hasn't been clicked before (recently)
        {
            lastClickTimeCur = 0; //Time since the last mouse click is set to 0 sec
            hasClickedOnce = true; //Record that it has been clicked
            StopAllCoroutines();
            StartCoroutine(MoveTo(transform.position, mouseInSpace, speed));
        }
    }

    IEnumerator MoveTo(Vector3 start, Vector3 destination, float speed)
    {
        if ((transform.position - destination).x > 0 && m_FacingRight) //If the destination is to the left of the character
        // and they are currently facing right...
        {
            Flip(); //flip them (to face left)
        }

        else if ((transform.position - destination).x < 0 && !m_FacingRight)//If the destination is to the right of the character
        // and they are currently facing left...
        {
            Flip(); //flip them (to face right)
        }
           
            while ((transform.position - destination).sqrMagnitude > 0.01f) //While the player is currently moving...
        {
            animator.SetFloat("Speed", (Mathf.Abs((transform.position - destination).x)) + 
                (Mathf.Abs((transform.position - destination).y))); //Set the "Speed" attribute in the Animator equal to the sum of the 
            //absolute values of the x and y movements, therefore if the player is currently moving in any direction, the run animation
            //will begin
            transform.position = Vector2.MoveTowards(transform.position,
                destination, speed * Time.deltaTime); //Move the player
            yield return null;
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void FixedUpdate()
    {
        if (lastClickTimeCur < lastClickTimeMax) //If the time since the last click is less than the maximum time specified above
        {
            if (Input.GetMouseButtonDown(0) && hasClickedOnce == true) //If the mouse is clicked and it's already been clicked once...
            {
                hasDoubleSpeed = true;
            }       
            lastClickTimeCur += Time.fixedDeltaTime; //Update the click timer
        }
        else
        {
            hasClickedOnce = false;
            hasDoubleSpeed = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 10) //If player collides with a villain...
        {
            Debug.Log("YOU DIED!!!");
            transform.position = startPos; //Send player back to the start position
            StopAllCoroutines(); //Remove any "momentum" the player may have had while they died
            animator.SetFloat("Speed", 0); //Set the player back to idle animation
        }
    }
}
