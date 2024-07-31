namespace Assets.Script.Mia
{
    public class MiaStateJump : MiaStatesAirBase
    {
        public MiaStateJump(MiaController _player, MiaFSM _FSM, string _animName) : base(_player, _FSM, _animName)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (Player.RB.velocity.y < 0 )
            {
                FSM.ChangeState(Player.FallState);
                return;
            }
        }
    }
}
