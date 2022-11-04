using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityLibrary : MonoBehaviour
{
    //variables for the class structure, please DO NOT add anything here
    private GameObject player;
    private Animator animator;
    [SerializeField] Destructible target;   //need to generalize, see the thing below
    private Player playerMovement;
    private GameObject cameraTarget;

    [SerializeField] string[] names;
    [SerializeField] Image[] UIimages;
    [SerializeField] TMP_Text[] UItexts;

    Dictionary<string, Ability> abilites = new Dictionary<string, Ability>();

    //you can add things below, please DO NOT add anything above
    Ability slice, dizzy, unsurprisingSlash, fireSpell, defaultAttack;        //might need to turn this into an array later if it gets out of control

    [SerializeField] GameObject defaultSpellBallPrefab;
    double meleeRange = 4;

    //TO ADD AN ABILITY
    //1. add the name to the line directly above this
    //2. define it in start
    //3. add it to the abilities list in start with the addAbility() method
    //4. add an if statement in update and useAbility(), ie if (condition1 && condition2 && attack1.useAbility()), the useAbility method will manage the cooldown, the if statement will not be entered if the ability is not cooled down
    //5. add any stuff you want to the if statement body (effects, extra damage, etc), the use ability will handle the base damage, base animation, and the cooldown UI
    //6. add the name and UI to the appropriate list fields of this script in the Unity Inspector Window
    //make sure everything is spelled correctly, make sure to use double ands (&&) in the if loops

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<Player>();
        target = playerMovement.target;
        player = gameObject;
        animator = player.GetComponent<Animator>();
        if (UIimages.Length != UItexts.Length) { Debug.Log("Ability UI array length mismatch"); }

        //abilites
        slice = new Ability("slice", player, 10, 10, "coolAttack");
        dizzy = new Ability("dizzy", player, 2, 20, "");
        unsurprisingSlash = new Ability("unsurprisingSlash", player, 1, 6, "aa");
        fireSpell = new Ability("fireSpell", player, 0, 3, "aa");
        defaultAttack = new Ability("defaultAttack", player, 4, 5, "coolAttack");


        //add abilities to the abilities list (with UI)
        addAbility(slice);
        addAbility(dizzy);
        addAbility(unsurprisingSlash);
        addAbility(fireSpell);

        //add abilities to the abilities list (without UI)
        addAbility(defaultAttack);

    }

    // Update is called once per frame
    void Update()
    {
        target = playerMovement.target;
        Enemy enemy = target.GetComponent<Enemy>(); //hopefully this whole target and enemy thing can be generalized, I will get to this later though
        //actually in order to get this to be more general to other enemies/multiple enimies/hitboxes/effects/special damage, I think the ability might just store the cooldown and everything else can be done in the if, we'll see

        //slice
        if (Input.GetKey("1") && slice.tryUseAbility(target)) { }

        //dizzy
        if (Input.GetKey("2") && dizzy.tryUseAbility(target))
        {
            enemy.MakeDizzy();
        }

        //unsurprising
        if (Input.GetKey("3") && inRange(meleeRange) && unsurprisingSlash.tryUseAbility(target))
        {
            if (enemy.isDizzy)
            {
                enemy.TakeDamage(unsurprisingSlash.damageToEnemy * 20);
                animator.SetTrigger("coolAttack");
            }
        }

        //fireSpell
        if (Input.GetKey("7") && fireSpell.tryUseAbility(target))
        {
            GameObject tempSpellBall = Instantiate(defaultSpellBallPrefab, player.transform.position + transform.forward + transform.up, player.transform.rotation);
            tempSpellBall.GetComponent<Rigidbody>().velocity = transform.forward * 10;

        }

        //defaultAttack
        if (inRange(meleeRange) && defaultAttack.tryUseAbility(target)) { }

        //Other stuff
        UIUpdate();
    }

    //methods for making ability if statements easier
    private bool inRange(double range)
    {
        return Vector3.Distance(transform.position, target.gameObject.transform.position) <= range;
    }


    //the stuff below this comment is not important if you're just here to add an ability
    private void UIUpdate()
    {
        for (int i = 0; i < UIimages.Length; i++)
        {
            Image image = UIimages[i];
            TMP_Text text = UItexts[i];
            Ability a = abilites[names[i]];
            UICDImageUpdate(a.timeSinceLastUse(), a.coolDown, ref image, ref text);
        }
    }

    private void UICDImageUpdate(float lastCall, float CD, ref Image imageCooldown, ref TMP_Text textCooldown)
    {
        if (lastCall >= CD)
        {
            //TODO: maybe also pass in background alpha as a conditional here? just one more lmao
            textCooldown.gameObject.SetActive(false);
            imageCooldown.fillAmount = 0.0f;
        }
        else
        {
            textCooldown.gameObject.SetActive(true);
            textCooldown.text = Mathf.RoundToInt(CD - lastCall).ToString();
            imageCooldown.fillAmount = 1 - (lastCall / CD);
        }
    }

    private void addAbility(Ability a)
    {
        abilites.Add(a.name, a);
    }
}
