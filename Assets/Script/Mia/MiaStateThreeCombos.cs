using Assets.Script.Mia;

public class MiaStateThreeCombos : MiaStatesAttackBase
{
    public MiaStateThreeCombos(MiaController _player, MiaFSM _FSM, string _animName) : base(_player, _FSM, _animName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Player.WeaponTrails.SetActive(false);
        AudioManager.Instance.StartCoroutine(AudioManager.Instance.PlayAttack(true));
        stateTime = 1.2f;
        Player.AttackDamage = damage * 2f;
    }
    public override void OnExit()
    {
        base.OnExit();
        AudioManager.Instance.StopAttack();
        Player.ThreeCombosFilled.fillAmount = 1;
        Player.SetThreeCombos(false);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (stateTime < 0)
        {
            FSM.ChangeState(Player.IdleState);
            return;
        }
    }
}
