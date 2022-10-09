using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Windows;

public class InputAccessor : MonoBehaviour
{
    // ALL CALLS MADE TO tap(), press(), AND release() MUST BE MADE IN PreUpdate()
    // ALL CALLS MADE TO GETTER METHODS MUST BE MADE IN Update()

    private Dictionary<string, bool> ins = new Dictionary<string, bool>();
    private Dictionary<string, bool> pressed = new Dictionary<string, bool>();
    private Dictionary<string, bool> released = new Dictionary<string, bool>();
    private Dictionary<string, bool> tapped = new Dictionary<string, bool>();

    // Start is called before the first frame update
    void Start()
    {
        //all possible inputs go here
        addInput("a");
        addInput("b");
        addInput("c");
    }

    void LateUpdate()
    {
        foreach (KeyValuePair<string, bool> entry in ins)
        {
            if (pressed[entry.Key] && ins[entry.Key]) pressed[entry.Key] = false;
            if (released[entry.Key] && !ins[entry.Key]) released[entry.Key] = false;
            if (tapped[entry.Key] && pressed[entry.Key]) release(entry.Key);
        }
    }

    // ALL CALLS MADE TO tap(), press(), AND release() MUST BE MADE IN PreUpdate()
    public void tap(string input)
    {
        ins[input] = true;
        pressed[input] = true;
        released[input] = false;
        tapped[input] = true;
    }

    public void press(string input)
    {
        ins[input] = true;
        pressed[input] = true;
        released[input] = false;
    }

    public void release(string input)
    {
        ins[input] = false;
        pressed[input] = false;
        released[input] = true;
    }

    // ALL CALLS MADE TO GETTER METHODS MUST BE MADE IN Update()
    public bool getInput(string input)
    {
        //returns the whether or not this input is activated, if the input does not exist it returns false
        if (!ins.ContainsKey(input)) { Debug.Log("Unrecognized Input"); return false; }
        return ins[input];
    }

    public bool wasPressed(string input)
    {
        //returns the whether or not this input was activated this frame, if the input does not exist it returns false
        if (!ins.ContainsKey(input)) { Debug.Log("Unrecognized Input"); return false; }
        return pressed[input];
    }

    public bool wasReleased(string input)
    {
        //returns the whether or not this input was deactivated this frame, if the input does not exist it returns false
        if (!ins.ContainsKey(input)) { Debug.Log("Unrecognized Input"); return false; }
        return released[input];
    }

    private void addInput(string input)
    {
        ins[input] = false;
        pressed[input] = false;
        released[input] = false;
        tapped[input] = false;
    }
}
