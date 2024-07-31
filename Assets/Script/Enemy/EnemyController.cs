using Assets.Script.Bullet;
using Assets.Script.Manager;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

namespace Assets.Script.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        public EnemyData Data;

        #region COMPONENTS
        public Animator animator { get; private set; }
        public Rigidbody2D RB { get; private set; }
        public EnemyFSM FSM { get; private set; }
        public UI_EnemyStatus uiEnemyStatus { get; private set; }
        public Transform AttackPoint { get; private set; }
        public Collider2D HitCollider { get; private set; }
        public SpriteRenderer Spr { get; private set; }

        public GameObject Bullet;
        public GameObject BloodFX;
        public GameObject RedSoul;
        public GameObject GreedSoul;

        [Header("FX")]
        [SerializeField] private Material flashFXMAT;
        [SerializeField] private Material stunFXMAT;
        private Material originalMAT;
        #endregion

        public bool IsDontAttack;
        public bool IsStunAttack;

        #region STATE PARAMETERS
        public bool IsFacingRight { get; protected set; }
        public bool IsAttacking { get; protected set; }
        public bool IsBattling { get; protected set; }
        public bool CanBeStunned { get; protected set; }
        public int CurrentHP { get; protected set; }
        public int MaxHP { get; protected set; }
        public bool IsDie { get { return CurrentHP <= 0; } }
        public bool IsOnGround { get { return LastOnGroundTime > 0; } }
        public bool IsOnFrontWall { get { return LastOnFrontWallTime > 0; } }
        public bool IsStunning { get { return StunResetTime >= 0; } }
        public bool IsHurting { get { return LastHurtTime >= 0; } }
        [Range(0.5f, 1.5f)] public float BodySize;

        public float AttackDamage;
        public bool IsHeavyAttack;
        #endregion

        #region ENEMY STATE 
        public EnemyStateHurt HurtState { get; private set; }
        public EnemyStateDie DieState { get; private set; }
        public EnemyStateIdle IdleState { get; private set; }
        public EnemyStatePatrol PatrolState { get; private set; }
        public EnemyStateChase ChaseState { get; private set; }
        public EnemyStateAttack1 Attack1State { get; private set; }
        public EnemyStateAttack2 Attack2State { get; private set; }
        public EnemyStateAttack3 Attack3State { get; private set; }
        public EnemyStateStun StunState { get; private set; }
        #endregion

        #region CHECK PARAMETERS
        //Set all of these up in the inspector
        [Header("Checks")]
        [SerializeField] public Transform _groundCheckPoint;
        [SerializeField] public Transform _frontGroundCheckPoint;
        //Size of groundCheck depends on the size of your character generally you want them slightly small than width (for ground) and height (for the wall check)
        [SerializeField] private Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);
        [SerializeField] private float _frontGroundCheckDistance = 2f;
        [SerializeField] private Transform _frontWallCheckPoint;
        [SerializeField] private Vector2 _wallCheckSize = new Vector2(0.5f, 1f);
        [SerializeField] private float _playerCheckDistance;
        #endregion

        #region LAYERS & TAGS
        [Header("Layers & Tags")]
        [SerializeField] protected LayerMask _groundLayer;
        [SerializeField] protected LayerMask _playerLayer;
        #endregion

        #region TIMERS
        public float LastOnGroundTime { get; protected set; }
        public float LastOnFrontWallTime { get; protected set; }
        [HideInInspector] public float LastAttack1Time;
        [HideInInspector] public float LastAttack2Time;
        [HideInInspector] public float LastAttack3Time;
        public float StunResetTime { get; protected set; }
        protected float notChaseDuringTime;
        public float LastHurtTime;
        #endregion

        protected virtual void Awake()
        {
            RB = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            AttackPoint = this.transform.Find("Attack").GameObject().transform;
            HitCollider = AttackPoint.GetComponent<Collider2D>();
            Spr = GetComponent<SpriteRenderer>();
            originalMAT = Spr.material;
            FSM = new EnemyFSM();
            SetFSMState();
            MaxHP = Data.maxHP;
            CurrentHP = MaxHP;
            IsFacingRight = Data.DefaultIsFacingRight;
        }
        protected virtual void Start()
        {
            SetIsBattling(true);
            uiEnemyStatus = GetComponentInChildren<UI_EnemyStatus>();
        }
        protected virtual void Update()
        {
            if (GameManager.Instance.IsPaused) return;
            #region TIMERS
            LastAttack1Time -= Time.deltaTime;
            LastAttack2Time -= Time.deltaTime;
            LastAttack3Time -= Time.deltaTime;
            LastOnGroundTime -= Time.deltaTime;
            LastOnFrontWallTime -= Time.deltaTime;
            StunResetTime -= Time.deltaTime;
            LastHurtTime -= Time.deltaTime;
            #endregion

            #region COLLISION CHECKS
            if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer)) //checks if set box overlaps with ground
            {
                LastOnGroundTime = 0.1f;
            }
            if ( _frontWallCheckPoint != null  
                       && Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _groundLayer))
            {
                LastOnFrontWallTime = 0.1f;
            }
            #endregion
            FSM.CurrentState.OnUpdate();

            if (IsFoundPlayer())
            {
                SetIsBattling(true);
            }
        }
        protected virtual void FixedUpdate()
        {
            FSM.CurrentState.OnFixedUpdate();
            if (!IsHurting && !IsDie && RB.velocity.x != 0)
            {
                CheckIsFacingRight(RB.velocity.x > 0);
            }
        }
        protected virtual void LateUpdate()
        {
            FSM.CurrentState.OnLateUpdate();
        }
        protected virtual void OnDestroy()
        {
            if (!this.gameObject.scene.isLoaded) return; //Load 會有一堆錯誤 用此判斷排除
            GameObject obj;
            Vector2 vector;
            if (RedSoul != null && Data.RedSoulDropRate >= Random.Range(1, 100))
            {
                int count = Random.Range(Data.RedSoulMinDrop, Data.RedSoulMinDrop);
                while (count > 0)
                {
                    obj = Instantiate(RedSoul, transform.position, Quaternion.identity);
                    vector = new Vector2(Random.Range(-3f,3f), Random.Range(3f, 5f));
                    obj.GetComponent<Rigidbody2D>().AddForce(vector, ForceMode2D.Impulse);
                    count--;
                }
            }
            if (GreedSoul != null && Data.GreenSoulDropRate >= Random.Range(1, 100))
            {
                int count = Random.Range(Data.GreenSoulMinDrop, Data.GreenSoulMinDrop);
                while (count > 0)
                {
                    obj = Instantiate(GreedSoul, transform.position, Quaternion.identity);
                    vector = new Vector2(Random.Range(-3f, 3f), 5);
                    obj.GetComponent<Rigidbody2D>().AddForce(vector, ForceMode2D.Impulse);
                    count--;
                }
            }
        }

        #region CHECK METHODS
        public void CheckIsFacingRight(bool isMovingRight)
        {
            if (isMovingRight != IsFacingRight)
                Turn();
        }
        public bool IsFoundPlayer() => Physics2D.Raycast(this.transform.position, Vector2.right * transform.localScale.x, _playerCheckDistance, _playerLayer);
        public bool IsGroundDetaected() => Physics2D.Raycast(_frontGroundCheckPoint.position, Vector2.down, _frontGroundCheckDistance, _groundLayer);
        public float PlayerDistanceX() => GameManager.Instance.Player.transform.position.x - transform.position.x;
        public float PlayerDistance() => Mathf.Abs(PlayerDistanceX());
        public Vector2 PlayerVector() => GameManager.Instance.Player.transform.position - transform.position;
        public bool CanAttack1() => PlayerDistance() < Data.ChaseDistance && LastAttack1Time < 0 && !IsDontAttack;
        public bool CanAttack2() => PlayerDistance() < Data.Attack2Distance && LastAttack2Time < 0 && !IsDontAttack;
        public bool CanAttack3() => PlayerDistance() < Data.RangedDistance && LastAttack3Time < 0
                            && Random.Range(0, 100) < Data.Attack3Rate && !IsDontAttack;
        #endregion

        #region GENERAL METHODS
        public void SetGravityScale(float scale)
        {
            RB.gravityScale = scale;
        }
        public void Turn()
        {
            //stores scale and flips the player along the x axis, 
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;

            //翻轉HP條
            scale = uiEnemyStatus.transform.localScale;
            scale.x *= -1;
            uiEnemyStatus.transform.localScale = scale;

            IsFacingRight = !IsFacingRight;
        }
        public void SetVelocity(float _xVeloctiy, float _yVeloctiy)
        {
            RB.velocity = new Vector2(_xVeloctiy, _yVeloctiy);
        }
        public void MoveToPlayer()
        {
            /*float veloctiyX = 0;
            Vector2 player = PlayerManager.Instance.Player.transform.position;
            if (player.x < transform.position.x)
            {
                veloctiyX = -1;
            }
            else if (player.x > transform.position.x)
            {
                veloctiyX = 1;
            }
            SetVelocity(veloctiyX * Data.runSpeed, RB.velocity.y);
            */
            CheckIsFacingRight(PlayerDistanceX() > 0);
            RB.position = Vector2.MoveTowards(RB.position
                                            , GameManager.Instance.Player.transform.position
                                            , Data.runSpeed * Time.deltaTime);
        }
        public void SetIsBattling(bool _isbattling)
        {
            IsBattling = _isbattling;
        }
        protected virtual void SetFSMState()
        {
            IdleState = new EnemyStateIdle(this, FSM, "Idle");
            PatrolState = new EnemyStatePatrol(this, FSM, "Run");
            ChaseState = new EnemyStateChase(this, FSM, "Run");
            DieState = new EnemyStateDie(this, FSM, "Die");
            HurtState = new EnemyStateHurt(this, FSM, "Hurt");
            StunState = new EnemyStateStun(this, FSM, "Idle");
            Attack1State = new EnemyStateAttack1(this, FSM, "Attack1");
            Attack2State = new EnemyStateAttack2(this, FSM, "Attack2");
            Attack3State = new EnemyStateAttack3(this, FSM, "Attack3");
            FSM.InitState(IdleState);
        }
        public void AnimationTrigger() => FSM.CurrentState.AnimationFinishTrigger();
        #endregion

        #region HURT METHODS
        public virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "PlayerAttack")
            {
                if (IsHurting || IsDie) { return; }
                MiaController _player = collision.GetComponentInParent<MiaController>();
                float damage = _player.AttackDamage * Random.Range(0.90f, 1.2f);
                Repel(collision.transform, false);
                PlayBloodFX(collision.transform);
                StartCoroutine(AudioManager.Instance.PlayHit());
                Vector2 vector = new Vector2(transform.position.x, collision.transform.position.y);
                EffectManager.Instance.DoHitFX(vector);
                Hurt(damage);
            }
        }

        public virtual void Hurt(float _damage, bool _isHeaveyAttack = false)
        {
            if (IsHurting || IsDie) { return; }
            TimerManager.Instance.DoFrozenTime(0.1f);
            CameraManager.Instance.Shake(1f, 0.1f);
            LastHurtTime = Data.HurtResetTime;
            if (_isHeaveyAttack) { LastHurtTime += 0.3f; }
            StartCoroutine(HurtFlashFX());
            CurrentHP = (int)Mathf.Clamp(CurrentHP - _damage, 0, MaxHP);
            uiEnemyStatus.DoLerpHealth(_damage);
            if (IsDie)
            {
                FSM.ChangeState(DieState);
            }
        }

        public void Repel(Transform source, bool isStrong = false)
        {
            Vector2 vector = transform.position - source.position;
            vector = vector.normalized;
            if (IsDie)
            {
                vector = new Vector2(vector.x * 5, 8);
                RB.AddForce(vector, ForceMode2D.Impulse);
            }
            else if (isStrong)
            {
                RB.AddForce(vector * 5, ForceMode2D.Impulse);
            }
            else
            {
                RB.AddForce(vector * 3, ForceMode2D.Impulse);
            }
        }
        #endregion

        #region Attack METHODS
        public virtual void RangedAttack()
        {
            if (Bullet == null) return;

            Vector3 vector = PlayerVector();
            vector = vector.normalized;
            GameObject gObj = Instantiate(Bullet, AttackPoint.position, Quaternion.identity);
            gObj.GetComponent<Rigidbody2D>().AddForce(vector * Data.Attack3BulletSpeed, ForceMode2D.Impulse);
            gObj.GetComponent<ProjectileBase>().Damage = Data.Attack3Damage;
            var angle = -1 * Mathf.Atan2(vector.x, vector.y) * Mathf.Rad2Deg + 90;
            gObj.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        #endregion

        #region STATE METHODS
        public virtual void ChaseStateAction()
        {
            if (CanAttack3())
            {
                FSM.ChangeState(Attack3State);
                return;
            }
            else if (CanAttack1())
            {
                FSM.ChangeState(Attack1State);
                return;
            }
            else if (PlayerDistance() > Data.ChaseDistance)
            {
                MoveToPlayer();
                return;
            }
            else
            {
                FSM.ChangeState(IdleState);
                return;
            }
        }
        #endregion

        #region STUN METHODS
        public virtual void OpenCanStunned()
        {
            CanBeStunned = true;
        }
        public virtual void CloseCanStunned()
        {
            CanBeStunned = false;
        }
        public virtual void Stunned()
        {
            FSM.ChangeState(IdleState);
            StunResetTime = Data.StunResetTime;
            StartCoroutine(StunFlashFX());
        }
        #endregion

        #region FX METHODS
        public void PlayBloodFX(Transform source)
        {
            if (BloodFX == null) { return; }
            GameObject obj = Instantiate(BloodFX
                                    , transform.position
                                    , Quaternion.identity);

            Vector2 vector = transform.position - source.position;
            vector = vector.normalized;
            if (vector.x < 0)
            {
                obj.transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                obj.transform.localScale = new Vector3(1, 1, 1);
            }
            Destroy(obj, 1f);
        }
        public IEnumerator HurtFlashFX()
        {
            if (IsStunning) { yield break; }
            Spr.material = flashFXMAT;
            yield return new WaitForSeconds(.2f);
            Spr.material = originalMAT;
        }
        public IEnumerator StunFlashFX()
        {
            while (IsStunning)
            {
                Spr.material = stunFXMAT;
                yield return new WaitForSeconds(.1f);
                Spr.material = originalMAT;
                yield return new WaitForSeconds(.1f);
                if (IsDie) break;
            }
        }
        #endregion

        #region EDIT METHODS
        public virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            if (_groundCheckPoint != null)
            {
                Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);
            }
            Gizmos.color = Color.blue;
            if (_frontWallCheckPoint != null)
            {
                Gizmos.DrawWireCube(_frontWallCheckPoint.position, _wallCheckSize);
            }
            Gizmos.color = Color.red;
            if (_frontWallCheckPoint != null)
            {
                Gizmos.DrawLine(_frontWallCheckPoint.position
                , new Vector3(_frontWallCheckPoint.position.x + (_playerCheckDistance * transform.localScale.x), _frontWallCheckPoint.position.y));
            }
        }
        #endregion
    }
}
