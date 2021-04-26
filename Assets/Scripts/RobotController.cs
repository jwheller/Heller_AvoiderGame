using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    public Transform playersTransform; //Player's location
    private Vector3 DistFromPlayer;
    private Vector3 smoothPos;
    public Animator robotAnimator;

    public List<WaypointScript> Waypoints = new List<WaypointScript>(); //List of waypoints that concern each instance of a robot
    public float Speed = 4f; 
    public int DestinationWaypoint = 1; //The first destination of the robot is Waypoint 1
    private bool Forwards = true; //All robots are created facing forwards
    private float TimePassed = 0f;
    private bool IsFacingPlayer; //To determine if the robot can "see" the player

    private Vector3 Destination;

    private bool m_FacingRight = true;  // For determining which way the robot is currently facing.
    private bool m_FacingForward = true;

    public enum PatrolDirection //Possible directions that the robots can be pacing in
    {
        Vertical,
        Horizontal
    }

    public PatrolDirection patrolDirection;

    void Start()
    {
        this.Destination = this.Waypoints[DestinationWaypoint].transform.position; //Robot's destination is the position of the next waypoint
        //on the list
    }

    void Update()
    {
        StopAllCoroutines();
        StartCoroutine(MoveTo());

        DistFromPlayer = this.transform.position - playersTransform.transform.position; //Difference between robot's and player's x,y,z coordinates

        if (patrolDirection == PatrolDirection.Horizontal)
        {
            if (m_FacingRight && DistFromPlayer.x < 0f) //If the robot is facing right and the player is standing to the right of the robot...
            {
                IsFacingPlayer = true; //Then the robot is facing the player
            }     
            else if (!m_FacingRight && DistFromPlayer.x > 0f) //If robot is facing left and player is to the left of the robot...
            {
                IsFacingPlayer = true; //Then the robot is facing the player
            }            
        }
        
        else //If the patrolDirection is vertical (the only other option)
        {
            if (m_FacingForward && DistFromPlayer.y > 0f) //If the robot is facing foward (down) and the player is below it...
            {
                IsFacingPlayer = true; //Then the robot is facing the player
            }         
            else if (!m_FacingForward && DistFromPlayer.y < 0f) //If the robot is look backwards (up) and the player is above it...
            {
                IsFacingPlayer = true; //Then the robot is facing the player
            }
        }
        
        if (Mathf.Abs(DistFromPlayer.x) <= 3f && Mathf.Abs(DistFromPlayer.y) <= 3f) //If the player gets within 3 x and y units of the robot..., 
        {
            if (IsFacingPlayer)
            {
                FollowPlayer();
            }
            else
                IsFacingPlayer = false;         
        }
        else //Incase the player runs out of range or is already out of range...
        {
            robotAnimator.SetBool("Following", false); //Don't play the "following" animation
            this.Destination = this.Waypoints[DestinationWaypoint].transform.position; //Reset destination to be the next waypoint
        }
    }

    IEnumerator MoveTo()
    {
        while((transform.position - this.Destination).sqrMagnitude > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position,
                this.Destination, this.Speed * Time.deltaTime);
            yield return null;
        }
        if ((transform.position - this.Destination).sqrMagnitude <= 0.01f)
        {
            if (this.Waypoints[DestinationWaypoint].IsSentry)
            {
                while (this.TimePassed < this.Waypoints[DestinationWaypoint].PauseTime)
                {
                    this.TimePassed += Time.deltaTime;
                    yield return null;
                }

                this.TimePassed = 0f;
            }
            GetNextWaypoint();
        }
    }

    void GetNextWaypoint()
    {
        if(this.Waypoints[DestinationWaypoint].IsEndpoint)
        {
            if (this.Forwards)
            {
                this.Forwards = false;
            }
            else
                this.Forwards = true;

            if (this.Forwards)
                ++DestinationWaypoint;
            else 
                --DestinationWaypoint;
            if (patrolDirection == PatrolDirection.Horizontal)
            {
                FlipHorozontal();
            }
            else
            {
                FlipVertical();
            }
            this.Destination = this.Waypoints[DestinationWaypoint].transform.position;            
        }
    }

    private void FlipHorozontal()
    {
        // Switch the way the robot is labelled as facing
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void FlipVertical()
    {
        //Switch the way the robot is labelled as facing
        m_FacingForward = !m_FacingForward;
        robotAnimator.SetBool("Backwards", !m_FacingForward); //Start the "backwards" moving animation
    }

    void FollowPlayer()
    {
        this.Destination = playersTransform.transform.position; //Robot's destination is set to be the player themselves
        robotAnimator.SetBool("Following", true); //Start the "Following" animation
    }
}
