using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Destructible
{
    public bool isDizzy = false;
    //This function will handle auto attacking once I implement that since auto attacking is fairly standard among a bunch of enemies even if its not whatever idk

    //maybe also implement the dizzy or the break/topple/launch/smash thing here 
    
    public virtual void MakeDizzy(){
        //if we have dizzy resistance as like a trait it'd go here
        isDizzy = true;
        StartCoroutine(BeDizzy());
    }

    IEnumerator BeDizzy(){
        yield return new WaitForSeconds(7); 
        isDizzy = false;
    }
    
    //we may wanna override some of Destructible's virtual functions, but for the demo it should be fine.
}
