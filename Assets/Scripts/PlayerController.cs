using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject thePlayer;
    public GameObject thePointer;

    //public float movementSpeed = 10.0f;
    public float distance = 25.0f;

    public bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("PlayerController Start()");
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            // https://forum.unity.com/threads/solved-moving-object-in-the-direction-of-camera-view.30330/
            // Move the thePlayer in the direction thePointer is facing distance units per second.
            thePlayer.transform.position = thePlayer.transform.position + thePointer.transform.forward * distance * Time.deltaTime;
        }


        //if (GvrController.AppButtonDown)
        if (GvrControllerInput.GetDevice(GvrControllerHand.Dominant).GetButtonDown(GvrControllerButton.App))
        {
            // Debug.Log("App Button pressed");
            isMoving = !isMoving;
        }
    }
}