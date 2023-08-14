using Assets.RobustFSM.Mono;
using Assets.SoccerGameEngine_Basic_.Scripts.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ChaseBall.MainState;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ControlBall.MainState;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.GoToHome.MainState;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.Init.MainState;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.KickBall.MainState;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.PickOutThreat.MainState;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ReceiveBall.MainState;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.SupportAttacker.MainState;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.Tackled.MainState;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.TacklePlayer.MainState;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.TakeKickOff.MainState;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.Wait.MainState;

namespace Assets.SoccerGameEngine_Basic_.Scripts.StateMachines.Entities
{
    public class InFieldPlayerFSM : MonoFSM<Player>
    {
        public override void AddStates()
        {
            base.AddStates();

            //set the manual sexecute time
            SetUpdateFrequency(0.5f);

            //add the states
            AddState<ChaseBallMainState>();
            AddState<ControlBallMainState>();
            AddState<GoToHomeMainState>();
            AddState<InitMainState>();
            AddState<KickBallMainState>();
            AddState<PickOutThreatMainState>();
            AddState<ReceiveBallMainState>();
            AddState<SupportAttackerMainState>();
            AddState<TackleMainState>();
            AddState<TackledMainState>();
            AddState<TakeKickOffMainState>();
            AddState<WaitMainState>();

            //set the inital state
            SetInitialState<InitMainState>();
        }
    }
}
