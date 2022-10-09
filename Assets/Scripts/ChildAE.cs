using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildAE : ActionExecuter
{
    public override void update()
    {
        
    }

    public override KeyValuePair<int, int> doActions(InputAccessor input, bool comboAvailible)
    {
        return new KeyValuePair<int, int>(1,1);
    }
}
