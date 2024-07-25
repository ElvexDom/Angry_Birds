using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private GameObject pivot;
    [SerializeField] float detachDelayTime = 0.5f;
    [SerializeField] float spawnDelayTime = 3;
    private GameObject currentBall;
    private Camera cam;
    private Rigidbody2D ballRigidBody;
    private SpringJoint2D ballSpringJoint;
    private bool isDragging = false;
    private Vector2 initialBallPosition;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        SpawnBall();
        initialBallPosition = ballRigidBody.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(!Touchscreen.current.primaryTouch.press.IsPressed())
        {
            if(isDragging && ballRigidBody != null)
            {
                ballRigidBody.isKinematic = false;
                Invoke(nameof(DetachBall), detachDelayTime);
                isDragging = false;
                Invoke(nameof(SpawnBall), spawnDelayTime);
                // Invoke(nameof(RestartBall), 3);
            }
            return;
        }

        isDragging = true;
        if(ballRigidBody != null)
        {
            ballRigidBody.isKinematic = true;
            Vector2 worldPosition = cam.
                ScreenToWorldPoint(Touchscreen.current.primaryTouch.position.ReadValue());
            ballRigidBody.position = worldPosition;
        }
    }

    private void DetachBall()
    {
        ballSpringJoint.enabled = false;
        ballRigidBody = null;
    }

    private void RestartBall()
    {
        ballRigidBody.position = initialBallPosition;
        ballRigidBody.velocity = new Vector2(0,0);
        ballSpringJoint.enabled = true;
    }

    private void SpawnBall()
    {
        if(currentBall != null)
        {
            Destroy(currentBall);
        }

        currentBall = Instantiate(ballPrefab);
        currentBall.transform.position = pivot.transform.position;
        ballRigidBody = currentBall.GetComponent<Rigidbody2D>();
        ballSpringJoint = currentBall.GetComponent<SpringJoint2D>();
        ballSpringJoint.connectedBody = pivot.GetComponent<Rigidbody2D>();
    }
}













// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.InputSystem;

// public class InputTouch : MonoBehaviour
// {
//     // [SerializeField] private gameObject agent1;
//     private Camera mainCamera;
//     public GameObject Bowl;
//     public Rigidbody2D ballRigidbody;
//     public SpringJoint2D spring;

//     // Start is called before the first frame update
//     void Start()
//     {
//         mainCamera = Camera.main;
//         ballRigidbody = Bowl.GetComponent<Rigidbody2D>();
//         spring = Bowl.GetComponent<SpringJoint2D>();
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         // Out the method if not touching the screen
//         if (!Touchscreen.current.primaryTouch.press.IsPressed())
//         {
//             ballRigidbody.isKinematic = false;  
//             return;
//         }

//         Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
//         // Debug.Log("TOUCH POSITION: " + touchPosition);
//         Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);
//         Debug.Log("WORLD POSITION: " + worldPosition);
//         // Bowl.transform.position = worldPosition;
//         ballRigidbody.isKinematic = true;
//         ballRigidbody.position = worldPosition;
//         spring.enabled = false;
//     }
// }
