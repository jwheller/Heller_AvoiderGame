using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform followTransform; //Set to follow the player in the Unity inspector
    private Vector3 smoothPos;
    private float smoothSpeed = 0.05f;

    public GameObject cameraLeftBorder;
    public GameObject cameraRightBorder;
    public GameObject cameraUpperBorder;
    public GameObject cameraLowerBorder;

    private float cameraHalfWidth;
    private float cameraHalfHeight;

    void Start()
    {
        cameraHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
        cameraHalfHeight = Camera.main.orthographicSize;
    }

    void FixedUpdate()
    {
        //The closest the center of the camera can get to any of the border empties is half a camera length (width or height depending
        //on which border)
        float borderLeft = cameraLeftBorder.transform.position.x + cameraHalfWidth;
        float borderRight = cameraRightBorder.transform.position.x - cameraHalfWidth;
        float borderUpper = cameraUpperBorder.transform.position.y - cameraHalfHeight;
        float borderLower = cameraLowerBorder.transform.position.y + cameraHalfHeight;

        smoothPos = Vector3.Lerp(this.transform.position,
            new Vector3(Mathf.Clamp(followTransform.position.x, borderLeft, borderRight),
            Mathf.Clamp(followTransform.position.y, borderLower, borderUpper),
            this.transform.position.z), smoothSpeed);

        this.transform.position = smoothPos; //Update camera's position to smoothly follow the player
    }
}
