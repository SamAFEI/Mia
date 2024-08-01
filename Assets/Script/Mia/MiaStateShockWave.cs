using Assets.Script.Mia;
using UnityEngine;

public class MiaStateShockWave : MiaStatesAttackBase
{
    public MiaStateShockWave(MiaController _player, MiaFSM _FSM, string _animName) : base(_player, _FSM, _animName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Player.SetAttackEffect(false);
        Player.PlayShockWaveFX();
        AudioManager.Instance.StartCoroutine(AudioManager.Instance.PlayShockWave());
        Player.ShockWaveCollider.enabled = true;
        stateTime = 0.5f;
        Player.SetGravityScale(0); 
        Player.RB.velocity = Vector2.zero; 
        Player.AttackDamage = damage * 2;
        //Player.SetSuperArmor(true);
    }
    public override void OnExit()
    {
        base.OnExit();
        Player.ShockWaveCollider.enabled = false;
        Player.ShockWaveFilled.fillAmount = 1;
        Player.SetShockWave(false);
        Player.SetSuperArmor(false);
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        Player.SetGravityScale(0);
        Player.RB.velocity = Vector2.zero;
        if (stateTime < 0)
        {
            FSM.ChangeState(Player.IdleState);
            return;
        }
    }
}
