using UnityEngine;

namespace Assets.Script.Mia
{
    public class MiaStateDash : MiaState
    {
        public MiaStateDash(MiaController _player, MiaFSM _FSM, string _animName) : base(_player, _FSM, _animName)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Player.shadow.ShadowBone = Player.boneRight;
            Player.shadow.maxAfterTime = Player.Data.dashEndTime+0.2f;
            Player.shadow.IsSpawn = true;
            Player.SetAttackEffect(true);
            AudioManager.Instance.StartCoroutine(AudioManager.Instance.PlayDash());
            Player.SetSuperArmor(true);
            Player.AttackDamage = 10f;
        }

        public override void OnExit()
        {
            base.OnExit();
            Player.SetAttackEffect(false);
            AudioManager.Instance.StopDash();
            Player.SetSuperArmor(false);
            Player.shadow.IsSpawn = false;
        }

        public override void OnUpdate()
        {
            if (!Player.IsDashing)
            {
                base.OnUpdate();
                if (Player.IsOnGround)
                {
                    FSM.ChangeState(Player.IdleState);
                }
                else
                {
                    FSM.ChangeState(Player.FallState);
                }
            }
            else
            {
                //Player.AttackHits(Player.Data.attackDamage);
            }
        }
    }
}
