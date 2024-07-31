namespace Assets.Script.Mia
{
    public class MiaStateRun : MiaStatesGroundBase
    {
        public MiaStateRun(MiaController _player, MiaFSM _FSM, string _animName) : base(_player, _FSM, _animName)
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

            if (Player.InputX == 0)
            {
                FSM.ChangeState(Player.StopState);
                return;
            }
        }

        public override void OnLateUpdate()
        {
            base.OnLateUpdate();
            AudioManager.Instance.StartCoroutine(AudioManager.Instance.PlayFootsteps());
        }
    }
}
