using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.Team.KickOff.SubStates;
using RobustFSM.Base;

namespace Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.Team.KickOff.MainState
{
    public class KickOffMainState : BHState
    {
        public override void AddStates()
        {
            //add the states
            AddState<TakeKickOff>();
            AddState<PrepareForKickOff>();
            AddState<WaitForKickOff>();

            //set the initial state
            SetInitialState<PrepareForKickOff>();
        }
    }
}
