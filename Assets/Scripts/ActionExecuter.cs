using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionExecuter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        update();
    }

    //use update() to do anything you want really
    public abstract void update();
    //use doActions() to process input, should an int to indicate the time the ActionManager should wait to
    //call the next doAction(), and a second int to indicate the time the time a combo should be availible for
    //uses a key value pair because I thought that was easier than Object[]
    public abstract KeyValuePair<int, int> doActions(InputAccessor input, bool comboAvailible);
}
