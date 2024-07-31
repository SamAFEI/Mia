namespace Assets.Script.Mia
{
    public class MiaStateAirHeavyAttack : MiaStatesAttackBase
    {
        public MiaStateAirHeavyAttack(MiaController _player, MiaFSM _FSM, string _animName) : base(_player, _FSM, _animName)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            damage = Player.HeavyAttackDamage;
        }

        public override void OnExit()
        {
            base.OnExit();
            Player.SetHeavyAttacking(false);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }
    }
}
