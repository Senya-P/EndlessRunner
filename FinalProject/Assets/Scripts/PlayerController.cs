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
    private enum SwipeDirection { None, Up, Down, Left, Right }
    SwipeDirection dir;
    private bool canSwipe = false;
    private bool multipleSlide = false;
    public AudioSource gameOverSound;
    public AudioSource mainTheme;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
        tempGravity = gravity;
        mainTheme.Play();
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

            dir = SwipeDirection.None;
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
                                dir = SwipeDirection.Right;
                            }
                            else dir = SwipeDirection.Left;
                        }
                        else
                        {
                            if (diff.y > 0)
                            {
                                dir = SwipeDirection.Up;
                            }
                            else dir = SwipeDirection.Down;
                        }
                    }   
                }
            }

            if (controller.isGrounded)
            {
                //if (Input.GetKeyDown(KeyCode.UpArrow))   <--- to play with keyboard arrows
                if (dir == SwipeDirection.Up)
                {
                    if (isSliding)
                    { 
                        gravity += 100; // jump interrupts sliding
                    }
                    animator.SetBool("isJump", true);
                    direction.y = jumpSpeed;
                }
                else
                {
                    animator.SetBool("isJump", false);
                }
            }

            //if (Input.GetKeyDown(KeyCode.DownArrow) && !isSliding)
            if (dir == SwipeDirection.Down)
            {
                if (!isSliding)
                {
                    StartCoroutine(Slide());
                }
                else
                    multipleSlide = true;
  
            }
            
            //if (Input.GetKeyDown(KeyCode.RightArrow))
            if (dir == SwipeDirection.Right)
            {
                lane++;
                if (lane == 3) lane = 2;
            }
            //if (Input.GetKeyDown(KeyCode.LeftArrow))
            if (dir == SwipeDirection.Left)
            {
                lane--;
                if (lane == -1) lane = 0;
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

            // transform.position = Vector3.Lerp(transform.position, newPosition, smoothFactor); // smooth movement between lanes

            Vector3 dist = newPosition - transform.position;
            Vector3 movement = dist.normalized * zSpeed * Time.deltaTime;  // gradual, swooth movement
            if (Mathf.Abs(movement.x) > Mathf.Abs(dist.x)) // desired position exceeded
            {
                movement = dist;  // to limit the movement
            }
            controller.Move(movement);
        }

    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Barrier")
        {
            gameOverSound.Play();
            GameManager.gameOver = true;
            zSpeed = 0;
            animator.SetBool("isGameStarted", false);
        }
    }
    private IEnumerator Slide()
    {
        isSliding = true;
        gravity -= 100;
        animator.SetBool("isSliding", true);
        controller.height = 1;                   // to pass under the barrier
        yield return new WaitForSeconds(1);      // sliding time
        if (multipleSlide)
            yield return new WaitForSeconds(1);  // double slide
        isSliding = false;
        multipleSlide = false;
        animator.SetBool("isSliding", false);
        controller.height = 2;
        if (gravity != tempGravity)
            gravity = tempGravity;

    }

}
