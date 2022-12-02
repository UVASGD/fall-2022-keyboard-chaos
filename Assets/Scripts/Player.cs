using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;
using System.Linq;

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

    //combat thingys
    public Destructible target;
    public bool isTargeting = true;

    private Transform camera;
    public CinemachineVirtualCamera lockOnCamera;

    public GameObject targetingArrowPrefab;
    private targetingArrow targetingArrow;

    List<Destructible> possibleTargets = new List<Destructible>();



    // Start is called before the first frame update
    void Start()
    {
        alive = true;
        healthBar.SetMaxHealth(maxHealth);

        // populate the list of all things you can target
        Destructible[] allDestructibles = FindObjectsOfType<Destructible>();
        foreach(Destructible thing in allDestructibles){
            // add every destructible to the list that isn't the player
            if(thing.gameObject != gameObject){
                possibleTargets.Add(thing);
            }
        }
        // initialize target if it hasn't been set yet
        if (target == null && possibleTargets.Count > 0)
        {
            target = possibleTargets[0];
        }
        Cursor.lockState = CursorLockMode.Locked;
        camera = Camera.main.gameObject.transform;
        controller = gameObject.GetComponent<CharacterController>();
        animator = gameObject.GetComponent<Animator>();
        //set up targeting arrow UI
        targetingArrow = GameObject.Instantiate(targetingArrowPrefab).GetComponent<targetingArrow>();
        targetingArrow.target = target.gameObject;
        player = gameObject;
        controller = gameObject.GetComponent<CharacterController>();
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!alive){
            return;
        }
        //movement
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        //Vector3 move = new Vector3(-1 * Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));
        Vector3 move;
        // if targeting something, then move in circle around it or move towards it
        if (isTargeting)
        {
            Vector3 towardsTarget = (target.gameObject.transform.position - transform.position).normalized;
            Vector3 tangentToMotionCircle = Vector3.Cross(Vector3.up, towardsTarget); //counterclockwise
            move = towardsTarget * Input.GetAxis("Vertical") + tangentToMotionCircle * Input.GetAxis("Horizontal");
        }
        else
        {
            
            Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized;
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            if(direction.magnitude > 0.1f)
            {
                move = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized;
            }
            else
            {
                move = new Vector3(0f, 0f, 0f);
            }
        }
        
        controller.Move(move * Time.deltaTime * playerSpeed);

        animator.SetBool("Running", true);
        if (move.magnitude < .2)
        { //TODO: finick with this number or change fomre "Horizontal"/"Vertical"
            animator.SetBool("Running", false);
        }
        if (move != Vector3.zero)
        {
            gameObject.transform.forward = Vector3.Slerp(gameObject.transform.forward, move, Time.deltaTime*5);
        }

        //spin towards target if stationary (so you dont attack behind you lol)
        if (isTargeting && move == Vector3.zero){
            //gracefully stolen code (like 90% of this project tbh)
            Vector3 lookPos = target.gameObject.transform.position - transform.position;
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

        // If left shift is pressed, toggle between being locked on or not
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if(isTargeting)
            {
                isTargeting = false;
            }
            else
            {
                isTargeting = true;
            }
        }
        // set up camera and targeting arrow UI depending on if you're locked on right now
        if (isTargeting)
        {
            //if currenty locked on, use the lock-on camera and make sure the targeting arrow knows to follow the enemy
            lockOnCamera.Priority = 10;
            targetingArrow.targeting = true;
            targetingArrow.target = target.gameObject;
            // if it's no longer alive, unlock
            if(!target.alive){
                isTargeting = false;
            }
        }
        else
        {
            // check to see if we should change who to target (always target closest destructible thing when not currently locked on)
            float minDist = float.PositiveInfinity; //Vector3.Distance(transform.position, target.gameObject.transform.position);
            foreach (Destructible obj in possibleTargets.ToList())
            {
                // if something isn't alive anymore, remove it from the list
                if(!obj.alive){
                    possibleTargets.Remove(obj);
                    continue;
                }
                if (obj.gameObject != this.gameObject && Vector3.Distance(transform.position, obj.gameObject.transform.position) < minDist)
                {
                    target = obj;
                    minDist = Vector3.Distance(transform.position, target.gameObject.transform.position);
                }
            }
            // ensure the free look camera is active, and move the targeting arrow to the right place
            lockOnCamera.Priority = 1;
            targetingArrow.targeting = false;
            targetingArrow.target = target.gameObject;
        }
    }

public override void Die(){
        alive = false;
        animator.SetTrigger("die");
    }

}
