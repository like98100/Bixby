using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContorl : PlayerStatusControl
{
    private GameObject camera;
    private Vector3 cameraForward;
    private Vector3 cameraRight;

    private Vector3 playerForward;
    private Vector3 playerRight;

    private CharacterController player;

    private Vector3 playerDirection = Vector3.forward;
    private bool isJumpPressed = false;
    private bool isSwimming = false;

    public Transform ProjectileStart;

    private LineRenderer projectileLine;
    private WaitForSeconds shotDuration = new WaitForSeconds(0.25f); //????? ???? ???? ????
    private Vector3 rayOrigin;
    private RaycastHit hitInfo;
    private RaycastHit[] hitInfo_all;
    private bool isAimAttack;

    private bool isOnAFK = false;
    private float latestActionTime = 0f;
    private float myAFKTime = 5.0f; //5?? ?? Idle?? ?????.

    public STATE State = STATE.NONE; // ???? ????.
    public STATE NextState = STATE.NONE; // ???? ????.
    public STATE PrevState = STATE.NONE; // ???? ????.
    public float StateTimer = 0.0f; // ????
    public enum STATE
    {
        NONE = -1, // ???? ????. ???? ????.
        IDLE = 0, // ???
        MOVE = 1, // ???
        AIM = 2, // ????
        DASH = 3, // ?????
        RUN = 4, // ????
        ATTACK = 5, // ?????
        CHARGE_ATTACK = 6, // ??????
        ELEMENT_SKILL = 7, // ??????
        ELEMENT_ULT_SKILL = 8, // ???????
        SWIMMING = 9, // ???? ??
        STUNNED = 10, // ????
        DEAD = 11, // ???

        NUM = 12, // ???? ????
    };

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        this.isJumpPressed = false;
        this.isAimAttack = false;
        this.State = STATE.NONE;
        this.NextState = STATE.IDLE;

        camera = GameObject.FindGameObjectWithTag("MainCamera");
        player = this.GetComponent<CharacterController>();
        projectileLine = this.GetComponent<LineRenderer>();
        projectileLine.enabled = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        vectorAlign();
        this.StateTimer += Time.deltaTime;
        if (UI_Control.Inst.OpenedWindow == null)
        {
            if (this.NextState == STATE.NONE)
            {
                if (isSwimming == true)
                {
                    this.PrevState = this.State;
                    this.NextState = STATE.SWIMMING;
                }
                switch (this.State)
                {
                    case STATE.IDLE:
                        if ((Input.GetAxis("Vertical") != 0) || (Input.GetAxis("Horizontal") != 0))
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.MOVE;
                        }
                        if (Input.GetMouseButton(1))
                            this.NextState = STATE.AIM;
                        if (Input.GetMouseButton(0) && Time.time > nextFire)
                        {
                            this.isAimAttack = false;
                            this.PrevState = this.State;
                            this.NextState = STATE.ATTACK;
                        }
                        if (Input.GetKeyDown(KeyCode.LeftShift) && Stamina > DashStaminaAmount && player.isGrounded)
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.DASH;
                        }
                        if (Input.GetKeyDown(KeyCode.E) && player.isGrounded)
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.ELEMENT_SKILL;
                        }
                        if (Input.GetKeyDown(KeyCode.Q) && player.isGrounded)
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.ELEMENT_ULT_SKILL;
                        }
                        break;
                    case STATE.MOVE:
                        if (Input.GetMouseButton(1))
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.AIM;
                        }
                        if (Input.GetMouseButton(0) && Time.time > nextFire)
                        {
                            this.isAimAttack = false;
                            this.PrevState = this.State;
                            this.NextState = STATE.ATTACK;
                        }
                        if (Input.GetKeyDown(KeyCode.LeftShift) && Stamina > DashStaminaAmount && player.isGrounded)
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.DASH;
                        }
                        if (Input.GetKeyDown(KeyCode.E) && player.isGrounded)
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.ELEMENT_SKILL;
                        }
                        if (Input.GetKeyDown(KeyCode.Q) && player.isGrounded)
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.ELEMENT_ULT_SKILL;
                        }
                        if (isAFK())
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.IDLE;
                        }
                        break;
                    case STATE.AIM:
                        if (!Input.GetMouseButton(1))
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.MOVE;
                        }
                        if (Input.GetMouseButton(0) && Time.time > nextFire)
                        {
                            this.isAimAttack = true;
                            this.PrevState = this.State;
                            this.NextState = STATE.ATTACK;
                        }
                        if (Input.GetKeyDown(KeyCode.LeftShift) && Stamina > DashStaminaAmount && player.isGrounded)
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.DASH;
                        }
                        if (Input.GetKeyDown(KeyCode.E) && player.isGrounded)
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.ELEMENT_SKILL;
                        }
                        if (Input.GetKeyDown(KeyCode.Q) && player.isGrounded)
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.ELEMENT_ULT_SKILL;
                        }
                        break;
                    case STATE.DASH:
                        if (Input.GetKey(KeyCode.LeftShift))
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.RUN;
                        }
                        else
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.MOVE;
                        }
                        break;
                    case STATE.RUN:
                        if (!Input.GetKey(KeyCode.LeftShift) || Stamina <= 0)
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.MOVE;
                        }
                        if (Input.GetKeyDown(KeyCode.E) && player.isGrounded)
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.ELEMENT_SKILL;
                        }
                        if (Input.GetKeyDown(KeyCode.Q) && player.isGrounded)
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.ELEMENT_ULT_SKILL;
                        }
                        break;
                    case STATE.ATTACK:
                        if (this.StateTimer >= SwitchToChargeTime)
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.CHARGE_ATTACK;
                        }
                        if (!Input.GetMouseButton(0))
                        {
                            nextFire = Time.time + FireRate;
                            attack();
                            if (this.PrevState == STATE.AIM)
                            {
                                this.PrevState = this.State;
                                this.NextState = STATE.AIM;
                            }
                            else
                            {
                                this.PrevState = this.State;
                                this.NextState = STATE.MOVE;
                            }
                        }
                        if (Input.GetKeyDown(KeyCode.LeftShift) && Stamina > DashStaminaAmount && player.isGrounded)
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.DASH;
                        }
                        break;
                    case STATE.CHARGE_ATTACK:
                        if (!Input.GetMouseButton(0))
                        {
                            if (this.Stamina >= ChargeAttackStaminaAmount)
                            {
                                nextFire = Time.time + FireRate;
                                StaminaUse(ChargeAttackStaminaAmount);
                                chargedAttack();
                            }
                            else
                            {
                                this.PrevState = this.State;
                                this.NextState = STATE.MOVE;
                            }
                            this.PrevState = this.State;
                            this.NextState = STATE.AIM;
                        }
                        if (Input.GetKeyDown(KeyCode.LeftShift) && Stamina > DashStaminaAmount && player.isGrounded)
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.DASH;
                        }
                        break;
                    case STATE.ELEMENT_SKILL:

                        break;
                    case STATE.ELEMENT_ULT_SKILL:
                        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Q))
                        {
                            nextFire = Time.time + FireRate;
                            elementUltSkill();
                            StartCoroutine(fallBack());
                            this.PrevState = this.State;
                            this.NextState = STATE.MOVE;
                        }
                        if (this.StateTimer >= 3.0f)
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.MOVE;
                        }
                        break;
                    case STATE.SWIMMING:
                        //Water Tag?? ???? ??????? ????????? Move?? ????.
                        //?????????? ??????? ??? ????? ??.
                        if (isSwimming == false)
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.MOVE;
                        }
                        if (Stamina <= 0)
                        {
                            //die();
                        }
                        break;
                    case STATE.STUNNED:
                        //???? ????? ??????, ??? Move?? ???? ????????.
                        this.PrevState = this.State;
                        this.NextState = STATE.MOVE;
                        break;
                    case STATE.DEAD:
                        //?????? ??????.
                        break;
                }
            }

            // ?????? ??????? ??------------.
            while (this.NextState != STATE.NONE)
            { // ?????? NONE??? = ?????? ??????.
                var offset = camera.transform.forward;

                this.State = this.NextState;
                this.NextState = STATE.NONE;
                switch (this.State)
                {
                    case STATE.IDLE:

                        break;
                    case STATE.MOVE:
                        MyCurrentSpeed = Speed;
                        camera.GetComponent<CamControl>().isOnAim = false;
                        break;
                    case STATE.AIM:
                        offset.y = 0;
                        camera.GetComponent<CamControl>().isOnAim = true;
                        break;
                    case STATE.DASH:
                        camera.GetComponent<CamControl>().isOnAim = false;
                        StaminaUse(DashStaminaAmount);
                        isDashed = false;
                        break;
                    case STATE.RUN:
                        MyCurrentSpeed = RunSpeed;
                        camera.GetComponent<CamControl>().isOnAim = false;
                        break;
                    case STATE.ATTACK:
                        MyCurrentSpeed = Speed;
                        switch (this.isAimAttack)
                        {
                            case true:
                                camera.GetComponent<CamControl>().isOnAim = true;
                                break;
                            case false:
                                break;
                        }
                        break;
                    case STATE.CHARGE_ATTACK:
                        MyCurrentSpeed = Speed;
                        camera.GetComponent<CamControl>().isOnAim = true;
                        break;
                    case STATE.ELEMENT_SKILL:
                        offset.y = 0;
                        transform.LookAt(player.transform.position + offset);
                        break;
                    case STATE.ELEMENT_ULT_SKILL:
                        offset.y = 0;
                        transform.LookAt(player.transform.position + offset);
                        camera.GetComponent<CamControl>().isOnAim = true;
                        break;
                    case STATE.SWIMMING:
                        MyCurrentSpeed = SwimSpeed;
                        camera.GetComponent<CamControl>().isOnAim = false;
                        break;
                    case STATE.STUNNED:

                        break;
                    case STATE.DEAD:

                        break;
                }
                this.StateTimer = 0.0f;
            }
            // ?? ??????? ????? ??----------.
            switch (this.State)
            {
                case STATE.IDLE:
                    move();
                    StaminaRegerenate();
                    break;
                case STATE.MOVE:
                    move();
                    StaminaRegerenate();
                    break;
                case STATE.AIM:
                    aimModeMove();
                    StaminaRegerenate();
                    break;
                case STATE.DASH:
                    dash();
                    break;
                case STATE.RUN:
                    move();
                    StaminaTickUse(RunStaminaAmount);
                    break;
                case STATE.ATTACK:
                    switch (this.isAimAttack)
                    {
                        case true:
                            aimModeMove();
                            break;
                        case false:
                            if (this.StateTimer < 0.3f)
                            {
                                move();
                            }
                            else
                            {
                                camera.GetComponent<CamControl>().isOnAim = true;
                                aimModeMove();
                            }
                            break;
                    }
                    break;
                case STATE.CHARGE_ATTACK:
                    aimModeMove();
                    break;
                case STATE.ELEMENT_SKILL:
                    elementSkill();
                    break;
                case STATE.ELEMENT_ULT_SKILL:
                    ultLockOn();
                    break;
                case STATE.SWIMMING:
                    swimming();
                    if (Stamina >= 0)
                    {
                        StaminaTickUse(SwimStaminaAmount);
                    }
                    break;
                case STATE.STUNNED:

                    break;
                case STATE.DEAD:
                    playerDirection.y -= GravityForce * Time.deltaTime;
                    break;
            }
        }

        if (isHitted)
        {

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Water")
        {
            Debug.Log("Swimming!");
            isSwimming = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Water")
        {
            Debug.Log("Not Swimming!");
            isSwimming = false;
        }
    }

    protected override void die()
    {
        base.die();
        this.PrevState = this.State;
        this.NextState = STATE.DEAD;
    }

    private bool isAFK()
    {
        latestActionTime += Time.deltaTime;
        if (Input.anyKey)
        {
            isOnAFK = false;
            latestActionTime = 0;
        }
        else
        {
            isOnAFK = true;
        }

        if ((latestActionTime >= myAFKTime) && isOnAFK)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void vectorAlign()
    {
        cameraForward = camera.transform.forward;
        cameraRight = camera.transform.right;

        playerForward = cameraForward;
        playerForward.y = 0;
        playerRight = cameraRight;
        playerRight.y = 0;
    }

    private void move() //????????? ????? ?? ?????? ?????? ???.
    {
        Vector3 snapGround = Vector3.zero;

        if ((Input.GetKey(KeyCode.A)) && (Input.GetKey(KeyCode.S)))
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation,
               Quaternion.LookRotation(-(playerRight + playerForward).normalized),
               Time.deltaTime * rotationSpeed);
        }

        else if ((Input.GetKey(KeyCode.D)) && (Input.GetKey(KeyCode.W)))
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation,
               Quaternion.LookRotation((playerRight + playerForward).normalized),
               Time.deltaTime * rotationSpeed);
        }

        else if ((Input.GetKey(KeyCode.A)) && (Input.GetKey(KeyCode.W)))
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation,
               Quaternion.LookRotation(-(playerRight - playerForward).normalized),
               Time.deltaTime * rotationSpeed);
        }

        else if ((Input.GetKey(KeyCode.D)) && (Input.GetKey(KeyCode.S)))
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation,
               Quaternion.LookRotation(-(-playerRight + playerForward).normalized),
               Time.deltaTime * rotationSpeed);
        }

        else if (Input.GetKey(KeyCode.W))
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation,
                Quaternion.LookRotation(playerForward),
                Time.deltaTime * rotationSpeed);
        }

        else if (Input.GetKey(KeyCode.S))
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation,
                Quaternion.LookRotation(-playerForward),
                Time.deltaTime * rotationSpeed);
        }

        else if (Input.GetKey(KeyCode.D))
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation,
                Quaternion.LookRotation(playerRight),
                Time.deltaTime * rotationSpeed);
        }

        else if (Input.GetKey(KeyCode.A))
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation,
               Quaternion.LookRotation(-playerRight),
               Time.deltaTime * rotationSpeed);
        }

        if (player.isGrounded)
        {
            if ((Input.GetAxis("Vertical") != 0) || (Input.GetAxis("Horizontal") != 0))
                playerDirection = new Vector3(0, 0, 1);
            else playerDirection = new Vector3(0, 0, 0);

            playerDirection = player.transform.TransformDirection(playerDirection);
            playerDirection *= MyCurrentSpeed;

            if (isJumpPressed == false && Input.GetButton("Jump"))
            {
                isJumpPressed = true;
                playerDirection.y = JumpPower;
            }
        }
        else
        {
            playerDirection.y -= GravityForce * Time.deltaTime;
        }

        if (!Input.GetButton("Jump"))
        {
            if (player.isGrounded) snapGround = Vector3.down;
            isJumpPressed = false;
        }

        player.Move(playerDirection * Time.deltaTime + snapGround);
    }
    private void swimming() //???? ??.
    {
        Vector3 snapGround = Vector3.down;

        if ((Input.GetKey(KeyCode.A)) && (Input.GetKey(KeyCode.S)))
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation,
               Quaternion.LookRotation(-(playerRight + playerForward).normalized),
               Time.deltaTime * rotationSpeed);
        }

        else if ((Input.GetKey(KeyCode.D)) && (Input.GetKey(KeyCode.W)))
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation,
               Quaternion.LookRotation((playerRight + playerForward).normalized),
               Time.deltaTime * rotationSpeed);
        }

        else if ((Input.GetKey(KeyCode.A)) && (Input.GetKey(KeyCode.W)))
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation,
               Quaternion.LookRotation(-(playerRight - playerForward).normalized),
               Time.deltaTime * rotationSpeed);
        }

        else if ((Input.GetKey(KeyCode.D)) && (Input.GetKey(KeyCode.S)))
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation,
               Quaternion.LookRotation(-(-playerRight + playerForward).normalized),
               Time.deltaTime * rotationSpeed);
        }

        else if (Input.GetKey(KeyCode.W))
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation,
                Quaternion.LookRotation(playerForward),
                Time.deltaTime * rotationSpeed);
        }

        else if (Input.GetKey(KeyCode.S))
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation,
                Quaternion.LookRotation(-playerForward),
                Time.deltaTime * rotationSpeed);
        }

        else if (Input.GetKey(KeyCode.D))
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation,
                Quaternion.LookRotation(playerRight),
                Time.deltaTime * rotationSpeed);
        }

        else if (Input.GetKey(KeyCode.A))
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation,
               Quaternion.LookRotation(-playerRight),
               Time.deltaTime * rotationSpeed);
        }

        if (player.isGrounded)
        {
            if ((Input.GetAxis("Vertical") != 0) || (Input.GetAxis("Horizontal") != 0))
                playerDirection = new Vector3(0, 0, 1);
            else playerDirection = new Vector3(0, 0, 0);

            playerDirection = player.transform.TransformDirection(playerDirection);
            playerDirection *= MyCurrentSpeed;
        }
        else
        {
            playerDirection.y -= GravityForce * Time.deltaTime;
        }

        player.Move(playerDirection * Time.deltaTime + snapGround);
    }

    private void aimModeMove() //?????? ?????? ???? ???. 50%?? ????? ?????.
    {
        Vector3 snapGround = Vector3.zero;

        var offset = camera.transform.forward;
        offset.y = 0;
        transform.LookAt(player.transform.position + offset);

        if (player.isGrounded)
        {
            playerDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            playerDirection = player.transform.TransformDirection(playerDirection);
            playerDirection *= (float)(Speed / 2);
        }
        else
            playerDirection.y -= GravityForce * Time.deltaTime;

        if (player.isGrounded) snapGround = Vector3.down;

        player.Move(playerDirection * Time.deltaTime + snapGround);
    }

    private void dash() //????
    {
        StartCoroutine(dashCoroutine());
        isDashed = true;
    }
    private IEnumerator dashCoroutine()
    {
        Vector3 snapGround = Vector3.zero;
        if (player.isGrounded) snapGround = Vector3.down;
        float startTime = Time.time; //????? ???? ????
        while (Time.time < startTime + 0.3f) //????? ????? ????, 0.1??
        {
            player.Move(this.transform.forward * DashSpeed * Time.deltaTime + snapGround);
            yield return null;
        }
    }

    public bool GetIsGrounded()
    {
        return player.isGrounded;
    }

    private void attack()
    {
        projectileLine.startColor = Color.yellow; //???????? ??????? ????, ???????? ??????.
        projectileLine.endColor = Color.white; //???????? ??????? ????, ???????? ??????.
        projectileLine.startWidth = 1.0f;
        projectileLine.endWidth = 1.0f;

        switch (this.isAimAttack)
        {
            case true:
                rayOrigin = camera.GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));

                projectileLine.SetPosition(0, ProjectileStart.position);
                if (Physics.Raycast(rayOrigin, camera.transform.forward, out hitInfo, ShootDistance))
                {
                    projectileLine.SetPosition(1, hitInfo.point);
                    this.gameObject.GetComponent<PlayerLineSkill>().ShowAttackEffect((int)this.State, (int)this.MyElement, ProjectileStart);
                    this.gameObject.GetComponent<PlayerLineSkill>().ShowHitEffect(hitInfo.point);
                    StartCoroutine(shootEffect());
                    // 건드린 부분
                    if (hitInfo.collider.tag == "Enemy")
                    {
                        setEnemyElement(hitInfo.collider.GetComponent<Enemy>().Stat.enemyElement);
                        hitInfo.collider.GetComponent<Enemy>().TakeDamage(attackedOnNormal(10.0f));
                    }
                }
                else
                {
                    projectileLine.SetPosition(1, rayOrigin + (camera.transform.forward * ShootDistance));
                    this.gameObject.GetComponent<PlayerLineSkill>().ShowAttackEffect((int)this.State, (int)this.MyElement, ProjectileStart);
                    StartCoroutine(shootEffect());
                }
                break;
            case false:
                rayOrigin = ProjectileStart.position;
                projectileLine.SetPosition(0, ProjectileStart.position);
                if (Physics.Raycast(rayOrigin, player.transform.forward, out hitInfo, ShootDistance))
                {
                    projectileLine.SetPosition(1, hitInfo.point);
                    this.gameObject.GetComponent<PlayerLineSkill>().ShowAttackEffect((int)this.State, (int)this.MyElement, ProjectileStart);
                    this.gameObject.GetComponent<PlayerLineSkill>().ShowHitEffect(hitInfo.point);
                    StartCoroutine(shootEffect());
                    //hitInfo.collider.GetComponent<StatusControl>();  ???? ????? ?????? ??????. ???? ????? ????.
                }
                else
                {
                    projectileLine.SetPosition(1, ProjectileStart.position + (player.transform.forward * ShootDistance));
                    this.gameObject.GetComponent<PlayerLineSkill>().ShowAttackEffect((int)this.State, (int)this.MyElement, ProjectileStart);
                    StartCoroutine(shootEffect());
                }
                break;
        }
    }

    private void chargedAttack()
    {
        projectileLine.startColor = mySkillStartColor; //???? ????? ???? ??? ?????? ????.
        projectileLine.endColor = mySkillEndColor; //???? ????? ???? ??? ?????? ????.
        projectileLine.startWidth = 2.5f;
        projectileLine.endWidth = 2.5f;

        rayOrigin = camera.GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
        projectileLine.SetPosition(0, ProjectileStart.position);
        if (Physics.Raycast(rayOrigin, camera.transform.forward, out hitInfo, ShootDistance))
        {
            projectileLine.SetPosition(1, hitInfo.point);
            this.gameObject.GetComponent<PlayerLineSkill>().ShowAttackEffect((int)this.State, (int)this.MyElement, ProjectileStart);
            this.gameObject.GetComponent<PlayerLineSkill>().ShowHitEffect(hitInfo.point);
            StartCoroutine(shootEffect());
            //hitInfo.collider.GetComponent<StatusControl>();  ???? ????? ?????? ??????. ???? ????? ????.
        }
        else
        {
            projectileLine.SetPosition(1, rayOrigin + (camera.transform.forward * ShootDistance));
            this.gameObject.GetComponent<PlayerLineSkill>().ShowAttackEffect((int)this.State, (int)this.MyElement, ProjectileStart);
            StartCoroutine(shootEffect());
        }
    }

    private void elementSkill()
    {
        //instanciate (???)
        StartCoroutine(fallBack()); //????
        this.NextState = STATE.MOVE;
    }

    private void ultLockOn()
    {
        Vector3 snapGround = Vector3.zero;
        var offset = camera.transform.forward;
        offset.y = 0;
        transform.LookAt(player.transform.position + offset);
        player.Move(snapGround);
    }

    private void elementUltSkill()
    {
        projectileLine.startColor = mySkillStartColor; //???? ????? ???? ??? ?????? ????.
        projectileLine.endColor = mySkillEndColor; //???? ????? ???? ??? ?????? ????.
        projectileLine.startWidth = 4.0f;
        projectileLine.endWidth = 4.0f;

        rayOrigin = camera.GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
        projectileLine.SetPosition(0, ProjectileStart.position);

        hitInfo_all = Physics.RaycastAll(rayOrigin, camera.transform.forward, ShootDistance);
        for (int i = 0; i < hitInfo_all.Length; i++)
        {
            hitInfo = hitInfo_all[i];
            //hitInfo.collider.GetComponent<StatusControl>();  ???? ????? ?????? ??????. ???? ????? ????.
        }
        projectileLine.SetPosition(1, rayOrigin + (camera.transform.forward * ShootDistance));
        StartCoroutine(shootEffect());
    }

    private IEnumerator fallBack()
    {
        Vector3 snapGround = Vector3.zero;
        if (player.isGrounded) snapGround = Vector3.down;
        float startTime = Time.time; //????? ???? ????
        while (Time.time < startTime + 0.25f) //????? ????? ????, 0.1??
        {
            player.Move(this.transform.forward * -15.0f * Time.deltaTime + snapGround);
            yield return null;
        }
    }

    private IEnumerator shootEffect()
    {
        projectileLine.enabled = true;
        yield return shotDuration;
        projectileLine.enabled = false;
    }



}