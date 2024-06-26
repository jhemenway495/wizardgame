using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Camera Settings
    [Header("Camera Variables")]
    [SerializeField] Transform playerCamera = null;
    [SerializeField] float mouseSensitivity = 3.5f;
    [SerializeField][Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;

    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero;
    //Camera Courser Lock
    [SerializeField] bool lockCursor = true;

    float cameraPitch = 0.0f;

    //Movement Variables
    [SerializeField] float walkSpeed = 6.0f;
    [SerializeField][Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;
    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelocity = Vector2.zero;
    [SerializeField] float jumpVelocity = 5.0f;
    int jumps = 1;


    //Gravity
    [SerializeField] float gravity = -13.0f;
    float velocityY = 0.0f;

    //Movement Controller
    CharacterController controller = null;
    void Start()
    {
        
        //Gets character controller
        controller = GetComponent<CharacterController>();
        
        //Locks The coursor in place and makes it invisible
        if(lockCursor)
        {

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

        }
        



    }


    void Update()
    {
        UpdateMouseLook();
        UpdateMovement();
    }

    void UpdateMouseLook()
    {
        //Camera input
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);
        
        //Camera input Math
        cameraPitch -= currentMouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);
        
        //Camera input application
        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);

        
    }

    void UpdateMovement() 
    {
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        if (controller.isGrounded)
        {
            velocityY = 0.0f;
            jumps = 1;
        
        }
        if (Input.GetKeyDown(KeyCode.Space) && jumps > 0)
        {
            velocityY = jumpVelocity;
            jumps -= 1;
        
        }

        velocityY += gravity * Time.deltaTime;

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);
        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * walkSpeed + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);
    
    }
    

}
