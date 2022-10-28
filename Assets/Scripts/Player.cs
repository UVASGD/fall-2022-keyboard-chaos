using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : Destructible
{
    public Animator animator;

    public GameObject player;
    
    //Movement stuff
    CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public float playerSpeed = 2.0f;
    public float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    public GameObject target; // i think we're implementing some new camera stuff, so this is no longer needed when that happens because all target references are now in AbilityLibrary


    // Start is called before the first frame update
    void Start()
    {
        player = gameObject;
        controller = gameObject.GetComponent<CharacterController>();
        animator = gameObject.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        //movement
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        //Vector3 move = new Vector3(-1 * Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));

        Vector3 towardsTarget = (target.transform.position - transform.position).normalized;
        Vector3 tangentToMotionCircle = Vector3.Cross(Vector3.up, towardsTarget); //counterclockwise
        Vector3 move = towardsTarget * Input.GetAxis("Vertical") + tangentToMotionCircle * Input.GetAxis("Horizontal");
        controller.Move(move * Time.deltaTime * playerSpeed);

        animator.SetBool("Running", true);
        if (move.magnitude < .2)
        { //TODO: finick with this number or change fomre "Horizontal"/"Vertical"
            animator.SetBool("Running", false);
        }
        if (move != Vector3.zero)
        {
            gameObject.transform.forward = Vector3.Slerp(gameObject.transform.forward, move, Time.deltaTime * 10);
        }

        //spin towards target if stationary (so you dont attack behind you lol)
        if (move == Vector3.zero)
        {
            //gracefully stolen code (like 90% of this project tbh)
            Vector3 lookPos = target.transform.position - transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2); //this 2 just makes it spin a lil faster
            //transform.rotation = Quaternion.LookRotation(Vector3.Lerp(transform.rotation.eulerAngles, target.transform.position - transform.position, Time.deltaTime));
        }

        // Changes the height position of the player.
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

}
