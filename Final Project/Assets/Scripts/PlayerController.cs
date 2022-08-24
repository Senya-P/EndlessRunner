using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;
    public float zSpeed;
    public float maxSpeed;
    private int lane = 1; // left-middle-right
    public float move = 4; // between lanes
    public float jumpSpeed;
    public float gravity = -20;
    private float tempGravity; // controls main gravity
    public float smoothFactor = 0.4f;
    private bool isSliding = false;
    private Touch theTouch;
    private Vector2 touchStartPosition, touchEndPosition;
    private string dir; // swipe direction
    private bool canSwipe = false;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
        tempGravity = gravity;
    }

    // Update is called once per frame
    void Update()
    {
        if(zSpeed < maxSpeed)
        {
            zSpeed += 0.1f * Time.deltaTime; // speed increasing
        }
        direction.z = zSpeed;
        direction.y += gravity * Time.deltaTime;
        if (!GameManager.gameOver)
        {
            animator.SetBool("isGameStarted", true);
            controller.Move(direction * Time.deltaTime);

            dir = "noSwipe";
            if (Input.touchCount > 0)
            {
                theTouch = Input.GetTouch(0);
                if (theTouch.phase == TouchPhase.Began)
                {
                    touchStartPosition = theTouch.position;
                    canSwipe = true;
                }
                if (theTouch.phase == TouchPhase.Ended && canSwipe)
                {
                    touchEndPosition = theTouch.position;
                    Vector2 diff = touchEndPosition - touchStartPosition;
                    diff = new Vector2(diff.x / Screen.width, diff.y / Screen.height); // in %
                    if (diff.magnitude > 0.1f)   // if the swipe is more than 10% of the screen width
                    {
                        canSwipe = false;
                        if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
                        {
                            if (diff.x > 0)
                            {
                                dir = "Right";
                            }
                            else dir = "Left";
                        }
                        else
                        {
                            if (diff.y > 0)
                            {
                                dir = "Up";
                            }
                            else dir = "Down";
                        }
                    }   
                }
            }

            if (controller.isGrounded)
            {
                //if (Input.GetKeyDown(KeyCode.UpArrow))   <--- to play with keyboard arrows
                if (dir == "Up")
                {
                    if (isSliding)
                    {
                        isSliding = false;  // jump interruprs sliding
                    }
                    else
                    {
                        animator.SetBool("isJump", true);
                        direction.y = jumpSpeed;
                    }
                }
                else
                {
                    animator.SetBool("isJump", false);
                }
            }

            //if (Input.GetKeyDown(KeyCode.DownArrow) && !isSliding)
            if (dir == "Down")
            {
                if (!isSliding)
                {
                    StartCoroutine(Slide());
                }
  
            }
            
            //if (Input.GetKeyDown(KeyCode.RightArrow))
            if (dir == "Right")
            {
                lane++;
                if (lane == 3) lane = 2;
            }
            //if (Input.GetKeyDown(KeyCode.LeftArrow))
            if (dir == "Left")
            {
                lane--;
                if (lane == -1) lane = 0;
            }

            if (!isSliding)  // returning from sliding state
            {
                animator.SetBool("isSliding", false);
                controller.height = 2;
                if (gravity != tempGravity)
                    gravity = tempGravity;
            }

            Vector3 newPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
            if (lane == 0)
            {
                newPosition += Vector3.left * move;
            }
            else if (lane == 2)
            {
                newPosition += Vector3.right * move;
            }

            transform.position = Vector3.Lerp(transform.position, newPosition, smoothFactor); // smooth movement between lanes
        }

    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Barrier")
        {
            GameManager.gameOver = true;
            zSpeed = 0;
            animator.SetBool("isGameStarted", false);
        }
    }
    private IEnumerator Slide()
    {
        isSliding = true;
        gravity = -150;
        animator.SetBool("isSliding", true);
        controller.height = 1;                   // to pass under the barrier
        yield return new WaitForSeconds(1);  // sliding time
        isSliding = false;
        

    }

}
