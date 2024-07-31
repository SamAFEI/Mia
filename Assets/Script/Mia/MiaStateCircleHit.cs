using Assets.Script.Manager;
using Assets.Script.Mia;
using UnityEngine;

public class MiaStateCircleHit : MiaStatesAttackBase
{
    public MiaStateCircleHit(MiaController _player, MiaFSM _FSM, string _animName) : base(_player, _FSM, _animName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Player.WeaponTrails.SetActive(false);
        AudioManager.Instance.StartCoroutine(AudioManager.Instance.PlayAttack(true));
        Player.PlayCircleHitFX();
        stateTime = 5f;
        Player.AttackDamage = damage * 2;
        Player.SetSuperArmor(true);
    }

    public override void OnExit()
    {
        base.OnExit();
        EffectManager.Instance.DestroyCircleHitFXobj();
        AudioManager.Instance.StopAttack();
        Player.CircleHitFilled.fillAmount = 1;
        Player.SetCircleHit(false);
        Player.SetSuperArmor(false);
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
