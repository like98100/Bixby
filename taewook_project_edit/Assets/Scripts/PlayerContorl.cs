using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContorl : PlayerStatusControl
{
    public GameObject Camera;
    private Vector3 cameraForward;
    private Vector3 cameraRight;

    private Vector3 playerForward;
    private Vector3 playerRight;

    private Vector3 dashDesination;

    private CharacterController player;

    private Vector3 playerDirection = Vector3.forward;
    private bool isJumpPressed = false;


    public Transform ProjectileStart;

    private LineRenderer projectileLine;
    private WaitForSeconds shotDuration = new WaitForSeconds(0.1f); //����ü ���� ���� �ð�
    private Vector3 rayOrigin;
    private RaycastHit hitInfo;
    private bool isAimAttack;

    public STATE State = STATE.NONE; // ���� ����.
    public STATE NextState = STATE.NONE; // ���� ����.
    public float StateTimer = 0.0f; // Ÿ�̸�
    public enum STATE
    {
        NONE = -1, // ���� ����. �ʱ�ȭ ����.
        IDLE = 0, // ���
        MOVE = 1, // �̵�
        AIM = 2, // ����
        DASH = 3, // ���ֱ�
        RUN = 4, // ����
        ATTACK = 5, // �����
        CHARGE_ATTACK = 6, // ������
        ELEMENT_SKILL = 7, // ���ҽ�ų
        ELEMENT_ULT_SKILL = 8, //���ұñر�

        NUM = 9, // ���� ����
    };

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        this.isAimAttack = false;
        this.State = STATE.NONE;
        this.NextState = STATE.IDLE;

        player = this.GetComponent<CharacterController>();
        projectileLine = this.GetComponent<LineRenderer>();
        projectileLine.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        vectorAlign();
        this.StateTimer += Time.deltaTime;
        if (this.NextState == STATE.NONE)
        {
            switch (this.State)
            {
                case STATE.IDLE:
                    this.NextState = STATE.MOVE; //����� ���߿� �Ų����� ��� ������Ʈ�� ������ �Ǿ�� �Ѵ�.
                    break;
                case STATE.MOVE:
                    if (Input.GetMouseButton(1))
                        this.NextState = STATE.AIM;
                    if (Input.GetMouseButton(0) && Time.time > nextFire)
                    {
                        this.isAimAttack = false;
                        this.NextState = STATE.ATTACK;
                    }
                    if (Input.GetKeyDown(KeyCode.LeftShift) && Stamina > DashStaminaAmount && player.isGrounded)
                        this.NextState = STATE.DASH;
                    break;
                case STATE.AIM:
                    if (!Input.GetMouseButton(1))
                        this.NextState = STATE.MOVE;
                    if (Input.GetMouseButton(0) && Time.time > nextFire)
                    {
                        this.isAimAttack = true;
                        this.NextState = STATE.ATTACK;
                    }
                    if (Input.GetKeyDown(KeyCode.LeftShift) && Stamina > DashStaminaAmount && player.isGrounded)
                        this.NextState = STATE.DASH;
                    break;
                case STATE.DASH:
                    if (Input.GetKey(KeyCode.LeftShift))
                        this.NextState = STATE.RUN;
                    else
                        this.NextState = STATE.MOVE;
                    break;
                case STATE.RUN:
                    if (!Input.GetKey(KeyCode.LeftShift) || Stamina <= 0)
                        this.NextState = STATE.MOVE;
                    break;
                case STATE.ATTACK:
                    if (this.StateTimer >= SwitchToChargeTime) this.NextState = STATE.CHARGE_ATTACK;
                    if (!Input.GetMouseButton(0))
                    {
                        Debug.Log("Attack shoot!");
                        nextFire = Time.time + FireRate;
                        attack();
                        this.NextState = STATE.MOVE;
                    }
                    if (Input.GetKeyDown(KeyCode.LeftShift) && Stamina > DashStaminaAmount && player.isGrounded)
                        this.NextState = STATE.DASH;
                    break;
                case STATE.CHARGE_ATTACK:
                    if (!Input.GetMouseButton(0))
                    {
                        Debug.Log("Charge attack shoot!");
                        nextFire = Time.time + FireRate;
                        chargedAttack();
                        this.NextState = STATE.MOVE;
                    }
                    if (Input.GetKeyDown(KeyCode.LeftShift) && Stamina > DashStaminaAmount && player.isGrounded)
                        this.NextState = STATE.DASH;
                    break;
                case STATE.ELEMENT_SKILL:
                    
                    break;
                case STATE.ELEMENT_ULT_SKILL:
                    
                    break;
            }
        }

        // ���°� ��ȭ���� ��------------.
        while (this.NextState != STATE.NONE)
        { // ���°� NONE�̿� = ���°� ��ȭ�ߴ�.
            var offset = Camera.transform.forward;

            this.State = this.NextState;
            this.NextState = STATE.NONE;
            switch (this.State)
            {
                case STATE.IDLE:

                    break;
                case STATE.MOVE:
                    MyCurrentSpeed = Speed;
                    Camera.GetComponent<CamControl>().isOnAim = false;
                    break;
                case STATE.AIM:
                    offset.y = 0;
                    transform.LookAt(player.transform.position + offset);
                    Camera.GetComponent<CamControl>().isOnAim = true;
                    break;
                case STATE.DASH:
                    Camera.GetComponent<CamControl>().isOnAim = false;
                    StaminaUse(DashStaminaAmount);
                    dashDesination = playerDirection;
                    dashDesination.y = 0.0f;
                    isDashed = false;
                    break;
                case STATE.RUN:
                    MyCurrentSpeed = RunSpeed;
                    Camera.GetComponent<CamControl>().isOnAim = false;
                    break;
                case STATE.ATTACK:
                    MyCurrentSpeed = Speed;
                    switch (this.isAimAttack)
                    {
                        case true:
                            offset.y = 0;
                            transform.LookAt(player.transform.position + offset);
                            Camera.GetComponent<CamControl>().isOnAim = true;
                            Debug.Log("attack on aim!");
                            break;
                        case false:
                            Debug.Log("attack on move!");
                            break;
                    }
                    break;
                case STATE.CHARGE_ATTACK:
                    MyCurrentSpeed = Speed;
                    offset.y = 0;
                    transform.LookAt(player.transform.position + offset);
                    Camera.GetComponent<CamControl>().isOnAim = true;
                    break;
                case STATE.ELEMENT_SKILL:

                    break;
                case STATE.ELEMENT_ULT_SKILL:

                    break;
            }
            this.StateTimer = 0.0f;
        }
        // �� ��Ȳ���� �ݺ��� ��----------.
        switch (this.State)
        {
            case STATE.IDLE:
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
                        move();
                        break;
                }
                break;
            case STATE.CHARGE_ATTACK:
                aimModeMove();
                break;
            case STATE.ELEMENT_SKILL:

                break;
            case STATE.ELEMENT_ULT_SKILL:

                break;
        }

        if (isHitted)
        {

        }
    }

    private void vectorAlign()
    {
        cameraForward = Camera.transform.forward;
        cameraRight = Camera.transform.right;

        playerForward = cameraForward;
        playerForward.y = 0;
        playerRight = cameraRight;
        playerRight.y = 0;
    }

    private void move() //�Ϲ������� �̵��� �� ����ϴ� ������ ���.
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

        else if((Input.GetKey(KeyCode.A)) && (Input.GetKey(KeyCode.W)))
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation,
               Quaternion.LookRotation(-(playerRight - playerForward).normalized),
               Time.deltaTime * rotationSpeed);
        }

        else if((Input.GetKey(KeyCode.D)) && (Input.GetKey(KeyCode.S)))
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation,
               Quaternion.LookRotation(-(-playerRight + playerForward).normalized),
               Time.deltaTime * rotationSpeed);
        }

        else if(Input.GetKey(KeyCode.W))
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation,
                Quaternion.LookRotation(playerForward),
                Time.deltaTime * rotationSpeed);
        }

        else if(Input.GetKey(KeyCode.S))
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation,
                Quaternion.LookRotation(-playerForward),
                Time.deltaTime * rotationSpeed);
        }

        else if(Input.GetKey(KeyCode.D))
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation,
                Quaternion.LookRotation(playerRight),
                Time.deltaTime * rotationSpeed);
        }

        else if(Input.GetKey(KeyCode.A))
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

    private void aimModeMove() //���ظ�� ������ ���� �Ұ�. 50%�� �ӵ��� �̵���.
    {
        Vector3 snapGround = Vector3.zero;

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            var offset = Camera.transform.forward;
            offset.y = 0;
            transform.LookAt(player.transform.position + offset);
        }

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

    private void dash() //����
    {
        Vector3 snapGround = Vector3.zero;
        if (player.isGrounded) snapGround = Vector3.down;
        player.Move(dashDesination + snapGround);
        isDashed = true;
    } 

    private void attack()
    {
        projectileLine.startColor = Color.red; //�� ���ε��� ���� �Ӽ���ȯ �׸񿡼� �ٷ��� ������.
        projectileLine.endColor = Color.red; //�� ���ε��� ���� �Ӽ���ȯ �׸񿡼� �ٷ��� ������.
        projectileLine.startWidth = 0.3f;
        projectileLine.endWidth = 0.3f;

        switch (this.isAimAttack)
        {
            case true:
                rayOrigin = Camera.GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
                
                projectileLine.SetPosition(0, ProjectileStart.position);
                if(Physics.Raycast(rayOrigin, Camera.transform.forward, out hitInfo, ShootDistance))
                {
                    projectileLine.SetPosition(1, hitInfo.point);
                    StartCoroutine(ShootEffect());
                    //hitInfo.collider.GetComponent<StatusControl>();  �´� ����� ������ ������. ���� �ٷ��� ����.
                }
                else
                {
                    projectileLine.SetPosition(1, rayOrigin + (Camera.transform.forward * ShootDistance));
                    StartCoroutine(ShootEffect());
                }
                break;
            case false:
                rayOrigin = ProjectileStart.position;
                projectileLine.SetPosition(0, ProjectileStart.position);
                if (Physics.Raycast(rayOrigin, player.transform.forward, out hitInfo, ShootDistance))
                {
                    projectileLine.SetPosition(1, hitInfo.point);
                    StartCoroutine(ShootEffect());
                    //hitInfo.collider.GetComponent<StatusControl>();  �´� ����� ������ ������. ���� �ٷ��� ����.
                }
                else
                {
                    projectileLine.SetPosition(1, ProjectileStart.position + (player.transform.forward * ShootDistance));
                    StartCoroutine(ShootEffect());
                }
                break;
        }
    }

    private void chargedAttack()
    {
        projectileLine.startColor = Color.blue; //�� ���ε��� ���� �Ӽ���ȯ �׸񿡼� �ٷ��� ������.
        projectileLine.endColor = Color.blue; //�� ���ε��� ���� �Ӽ���ȯ �׸񿡼� �ٷ��� ������.
        projectileLine.startWidth = 0.5f;
        projectileLine.endWidth = 0.5f;

        rayOrigin = Camera.GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
        projectileLine.SetPosition(0, ProjectileStart.position);
        if (Physics.Raycast(rayOrigin, Camera.transform.forward, out hitInfo, ShootDistance))
        {
            projectileLine.SetPosition(1, hitInfo.point);
            StartCoroutine(ShootEffect());
            //hitInfo.collider.GetComponent<StatusControl>();  �´� ����� ������ ������. ���� �ٷ��� ����.
        }
        else
        {
            projectileLine.SetPosition(1, rayOrigin + (Camera.transform.forward * ShootDistance));
            StartCoroutine(ShootEffect());
        }
    }

    private void elementSkill()
    {

    }

    private void elementUltSkill()
    {

    }

    private IEnumerator ShootEffect()
    {
        projectileLine.enabled = true;
        yield return shotDuration;
        projectileLine.enabled = false;
    }


}