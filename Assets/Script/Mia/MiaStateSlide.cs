namespace Assets.Script.Mia
{
    public class MiaStateSlide : MiaStatesAirBase
    {
        public MiaStateSlide(MiaController _player, MiaFSM _FSM, string _animName) : base(_player, _FSM, _animName)
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
            if (Player.IsWallJumping)
            {
                FSM.ChangeState(Player.WallJumpState);
                return;
            }
            if (!Player.IsSliding)
            {
                FSM.ChangeState(Player.FallState);
                return;
            }
        }
    }
}
