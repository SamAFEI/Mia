using Assets.Script.Bullet;
using Assets.Script.Collectibles;
using Assets.Script.Enemy;
using Assets.Script.Manager;
using Assets.Script.Mia;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MiaController : MonoBehaviour
{
    public PlayerData Data;

    #region COMPONENTS
    public Rigidbody2D RB { get; private set; }
    public MiaFSM FSM { get; private set; }
    public Animator animator { get; private set; }
    public UI_PlayerStatus uiPlayerStatus { get; private set; }

    public Shadow shadow { get; private set; }
    public GameObject boneSide;
    public GameObject boneRight;
    public GameObject WeaponTrails;
    public GameObject WeaponPoint;
    public GameObject AttackPoint;
    public GameObject ParryPoint;
    public Collider2D CollectCollider;
    public Collider2D ShockWaveCollider;
    private SpriteRenderer[] spritList;
    public Image DashFilled;
    public Image ThreeCombosFilled;
    public Image CircleHitFilled;
    public Image ShockWaveFilled;

    [Header("FX")]
    [SerializeField] private Material flashFXMAT;
    private Material originalMAT;
    #endregion

    #region PLAYER STATE
    public int CurrentHP { get; private set; }
    public int MaxHP { get; private set; }
    public int HeavyAttackDamage { get; private set; }
    public int CircleAttackDamage { get; private set; }
    public int RedSouls;
    public bool IsHeavyAttack;
    public float AttackDamage;
    #endregion

    #region FSM STATE 
    public MiaStateHurt HurtState { get; private set; }
    public MiaStateStun StunState { get; private set; }
    public MiaStateStandUp StandUpState { get; private set; }
    public MiaStateDie DieState { get; private set; }
    public MiaStateDash DashState { get; private set; }
    public MiaStateIdle IdleState { get; private set; }
    public MiaStateRun RunState { get; private set; }
    public MiaStateStop StopState { get; private set; }
    public MiaStateDodge DodgeState { get; private set; }
    public MiaStateAttack AttackState { get; private set; }
    public MiaStateHeavyAttack HeavyAttackState { get; private set; }
    public MiaStateParry ParryState { get; private set; }

    public MiaStateJump JumpState { get; private set; }
    public MiaStateFall FallState { get; private set; }
    public MiaStateLand LandState { get; private set; }
    public MiaStateSlide SlideState { get; private set; }
    public MiaStateWallJump WallJumpState { get; private set; }
    public MiaStateCounter CounterState { get; private set; }
    public MiaStateAirHike AirHikeState { get; private set; }
    public MiaStateAirAttack AirAttackState { get; private set; }
    public MiaStateAirHeavyAttack AirHeavyAttackState { get; private set; }
    public MiaStateThreeCombos ThreeCombosState { get; private set; }
    public MiaStateCircleHit CircleHitState { get; private set; }
    public MiaStateShockWave ShockWaveState { get; private set; }
    #endregion

    #region CONTROL PARAMETERS
    //Variables control the various actions the player can perform at any time.
    //These are fields which can are public allowing for other sctipts to read them
    //but can only be privately written to.
    public bool IsFacingRight { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsWallJumping { get; private set; }
    public bool IsDashing { get; private set; }
    public bool IsSliding { get; private set; }
    public bool IsDodging { get; private set; }
    public bool IsParrying { get; private set; }
    public bool IsStunning { get; private set; }
    public bool IsSuperArmoring { get; private set; }
    public bool IsAttacking { get; private set; }
    public bool IsHeavyAttacking { get; private set; }
    public bool IsOnGround { get { return LastOnGroundTime > 0; } }
    public bool IsPressedAttack { get { return LastPressedAttackTime > 0; } }
    public bool IsPressedHeavyAttack { get { return LastPressedHeavyAttackTime > 0; } }
    public bool IsPressedParry { get { return LastPressedParryTime > 0; } }
    public bool IsUpParry { get { return LastUpParryTime > 0; } }
    public bool CanCounter { get { return CanCounterTime > 0; } }
    public bool IsHoldDownAttack => (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.JoystickButton2));
    public bool IsHoldDownHeavyAttack => (Input.GetKey(KeyCode.Mouse2) || Input.GetKey(KeyCode.K) || Input.GetKey(KeyCode.JoystickButton3));
    public bool IsThreeCombosing { get; private set; }
    public bool IsCircleHiting { get; private set; }
    public bool IsShockWaving { get; private set; }
    public bool IsSkilling { get; private set; }
    public bool IsHurting { get { return LastHurtTime >= 0; } }
    public bool IsDefending { get { return IsParrying && CanCounterTime < 0; } }
    public float DashCD { get; private set; }
    public float ThreeCombosCD { get; private set; }
    public float CircleHitCD { get; private set; }
    public float ShockWaveCD { get; private set; }

    public bool CanInputMove;

    //Timers (also all fields, could be private and a method returning a bool could be used)
    public float LastOnGroundTime { get; private set; }
    public float LastOnWallTime { get; private set; }
    public float LastOnWallRightTime { get; private set; }
    public float LastOnWallLeftTime { get; private set; }
    public float LastHurtTime;
    public float LastPressedParryTime;
    public float LastUpParryTime;
    public float CanCounterTime;

    //Jump
    private bool _isJumpCut;
    private bool _isJumpFalling;

    //Wall Jump
    private float _wallJumpStartTime;
    private int _lastWallJumpDir;

    //Dash
    private int _dashesLeft;
    private bool _dashRefilling;
    private Vector2 _lastDashDir;
    private bool _isDashAttacking;
    #endregion

    #region INPUT PARAMETERS
    private Vector2 _moveInput;
    public float InputX { get { return _moveInput.x; } }
    public float InputY { get { return _moveInput.y; } }
    public float LastPressedJumpTime { get; private set; }
    public float LastPressedDashTime { get; private set; }
    public float LastPressedDodgeTime { get; private set; }
    public float LastPressedAttackTime { get; private set; }
    public float LastPressedHeavyAttackTime { get; private set; }
    public float NextDashTime { get; private set; }
    public float NextDodgeTime { get; private set; }
    public float LastPressedThreeCombosTime { get; private set; }
    public float LastPressedCircleHitTime { get; private set; }
    public float LastPressedShockWaveTime { get; private set; }

    #endregion

    #region CHECK PARAMETERS
    //Set all of these up in the inspector
    [Header("Checks")]
    [SerializeField] private Transform _groundCheckPoint;
    //Size of groundCheck depends on the size of your character generally you want them slightly small than width (for ground) and height (for the wall check)
    [SerializeField] private Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);
    [Space(5)]
    [SerializeField] private Transform _frontWallCheckPoint;
    [SerializeField] private Transform _backWallCheckPoint;
    [SerializeField] private Vector2 _wallCheckSize = new Vector2(0.5f, 1f);
    #endregion

    #region LAYERS & TAGS
    [Header("Layers & Tags")]
    [SerializeField] private LayerMask _groundLayer;
    #endregion

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        shadow = GetComponent<Shadow>();
        spritList = boneSide.GetComponentsInChildren<SpriteRenderer>();
        spritList = spritList.Concat(boneRight.GetComponentsInChildren<SpriteRenderer>()).ToArray();
        originalMAT = spritList[0].material;
        FSM = new MiaFSM();

        DieState = new MiaStateDie(this, FSM, "Stun");
        HurtState = new MiaStateHurt(this, FSM, "Hurt");
        StunState = new MiaStateStun(this, FSM, "Stun");
        StandUpState = new MiaStateStandUp(this, FSM, "StandUp");
        DashState = new MiaStateDash(this, FSM, "Dash");

        DodgeState = new MiaStateDodge(this, FSM, "Dodge");
        IdleState = new MiaStateIdle(this, FSM, "Idle");
        RunState = new MiaStateRun(this, FSM, "Run");
        StopState = new MiaStateStop(this, FSM, "Stop");
        AttackState = new MiaStateAttack(this, FSM, "");
        HeavyAttackState = new MiaStateHeavyAttack(this, FSM, "HeavyAttack");
        ParryState = new MiaStateParry(this, FSM, "Parry");

        JumpState = new MiaStateJump(this, FSM, "Jump");
        FallState = new MiaStateFall(this, FSM, "Fall");
        LandState = new MiaStateLand(this, FSM, "Land");
        AirHikeState = new MiaStateAirHike(this, FSM, "AirHike");
        SlideState = new MiaStateSlide(this, FSM, "Slide");
        WallJumpState = new MiaStateWallJump(this, FSM, "WallJump");
        CounterState = new MiaStateCounter(this, FSM, "Attack2");
        AirAttackState = new MiaStateAirAttack(this, FSM, "");
        AirHeavyAttackState = new MiaStateAirHeavyAttack(this, FSM, "AirHeavyAttack");
        ThreeCombosState = new MiaStateThreeCombos(this, FSM, "ThreeCombos");
        CircleHitState = new MiaStateCircleHit(this, FSM, "CircleHit");
        ShockWaveState = new MiaStateShockWave(this, FSM, "ShockWave");

        FSM.InitState(IdleState);
        SetAttackEffect(false);
        SetParrying(false);
        CanInputMove = true;
    }

    private void Start()
    {
        uiPlayerStatus = GameObject.FindObjectOfType<UI_PlayerStatus>();
        SetGravityScale(Data.gravityScale);
        IsFacingRight = true;
        MaxHP = Data.maxHP;
        CurrentHP = MaxHP;
        DashCD = 2f;
        ThreeCombosCD = 3f;
        CircleHitCD = 5f;
        ShockWaveCD = 3f;
    }

    private void Update()
    {
        if (GameManager.Instance.IsPaused)
        {
            SetGravityScale(0);
            return;
        }
        #region TIMERS
        LastOnGroundTime -= Time.deltaTime;
        LastOnWallTime -= Time.deltaTime;
        LastOnWallRightTime -= Time.deltaTime;
        LastOnWallLeftTime -= Time.deltaTime;

        LastPressedJumpTime -= Time.deltaTime;
        LastPressedDashTime -= Time.deltaTime;
        LastPressedDodgeTime -= Time.deltaTime;
        LastPressedAttackTime -= Time.deltaTime;
        LastPressedHeavyAttackTime -= Time.deltaTime;
        LastPressedParryTime -= Time.deltaTime;
        LastUpParryTime -= Time.deltaTime;
        CanCounterTime -= Time.deltaTime;
        NextDodgeTime -= Time.deltaTime;
        NextDashTime -= Time.deltaTime;
        LastPressedThreeCombosTime -= Time.deltaTime;
        LastPressedCircleHitTime -= Time.deltaTime;
        LastPressedShockWaveTime -= Time.deltaTime;
        LastHurtTime -= Time.deltaTime;
        #endregion

        SetAttackPoint();
        FSM.CurrentState.OnUpdate();

        #region INPUT HANDLER
        if (CanInputMove && !IsHurting && !GameManager.Instance.IsDie)
        {
            _moveInput.x = Input.GetAxisRaw("Horizontal");
            _moveInput.y = Input.GetAxisRaw("Vertical");

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton1))
            {
                OnJumpInput();
            }
            if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.JoystickButton1))
            {
                OnJumpUpInput();
            }
            if (Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.JoystickButton7))
            {
                OnDashInput();
            }
            //if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Mouse2) || Input.GetKeyDown(KeyCode.JoystickButton6))
            //{
            //    OnDodgeInput();
            //}
            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.JoystickButton2))
            {
                OnAttackInput();
            }
            //if (Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.JoystickButton3))
            //{
            //    OnHeavyAttackInput();
            //}
            if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.H) || Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                OnParryInput();
            }
            //foreach (KeyCode kcode in System.Enum.GetValues(typeof(KeyCode)))
            //{
            //    if (Input.GetKeyDown(kcode))
            //        Debug.Log("KeyCode down: " + kcode);
            //}
            if (Input.GetMouseButtonDown(2) || Input.GetKeyDown(KeyCode.K))
            {
                OnThreeCombosInput();
            }

            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.U))
            {
                OnCircleHitInput();
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                OnShockWaveInput();
            }
        }
        if (Input.GetKeyUp(KeyCode.F) || Input.GetKeyUp(KeyCode.H) || Input.GetKeyUp(KeyCode.JoystickButton0))
        {
            OnParryCancelInput();
        }
        if (Input.GetKeyUp(KeyCode.B))
        {
            CurrentHP = 0;
        }
        #endregion

        #region COLLISION CHECKS
        if (!IsJumping)
        {
            //Ground Check
            if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer)) //checks if set box overlaps with ground
            {
                LastOnGroundTime = Data.coyoteTime; //if so sets the lastGrounded to coyoteTime
            }

            //Right Wall Check
            if (((Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && IsFacingRight)
                    || (Physics2D.OverlapBox(_backWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && !IsFacingRight)) && !IsWallJumping)
                LastOnWallRightTime = 0.001f;

            //Right Wall Check
            if (((Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && !IsFacingRight)
                || (Physics2D.OverlapBox(_backWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && IsFacingRight)) && !IsWallJumping)
                LastOnWallLeftTime = 0.001f;

            //Two checks needed for both left and right walls since whenever the play turns the wall checkPoints swap sides
            LastOnWallTime = Mathf.Max(LastOnWallLeftTime, LastOnWallRightTime);
        }
        #endregion

        #region JUMP CHECKS
        if (IsJumping && RB.velocity.y < 0)
        {
            IsJumping = false;

            _isJumpFalling = true;
        }

        if (IsWallJumping && Time.time - _wallJumpStartTime > Data.wallJumpTime)
        {
            IsWallJumping = false;
        }

        if (LastOnGroundTime > 0 && !IsJumping && !IsWallJumping)
        {
            _isJumpCut = false;

            _isJumpFalling = false;
        }

        if (!IsDashing)
        {
            //Jump
            if (CanJump() && LastPressedJumpTime > 0)
            {
                IsJumping = true;
                IsWallJumping = false;
                _isJumpCut = false;
                _isJumpFalling = false;
                Jump();
            }
            //WALL JUMP
            //else if (CanWallJump() && LastPressedJumpTime > 0)
            //{
            //    IsWallJumping = true;
            //    IsJumping = false;
            //    _isJumpCut = false;
            //    _isJumpFalling = false;
            //
            //    _wallJumpStartTime = Time.time;
            //    _lastWallJumpDir = (LastOnWallRightTime > 0) ? -1 : 1;
            //
            //    WallJump(_lastWallJumpDir);
            //}
        }
        #endregion

        #region DASH CHECKS
        if (CanDash() && LastPressedDashTime > 0)
        {

            if (_moveInput.x != 0)
                _lastDashDir = _moveInput;
            else
                _lastDashDir = IsFacingRight ? Vector2.right : Vector2.left;

            _lastDashDir.y = 0;
            CheckDirectionToFace(_lastDashDir.x > 0);

            IsDashing = true;
            IsJumping = false;
            IsWallJumping = false;
            _isJumpCut = false;
            StartCoroutine(StartDash(_lastDashDir));
        }
        #endregion

        #region SLIDE CHECKS
        if (CanSlide() && ((LastOnWallLeftTime > 0 && _moveInput.x < 0) || (LastOnWallRightTime > 0 && _moveInput.x > 0)))
            IsSliding = true;
        else
            IsSliding = false;
        #endregion

        #region DODGE CHECKS
        if (CanDodge() && LastPressedDodgeTime > 0)
        {
            //Freeze game for split second. Adds juiciness and a bit of forgiveness over directional input
            Sleep(Data.dashSleepTime);

            //If not direction pressed, dash forward
            if (_moveInput.x != 0)
                _lastDashDir = _moveInput;
            else
                _lastDashDir = IsFacingRight ? Vector2.right : Vector2.left;

            _lastDashDir.y = 0;
            CheckDirectionToFace(_lastDashDir.x > 0);

            IsDodging = true;
            StartCoroutine(Dodge(_lastDashDir));
        }
        #endregion

        #region ATTACK CHECKS
        if (CanAttack() && IsPressedAttack)
        {
            SetAttacking(true);
        }
        if (CanAttack() && IsPressedHeavyAttack)
        {
            SetHeavyAttacking(true);
        }
        #endregion

        #region PARRY CHECKS
        if (CanParry() && IsPressedParry)
        {
            SetParrying(true);
        }
        if (IsParrying && IsUpParry)
        {
            SetParrying(false);
        }
        #endregion

        #region SKLL CHECKS
        //連擊
        if (CanThreeCombos() && LastPressedThreeCombosTime > 0)
        {
            SetThreeCombos(true);
        }
        //迴旋
        if (CanCircleHit() && LastPressedCircleHitTime > 0)
        {
            SetCircleHit(true);
        }
        //ShockWave
        if (CanShockWave() && LastPressedShockWaveTime > 0)
        {
            SetShockWave(true);
        }
        #endregion

        IsSliding = false;
        #region GRAVITY
        if (!_isDashAttacking)
        {
            //Higher gravity if we've released the jump input or are falling
            if (IsSliding)
            {
                SetGravityScale(0);
            }
            else if (RB.velocity.y < 0 && _moveInput.y < 0)
            {
                //Much higher gravity if holding down
                SetGravityScale(Data.gravityScale * Data.fastFallGravityMult);
                //Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
                RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFastFallSpeed));
            }
            else if (_isJumpCut)
            {
                //Higher gravity if jump button released
                SetGravityScale(Data.gravityScale * Data.jumpCutGravityMult);
                RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFallSpeed));
            }
            else if ((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
            {
                SetGravityScale(Data.gravityScale * Data.jumpHangGravityMult);
            }
            else if (RB.velocity.y < 0)
            {
                //Higher gravity if falling
                SetGravityScale(Data.gravityScale * Data.fallGravityMult);
                //Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
                RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFallSpeed));
            }
            else
            {
                //Default gravity if standing on a platform or moving upwards
                SetGravityScale(Data.gravityScale);
            }
        }
        else
        {
            //No gravity when dashing (returns to normal once initial dashAttack phase over)
            SetGravityScale(0);
        }
        #endregion
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.IsPaused) return;
        FSM.CurrentState.OnFixedUpdate();
        GetCollectibles();

        if (!IsCircleHiting && !IsDodging && !IsDashing && _moveInput.x != 0)
            CheckDirectionToFace(_moveInput.x > 0);

        //Handle Run
        if (!IsDashing && (!IsAttacking || IsCircleHiting) && !IsDodging && CanInputMove)
        {
            if (IsWallJumping)
                Run(Data.wallJumpRunLerp);
            else
                Run(1);
        }
        else if (_isDashAttacking)
        {
            Run(Data.dashEndRunLerp);
        }

        //Handle Slide
        if (IsSliding)
            Slide();
    }

    #region INPUT CALLBACKS
    //Methods which whandle input detected in Update()
    public void OnJumpInput()
    {
        LastPressedJumpTime = Data.jumpInputBufferTime;
    }
    public void OnJumpUpInput()
    {
        if (CanJumpCut() || CanWallJumpCut())
            _isJumpCut = true;
    }
    public void OnDashInput()
    {
        LastPressedDashTime = Data.dashInputBufferTime;
    }
    public void OnDodgeInput()
    {
        LastPressedDodgeTime = Data.dashInputBufferTime;
    }
    private void OnAttackInput()
    {
        LastPressedAttackTime = Data.attackInputBufferTime;
    }
    private void OnHeavyAttackInput()
    {
        LastPressedHeavyAttackTime = Data.attackInputBufferTime;
    }
    private void OnParryInput()
    {
        LastPressedParryTime = Data.attackInputBufferTime;
    }
    private void OnParryCancelInput()
    {
        LastUpParryTime = Data.attackInputBufferTime;
    }

    private void OnThreeCombosInput()
    {
        LastPressedThreeCombosTime = Data.attackInputBufferTime;
    }
    private void OnCircleHitInput()
    {
        LastPressedCircleHitTime = Data.attackInputBufferTime;
    }
    private void OnShockWaveInput()
    {
        LastPressedShockWaveTime = Data.attackInputBufferTime;
    }
    #endregion

    #region GENERAL METHODS
    public void SetGravityScale(float scale)
    {
        RB.gravityScale = scale;
    }

    private void Sleep(float duration)
    {
        //Method used so we don't need to call StartCoroutine everywhere
        //nameof() notation means we don't need to input a string directly.
        //Removes chance of spelling mistakes and will improve error messages if any
        StartCoroutine(nameof(PerformSleep), duration);
    }

    private IEnumerator PerformSleep(float duration)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(duration); //Must be Realtime since timeScale with be 0 
        Time.timeScale = 1;
    }

    public void AnimationTrigger() => FSM.CurrentState.AnimationFinishTrigger();
    #endregion

    //MOVEMENT METHODS
    #region RUN METHODS
    private void Run(float lerpAmount)
    {
        //Calculate the direction we want to move in and our desired velocity
        float targetSpeed = _moveInput.x * Data.runMaxSpeed;
        //We can reduce are control using Lerp() this smooths changes to are direction and speed
        targetSpeed = Mathf.Lerp(RB.velocity.x, targetSpeed, lerpAmount);

        #region Calculate AccelRate
        float accelRate;

        //Gets an acceleration value based on if we are accelerating (includes turning) 
        //or trying to decelerate (stop). As well as applying a multiplier if we're air borne.
        if (LastOnGroundTime > 0)
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : Data.runDeccelAmount;
        else
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount * Data.accelInAir : Data.runDeccelAmount * Data.deccelInAir;
        #endregion

        #region Add Bonus Jump Apex Acceleration
        //Increase are acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
        if ((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
        {
            accelRate *= Data.jumpHangAccelerationMult;
            targetSpeed *= Data.jumpHangMaxSpeedMult;
        }
        #endregion

        #region Conserve Momentum
        //We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
        if (Data.doConserveMomentum && Mathf.Abs(RB.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(RB.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && LastOnGroundTime < 0)
        {
            //Prevent any deceleration from happening, or in other words conserve are current momentum
            //You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
            accelRate = 0;
        }
        #endregion

        //Calculate difference between current velocity and desired velocity
        float speedDif = targetSpeed - RB.velocity.x;
        //Calculate force along x-axis to apply to thr player

        float movement = speedDif * accelRate;

        //Convert this to a vector and apply to rigidbody
        RB.AddForce(movement * Vector2.right, ForceMode2D.Force);

        /*
		 * For those interested here is what AddForce() will do
		 * RB.velocity = new Vector2(RB.velocity.x + (Time.fixedDeltaTime  * speedDif * accelRate) / RB.mass, RB.velocity.y);
		 * Time.fixedDeltaTime is by default in Unity 0.02 seconds equal to 50 FixedUpdate() calls per second
		*/
    }

    private void Turn()
    {
        //stores scale and flips the player along the x axis, 
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        IsFacingRight = !IsFacingRight;
    }
    #endregion

    #region JUMP METHODS
    private void Jump()
    {
        //Ensures we can't call Jump multiple times from one press
        LastPressedJumpTime = 0;
        LastOnGroundTime = 0;

        #region Perform Jump
        //We increase the force applied if we are falling
        //This means we'll always feel like we jump the same amount 
        //(setting the player's Y velocity to 0 beforehand will likely work the same, but I find this more elegant :D)
        float force = Data.jumpForce;
        if (RB.velocity.y < 0)
            force -= RB.velocity.y;

        RB.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        #endregion
    }

    private void WallJump(int dir)
    {
        //Ensures we can't call Wall Jump multiple times from one press
        LastPressedJumpTime = 0;
        LastOnGroundTime = 0;
        LastOnWallRightTime = 0;
        LastOnWallLeftTime = 0;

        #region Perform Wall Jump
        Vector2 force = new Vector2(Data.wallJumpForce.x, Data.wallJumpForce.y);
        force.x *= dir; //apply force in opposite direction of wall

        if (Mathf.Sign(RB.velocity.x) != Mathf.Sign(force.x))
            force.x -= RB.velocity.x;

        if (RB.velocity.y < 0) //checks whether player is falling, if so we subtract the velocity.y (counteracting force of gravity). This ensures the player always reaches our desired jump force or greater
            force.y -= RB.velocity.y;

        //Unlike in the run we want to use the Impulse mode.
        //The default mode will apply are force instantly ignoring masss
        RB.AddForce(force, ForceMode2D.Impulse);
        #endregion
    }
    #endregion

    #region DASH METHODS
    //Dash Coroutine
    private IEnumerator StartDash(Vector2 dir)
    {
        RB.velocity = Vector2.zero;
        //Overall this method of dashing aims to mimic Celeste, if you're looking for
        // a more physics-based approach try a method similar to that used in the jump

        //LastOnGroundTime = 0;
        LastPressedDashTime = 0;

        float startTime = Time.time;

        _dashesLeft--;
        _isDashAttacking = true;

        SetGravityScale(0);

        //We keep the player's velocity at the dash speed during the "attack" phase (in celeste the first 0.15s)
        while (Time.time - startTime <= Data.dashAttackTime)
        {
            RB.velocity = dir.normalized * Data.dashSpeed;
            //Pauses the loop until the next frame, creating something of a Update loop. 
            //This is a cleaner implementation opposed to multiple timers and this coroutine approach is actually what is used in Celeste :D
            yield return null;
        }

        startTime = Time.time;

        _isDashAttacking = false;

        //Begins the "end" of our dash where we return some control to the player but still limit run acceleration (see Update() and Run())
        SetGravityScale(Data.gravityScale);
        RB.velocity = Data.dashEndSpeed * dir.normalized;

        while (Time.time - startTime <= Data.dashEndTime)
        {
            yield return null;
        }

        //Dash over
        IsDashing = false;
        NextDashTime = Data.dashResetTime;
        DashFilled.fillAmount = 1f;
    }
    #endregion

    #region DODGE METHODS
    private IEnumerator Dodge(Vector2 dir)
    {
        RB.velocity = Vector2.zero;
        //Overall this method of dashing aims to mimic Celeste, if you're looking for
        // a more physics-based approach try a method similar to that used in the jump
        LastPressedDodgeTime = 0;

        float startTime = Time.time;
        SetGravityScale(0);
        //We keep the player's velocity at the dash speed during the "attack" phase (in celeste the first 0.15s)
        while (Time.time - startTime <= Data.dodgeStartTime)
        {
            RB.velocity = dir.normalized * Data.dodgeSpeed;
            //Pauses the loop until the next frame, creating something of a Update loop. 
            //This is a cleaner implementation opposed to multiple timers and this coroutine approach is actually what is used in Celeste :D
            yield return null;
        }

        startTime = Time.time;
        SetGravityScale(Data.gravityScale);
        RB.velocity = Data.dodgeEndSpeed * dir.normalized;
        while (Time.time - startTime <= Data.dodgeEndTime)
        {
            yield return null;
        }

        IsDodging = false;
        NextDodgeTime = Data.dodgeResetTime;
    }
    #endregion

    #region SLIDE METHODS
    private void Slide()
    {
        //We remove the remaining upwards Impulse to prevent upwards sliding
        if (RB.velocity.y > 0)
        {
            RB.AddForce(-RB.velocity.y * Vector2.up, ForceMode2D.Impulse);
        }

        //Works the same as the Run but only in the y-axis
        //THis seems to work fine, buit maybe you'll find a better way to implement a slide into this system
        float speedDif = Data.slideSpeed - RB.velocity.y;
        float movement = speedDif * Data.slideAccel;
        //So, we clamp the movement here to prevent any over corrections (these aren't noticeable in the Run)
        //The force applied can't be greater than the (negative) speedDifference * by how many times a second FixedUpdate() is called. For more info research how force are applied to rigidbodies.
        movement = Mathf.Clamp(movement, -Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime), Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime));

        RB.AddForce(movement * Vector2.up);
    }
    #endregion

    #region ATTACK METHODS
    public void SetAttackPoint()
    {
        AttackPoint.transform.position = new Vector3(WeaponPoint.transform.position.x
                                                    , WeaponPoint.transform.position.y, 0);
    }
    public void SetAttacking(bool _isAttacking)
    {
        IsAttacking = _isAttacking;
    }
    public void SetHeavyAttacking(bool _isAttacking)
    {
        IsAttacking = _isAttacking;
        IsHeavyAttacking = _isAttacking;
    }
    public void SetAttackEffect(bool value)
    {
        AttackPoint.SetActive(value);
        WeaponTrails.SetActive(value);
    }

    #endregion

    #region ThreeCombos METHODS
    public void SetThreeCombos(bool _value)
    {
        IsAttacking = _value;
        IsThreeCombosing = _value;
        IsSkilling = _value;
    }
    #endregion

    #region CircleHit METHODS
    public void SetCircleHit(bool _value)
    {
        IsAttacking = _value;
        IsCircleHiting = _value;
        IsSkilling = _value;
    }
    #endregion

    #region ShockWave METHODS
    public void SetShockWave(bool _value)
    {
        IsAttacking = _value;
        IsShockWaving = _value;
        IsSkilling = _value;
    }
    #endregion

    #region HURT METHODS
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyAttack")
        {
            if (IsHurting || IsSuperArmoring || IsStunning || CurrentHP <= 0) { return; }
            EnemyController _enemy = collision.GetComponentInParent<EnemyController>();
            if (!_enemy.CanDamage) { return; }
            StartCoroutine(AudioManager.Instance.PlayHit());
            float damage = _enemy.AttackDamage * Random.Range(0.90f, 1.2f);
            if (IsDefending)
            {
                damage *= 0.5f;
            }
            Hurt(damage, _enemy.IsHeavyAttack);
            Repel(_enemy.transform, _enemy.IsHeavyAttack);
        }
        else if (collision.tag == "Bomb")
        {
            if (IsHurting || IsSuperArmoring || IsStunning || CurrentHP <= 0) { return; }
            Bomb _enemy = collision.GetComponentInParent<Bomb>();
            if (!_enemy.AttackPoint.gameObject.activeSelf) { return; }
            float damage = _enemy.Damage * Random.Range(0.90f, 1.2f);
            if (IsDefending)
            {
                damage *= 0.5f;
            }
            Hurt(damage);
            Repel(_enemy.transform, false);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "EnemyAttack")
        {
            if (IsHurting || IsSuperArmoring || IsStunning || CurrentHP <= 0) { return; }
            EnemyController _enemy = collision.GetComponentInParent<EnemyController>();
            if (!_enemy.CanDamage) { return; }
            StartCoroutine(AudioManager.Instance.PlayHit());
            float damage = _enemy.AttackDamage * Random.Range(0.90f, 1.2f);
            if (IsDefending)
            {
                damage *= 0.5f;
            }
            Hurt(damage, _enemy.IsHeavyAttack);
            Repel(_enemy.transform, _enemy.IsHeavyAttack);
        }
        else if (collision.tag == "Bomb")
        {
            if (IsHurting || IsSuperArmoring || IsStunning || CurrentHP <= 0) { return; }
            Bomb _enemy = collision.GetComponentInParent<Bomb>();
            if (!_enemy.AttackPoint.gameObject.activeSelf) { return; }
            float damage = _enemy.Damage * Random.Range(0.90f, 1.2f);
            if (IsDefending)
            {
                damage *= 0.5f;
            }
            Hurt(damage);
            Repel(_enemy.transform, false);
        }
    }

    public virtual void Hurt(float _damage, bool IsStun = false)
    {
        if (IsHurting || IsSuperArmoring || IsStunning || CurrentHP <= 0) { return; }
        if (!IsStun)
        {
            CameraManager.Instance.Shake(0.5f, 0.1f);
        }
        LastHurtTime = Data.HurtResetTime;
        SetCurrentHP(_damage);
    }

    public void Repel(Transform source, bool IsStun = false)
    {
        Vector2 vector = new Vector2(5, 1);
        //Vector2 vector = Vector2.right * 5;
        if (transform.position.x < source.position.x)
        {
            vector.x *= -1;
        }
        if (IsStun)
        {
            vector *= 5;
        }
        RB.velocity = Vector2.zero;
        RB.AddForce(vector, ForceMode2D.Impulse);
        if (IsStun)
        {
            CheckDirectionToFace(vector.x < 0);
            StartCoroutine(Stun());
            TimerManager.Instance.DoFrozenTime(0.1f);
            CameraManager.Instance.Shake(3f, 0.2f);
        }
    }

    public void SetCurrentHP(float damage)
    {
        CurrentHP = (int)Mathf.Clamp(CurrentHP - damage, 0, MaxHP);
        uiPlayerStatus.DoLerpHealth(damage);
    }
    #endregion

    #region STUN METHODS
    public virtual IEnumerator Stun()
    {
        IsStunning = true;
        yield return new WaitForSeconds(Data.StunResetTime);
        IsStunning = false;
        StartCoroutine(SuperArmor());
    }
    #endregion

    #region SuperArmor METHODS
    public IEnumerator SuperArmor()
    {
        IsSuperArmoring = true;
        yield return new WaitForSeconds(Data.SuperArmorTime);
        IsSuperArmoring = false;
    }
    public void SetSuperArmor(bool isSuperArmor)
    {
        IsSuperArmoring = isSuperArmor;
    }
    #endregion

    #region COLLECTIBLES METHODS
    public void GetCollectibles()
    {
        List<Collider2D> collidersToDamage = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;
        Physics2D.OverlapCollider(CollectCollider, filter, collidersToDamage);
        foreach (var collider in collidersToDamage)
        {
            Collectibles collectibles = collider.GetComponent<Collectibles>();
            if (collectibles != null)
            {
                collectibles.SetCollecting();
                float distance = Vector2.Distance(collider.transform.position, transform.position);
                if (collectibles.IsActive && distance < 1)
                {
                    if (collectibles.Data.Type == CollectiblesData.emType.RedSoul)
                    {
                        GetRedSouls(collectibles.Value);
                    }
                    else if (collectibles.Data.Type == CollectiblesData.emType.GreedSoul)
                    {
                        GetGreenSouls(collectibles.Value);
                    }
                    Destroy(collectibles.gameObject);
                }
            }
        }
    }
    public void GetRedSouls(int value)
    {
        RedSouls += value;
    }
    public void GetGreenSouls(int value)
    {
        SetCurrentHP(value * -1);
    }
    #endregion

    #region PARRY METHODS
    public void SetParrying(bool value)
    {
        IsParrying = value;
        if (value)
        {
            CanCounterTime = 0.3f;
        }
        else
        {
            CanCounterTime = 0;
        }
    }
    public bool ParryHits()
    {
        bool canCounter = false;
        Collider2D HitCollider = ParryPoint.GetComponent<Collider2D>();
        List<Collider2D> collidersToDamage = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;
        Physics2D.OverlapCollider(HitCollider, filter, collidersToDamage);
        foreach (var collider in collidersToDamage)
        {
            if (collider.tag == "EnemyAttack")
            {
                if (!IsDefending)
                {
                    EnemyController enemy = collider.GetComponentInParent<EnemyController>();
                    if (enemy != null && enemy.CanBeStunned)
                    {
                        enemy.Stunned();
                        canCounter = true;
                        EffectManager.Instance.DoHitFX(AttackPoint);
                        TimerManager.Instance.SlowFrozenTime(0.3f);
                        CameraManager.Instance.Shake(3f, 0.1f);
                    }
                }
            }
        }
        return canCounter;
    }
    #endregion

    #region CHECK METHODS
    public void CheckDirectionToFace(bool isMovingRight)
    {
        if (isMovingRight != IsFacingRight)
            Turn();
    }

    private bool CanJump()
    {
        return LastOnGroundTime > 0 && !IsJumping && (!IsAttacking || IsCircleHiting) && !_isJumpFalling;
    }

    private bool CanWallJump()
    {
        return LastPressedJumpTime > 0 && LastOnWallTime > 0 && LastOnGroundTime <= 0 && (!IsWallJumping ||
             (LastOnWallRightTime > 0 && _lastWallJumpDir == 1) || (LastOnWallLeftTime > 0 && _lastWallJumpDir == -1));
    }

    private bool CanJumpCut()
    {
        return IsJumping && RB.velocity.y > 0;
    }

    private bool CanWallJumpCut()
    {
        return IsWallJumping && RB.velocity.y > 0;
    }

    private bool CanDash()
    {
        DashFilled.fillAmount -= 1.0f / DashCD * Time.deltaTime;
        return !IsDashing && DashFilled.fillAmount == 0;
    }

    public bool CanSlide()
    {
        if (LastOnWallTime > 0 && !IsJumping && !IsWallJumping && !IsDashing && !IsOnGround)
            return true;
        else
            return false;
    }

    private bool CanAttack()
    {
        return !IsAttacking && !IsDashing && !IsDodging;
    }

    private bool CanDodge()
    {
        return IsOnGround && !IsDashing && !IsDodging && NextDodgeTime < 0f;
    }
    private bool CanParry()
    {
        return !IsParrying && !IsDashing && !IsDodging;
    }
    private bool CanThreeCombos()
    {
        ThreeCombosFilled.fillAmount -= 1.0f / ThreeCombosCD * Time.deltaTime;
        return CanAttack() && ThreeCombosFilled.fillAmount == 0;
    }
    private bool CanCircleHit()
    {
        CircleHitFilled.fillAmount -= 1.0f / CircleHitCD * Time.deltaTime;
        return CanAttack() && CircleHitFilled.fillAmount == 0;
    }
    private bool CanShockWave()
    {
        ShockWaveFilled.fillAmount -= 1.0f / ShockWaveCD * Time.deltaTime;
        return CanAttack() && ShockWaveFilled.fillAmount == 0;
    }
    #endregion

    #region FX METHODS
    public void DoLandFX()
    {
        EffectManager.Instance.DoLandFX(_groundCheckPoint.gameObject);
    }
    public void DoSlideFX()
    {
        EffectManager.Instance.DoSlideFX(_groundCheckPoint.gameObject);
    }
    public IEnumerator HurtFlashFX()
    {
        while (IsSuperArmoring)
        {
            foreach (SpriteRenderer spr in spritList)
            {
                spr.material = flashFXMAT;
            }
            yield return new WaitForSeconds(.1f);
            foreach (SpriteRenderer spr in spritList)
            {
                spr.material = originalMAT;
            }
            yield return new WaitForSeconds(.1f);
        }
    }

    public void PlayThreeCombosFX(int angle = 0)
    {
        EffectManager.Instance.DoThreeCombosFX(AttackPoint, IsFacingRight, angle);
    }
    public void PlayCircleHitFX()
    {
        EffectManager.Instance.DoCircleHitFX(gameObject, transform);
    }
    public void PlayShockWaveFX()
    {
        EffectManager.Instance.DoShockWaveFX(gameObject, transform);
    }
    #endregion

    #region EDITOR METHODS
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(_frontWallCheckPoint.position, _wallCheckSize);
        Gizmos.DrawWireCube(_backWallCheckPoint.position, _wallCheckSize);
    }
    #endregion
}

