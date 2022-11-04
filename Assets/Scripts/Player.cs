using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class Player : Destructible
{
    Animator animator;
    
    //Movement stuff
    CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public float playerSpeed = 2.0f;
    public float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    //combat thingys
    public float autoDmg = 5;
    public float autoSwingTimer = 4; //how long between autos
    private float lastAutoSwing = 0;  //time since last swing    

    public int swingRange = 4;
    public GameObject target; //TODO, be able to swap target (also extend this same target to PlayerCamera.cs)
    public bool isTargeting = true;

    private Transform camera;
    public CinemachineVirtualCamera lockOnCamera;

    public GameObject targetingArrowPrefab;
    private targetingArrow targetingArrow;

    private Destructible[] possibleTargets;

    //Abilities
    //TO ADD NEW: 
    //  Make sure that your button isn't already covered
    //  For time cooldown, follow the format of variables for Slice (modifying numbers as you see fit)
    //  Jump down to UpdateCDs() if you want it to cooldown based on time
    //      Add it to the other cooldowns
    //  Add your ability in the same order as up here in the FixedUpdate() function (or elsewhere in it's own function if you're fancy, but should probably be called there)
    //  That's it!

    //Slice = 1
    //Limited by swingRange
    public float sliceCD = 10;
    public float lastSliceCall = 0;
    public int sliceDmg = 10;

    [SerializeField]
    private Image imageCooldownSlice;
    [SerializeField]
    private TMP_Text textCooldownSlice;

    //Dizzy Dizzy = 2 
    public float dizzyCD = 30;
    public float lastDizzyCall = 0;
    public int dizzyDmg = 2;

    [SerializeField]
    private Image imageCooldownDizzy;

    [SerializeField]
    private TMP_Text textCooldownDizzy;

    //Unsurprising Slash = 3 (name not permanet just can't think of anything else that annoys dizzy people)
    //Limited by swingRange
    public float unsurprisingCD = 6;
    public float lastUnsurprisingCall = 0;
    public int unsurprisingDmg = 1; //(damage will be x20 on dizzy)

    [SerializeField]
    private Image imageCooldownUnsurprising;

    [SerializeField]
    private TMP_Text textCooldownUnsurprising;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        camera = Camera.main.gameObject.transform;
        controller = gameObject.GetComponent<CharacterController>();
        animator = gameObject.GetComponent<Animator>();
        //set up targeting arrow UI
        targetingArrow = GameObject.Instantiate(targetingArrowPrefab).GetComponent<targetingArrow>();
        targetingArrow.target = target;
        possibleTargets = FindObjectsOfType<Destructible>();
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
        Vector3 move;
        // if targeting something, then move in circle around it or move towards it
        if (isTargeting)
        {
            Vector3 towardsTarget = (target.transform.position - transform.position).normalized;
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
        if (move.magnitude < .2){ //TODO: finick with this number or change fomre "Horizontal"/"Vertical"
            animator.SetBool("Running", false);
        }
        if (move != Vector3.zero)
        {
            gameObject.transform.forward = Vector3.Slerp(gameObject.transform.forward, move, Time.deltaTime*5);
        }

        //spin towards target if stationary (so you dont attack behind you lol)
        if (isTargeting && move == Vector3.zero){
            //gracefully stolen code (like 90% of this project tbh)
            Vector3 lookPos = target.transform.position - transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5); //this 2 just makes it spin a lil faster
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
            targetingArrow.target = target;
        }
        else
        {
            // check to see if we should change who to target (always target closest destructible thing when not currently locked on)
            float minDist = Vector3.Distance(transform.position, target.transform.position);
            foreach (Destructible obj in possibleTargets)
            {
                if (obj.gameObject != this.gameObject && Vector3.Distance(transform.position, obj.gameObject.transform.position) < minDist)
                {
                    target = obj.gameObject;
                    minDist = Vector3.Distance(transform.position, target.transform.position);
                }
            }
            // ensure the free look camera is active, and move the targeting arrow to the right place
            lockOnCamera.Priority = 1;
            targetingArrow.targeting = false;
            targetingArrow.target = target;
        }
    }
    // Fixed Update is called on a consistant basis
    void FixedUpdate(){
        //Destructible destruct = target.GetComponent<Destructible>(); //maybe we want some abilitys to hit non-enemeies???? Food for thought otherwise whoops I have another layer of classes  for no reason :shrugs:
        Enemy enemy = target.GetComponent<Enemy>();
        UpdateCDs();
        UIUpdate();
        if(lastAutoSwing >= autoSwingTimer && Vector3.Distance(transform.position, target.gameObject.transform.position) <= swingRange){
            lastAutoSwing = 0;
            enemy.TakeDamage(autoDmg); 
            animator.SetTrigger("aa");
        }
        //Slice
        if(lastSliceCall >= sliceCD && Input.GetKey("1")){
            lastSliceCall = 0;
            enemy.TakeDamage(sliceDmg);
            animator.SetTrigger("coolAttack");
            textCooldownSlice.gameObject.SetActive(true);
        }
        //Dizzy
        if(lastDizzyCall >= dizzyCD && Input.GetKey("2")){
            lastDizzyCall = 0;
            enemy.MakeDizzy();
            enemy.TakeDamage(dizzyDmg);
            textCooldownDizzy.gameObject.SetActive(true);
        
        }
        // Unsurprising Slash
        if(lastUnsurprisingCall >= unsurprisingCD && Input.GetKey("3") && Vector3.Distance(transform.position, target.gameObject.transform.position) <= swingRange){
            lastUnsurprisingCall = 0;
            if(enemy.isDizzy){
                enemy.TakeDamage(unsurprisingDmg * 20);
                animator.SetTrigger("coolAttack");
            }
            else{
                enemy.TakeDamage(unsurprisingDmg);
                animator.SetTrigger("aa");
            }
            textCooldownUnsurprising.gameObject.SetActive(true);
        }

    }

    private void UpdateCDs(){
        lastAutoSwing += Time.fixedDeltaTime;
        lastSliceCall += Time.fixedDeltaTime;
        lastDizzyCall += Time.fixedDeltaTime;
        lastUnsurprisingCall += Time.fixedDeltaTime;
    }

    private void UIUpdate(){
        UICDImage(lastSliceCall, sliceCD, ref imageCooldownSlice, ref textCooldownSlice);
        UICDImage(lastDizzyCall, dizzyCD, ref imageCooldownDizzy, ref textCooldownDizzy);
        UICDImage(lastUnsurprisingCall, unsurprisingCD, ref imageCooldownUnsurprising, ref textCooldownUnsurprising);
    }

    private void UICDImage(float lastCall, float CD, ref Image imageCooldown, ref TMP_Text textCooldown){
        if(lastCall >= CD){
            //TODO: maybe also pass in background alpha as a conditional here? just one more lmao
            textCooldown.gameObject.SetActive(false);
            imageCooldown.fillAmount = 0.0f;
        }
        else{
            textCooldown.text = Mathf.RoundToInt(CD - lastCall).ToString();
            imageCooldown.fillAmount = 1 - (lastCall / CD);
        }
    }


}
