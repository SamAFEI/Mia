namespace Assets.Script.Mia
{
    public class MiaStateWallJump : MiaStatesAirBase
    {
        public MiaStateWallJump(MiaController _player, MiaFSM _FSM, string _animName) : base(_player, _FSM, _animName)
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
            if(!Player.IsWallJumping)
            {
                FSM.ChangeState(Player.FallState);
                return;
            }
        }
    }
}
