namespace Assets.Script.Mia
{
    public class MiaStatesAirBase : MiaState
    {
        public MiaStatesAirBase(MiaController _player, MiaFSM _FSM, string _animName) : base(_player, _FSM, _animName)
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

            if (Player.IsOnGround)
            {
                FSM.ChangeState(Player.LandState);
                return;
            }
            if (Player.IsThreeCombosing)
            {
                FSM.ChangeState(Player.ThreeCombosState);
                return;
            }
            if (Player.IsAttacking && !Player.IsSkilling)
            {
                if (Player.IsHeavyAttacking)
                {
                    FSM.ChangeState(Player.AirHeavyAttackState);
                    return;
                }
                else
                {
                    FSM.ChangeState(Player.AirAttackState);
                    return;
                }
            }
            if (Player.IsSliding)
            {
                FSM.ChangeState(Player.SlideState);
                return;
            }
        }
    }
}
