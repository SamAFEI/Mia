using Assets.Script.Manager;

namespace Assets.Script.Mia
{
    public class MiaStateLand : MiaStatesGroundBase
    {
        public MiaStateLand(MiaController _player, MiaFSM _FSM, string _animName) : base(_player, _FSM, _animName)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Player.DoLandFX();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                
            if (Player.InputX != 0)
            {
                FSM.ChangeState(Player.RunState);
                return;
            }
            if (isAnimFinish)
            {
                FSM.ChangeState(Player.IdleState);
                return;
            }
        }
    }
}
