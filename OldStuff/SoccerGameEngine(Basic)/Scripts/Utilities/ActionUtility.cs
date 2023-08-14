using System;

namespace Assets.SoccerGameEngine_Basic_.Scripts.Utilities
{
    public class ActionUtility
    {
        public static void Invoke_Action(Action action)
        {
            //get the action
            Action temp = action;

            //run action if not null
            if (temp != null) temp.Invoke();
        }
    }
}
