using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    [SerializeField] InputAccessor input;
    [SerializeField] ActionExecuter ae;

    bool disabled = false;
    bool inAction = false;
    bool comboAvailible = false;

    // Update is called once per frame
    void Update()
    {
        if (disabled) return;
        if (inAction) return;

        KeyValuePair<int, int> temp = ae.doActions(input, comboAvailible);
        waitForActionEnd(temp.Key);
        waitForComboEnd(temp.Value + temp.Key);
        
    }
    
    public IEnumerator waitForActionEnd(float seconds)
    {
        inAction = true;
        yield return new WaitForSeconds(seconds);
        inAction = false;
    }

    public IEnumerator waitForComboEnd(float seconds)
    {
        comboAvailible = true;
        yield return new WaitForSeconds(seconds);
        comboAvailible = false;
    }
}
