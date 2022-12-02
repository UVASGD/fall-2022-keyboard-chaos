using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    private bool RPressed = false;

    private List<Destructible> enemies = new List<Destructible>();

    // Start is called before the first frame update
    void Start()
    {
        RPressed = false;
        Destructible[] possibleTargets = FindObjectsOfType<Destructible>();
        foreach(Destructible e in possibleTargets){
            if(e.GetComponent<Player>() == null){
                enemies.Add(e);
            }
        }
    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.R) && !RPressed)
        {
            Restart();
            RPressed = true;
        }
    }

    private void FixedUpdate()
    {
        // check to see if all enemies are dead, proceed to next level if so
        // note: may not be very performant if there are many enemies, but it should be fine if there aren't a ton
        bool allEnemiesDead = true;
        foreach(Destructible enemy in enemies){
            if(enemy.alive){
                allEnemiesDead = false;
            }
        }
        if(allEnemiesDead){
            winLevel();
        }
    }

    public void winLevel()
    {
        //FindObjectOfType<AudioManager>().Play("WinLevel");
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void Restart()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("WipeOut");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }
}
