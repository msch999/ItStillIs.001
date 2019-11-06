using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.Tween;

public class PlayerController : MonoBehaviour
{
    public GameObject thePlayer;
    public GameObject thePointer;

    // Lerp code from
    // http://www.blueraja.com/blog/404/how-to-use-unity-3ds-linear-interpolation-vector3-lerp-correctly

    public float distance = 25.0f;

    public bool isMoving = false;

    Vector3 origPos;

    /// The time taken to move from the start to finish positions
    public float timeTakenDuringLerp = 1f;

    /// How far the object should move when 'space' is pressed
    public float distanceToMove = 10;

    //Whether we are currently interpolating or not
    private bool _isLerping;

    //The start and finish positions for the interpolation
    private Vector3 _startPosition;
    private Vector3 _endPosition;

    //The Time.time value when we started the interpolation
    private float _timeStartedLerping;

    /// Called to begin the linear interpolation
    void StartLerping()
    {
        _isLerping = true;
        _timeStartedLerping = Time.time;

        //We set the start position to the current position
        _startPosition = thePlayer.transform.position;
        _endPosition = origPos;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("PlayerController Start()");
        origPos = thePlayer.transform.position;
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


        // Check IsTouching before retrieving TouchPos. if (GvrControllerInput.IsTouching)
        if (GvrControllerInput.GetDevice(GvrControllerHand.Dominant).GetButton(GvrControllerButton.TouchPadTouch))

        {
            float f;

            // Vector2 touchPos = GvrControllerInput.TouchPos;
            Vector2 touchPos = GvrControllerInput.GetDevice(GvrControllerHand.Dominant).TouchPos;
            f = touchPos.y;
            //Debug.Log("touchPos: " + touchPos.x + "," + touchPos.y + " f: " + f);
            distance = f * 10;
        }  else
        {
            distance = 10;
        }

        // Click Button(touchpad button)
        // if (GvrControllerInput.ClickButton)
        if (GvrControllerInput.GetDevice(GvrControllerHand.Dominant).GetButton(GvrControllerButton.TouchPadButton))
        {
            isMoving = !isMoving;
        }

        //if (GvrController.AppButtonDown)
        if (GvrControllerInput.GetDevice(GvrControllerHand.Dominant).GetButtonDown(GvrControllerButton.App))
        {
            // Debug.Log("App Button pressed");
            //thePlayer.transform.position = origPos;
            StartLerping();
        }
    }

    //We do the actual interpolation in FixedUpdate(), since we're dealing with a rigidbody
    void FixedUpdate()
    {
        if (_isLerping)
        {
            //We want percentage = 0.0 when Time.time = _timeStartedLerping
            //and percentage = 1.0 when Time.time = _timeStartedLerping + timeTakenDuringLerp
            //In other words, we want to know what percentage of "timeTakenDuringLerp" the value
            //"Time.time - _timeStartedLerping" is.
            float timeSinceStarted = Time.time - _timeStartedLerping;
            float percentageComplete = timeSinceStarted / timeTakenDuringLerp;

            //Perform the actual lerping.  Notice that the first two parameters will always be the same
            //throughout a single lerp-processs (ie. they won't change until we hit the space-bar again
            //to start another lerp)
            transform.position = Vector3.Lerp(_startPosition, _endPosition, percentageComplete);

            //When we've completed the lerp, we set _isLerping to false
            if (percentageComplete >= 1.0f)
            {
                _isLerping = false;
            }
        }
    }
}