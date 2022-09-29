using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContorl : StatusControl
{
    public GameObject Camera;
    Vector3 cameraForward;
    Vector3 cameraRight;

    Vector3 playerForward;
    Vector3 playerRight;

    Vector3 dashDesination;

    private CharacterController player;

    private Vector3 playerDirection = Vector3.forward;
    public Vector3 DefaultForward = Vector3.forward;
    bool isJumpPressed = false;

    private Vector3 attackHitPoint;
    private Ray ray;
    private RaycastHit hitInfo;

    public STATE step = STATE.NONE; // ���� ����.
    public STATE next_step = STATE.NONE; // ���� ����.
    public float step_timer = 0.0f; // Ÿ�̸�
    public enum STATE
    {
        NONE = -1,
        IDLE = 0,
        MOVE = 1,
        AIM = 2,
        DASH = 3,
        RUN = 4,

        NUM = 5, // ���� ����
    };

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        this.step = STATE.NONE;
        this.next_step = STATE.IDLE;

        player = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        vectorAlign();

            this.step_timer += Time.deltaTime;
        if (this.next_step == STATE.NONE)
        {
            switch (this.step)
            {
                case STATE.IDLE:
                    this.next_step = STATE.MOVE; //����� ���߿� �Ų����� ��� ������Ʈ�� ������ �Ǿ�� �Ѵ�.
                    break;
                case STATE.MOVE:
                    if (Input.GetMouseButton(1))
                        this.next_step = STATE.AIM;
                    if (Input.GetKeyDown(KeyCode.LeftShift) && stamina > dashStaminaAmount && player.isGrounded)
                        this.next_step = STATE.DASH;
                    break;
                case STATE.AIM:
                    if (!Input.GetMouseButton(1))
                        this.next_step = STATE.MOVE;
                    if (Input.GetKeyDown(KeyCode.LeftShift) && stamina > dashStaminaAmount && player.isGrounded)
                        this.next_step = STATE.DASH;
                    break;
                case STATE.DASH:
                    if (Input.GetKey(KeyCode.LeftShift))
                        this.next_step = STATE.RUN;
                    else
                        this.next_step = STATE.MOVE;
                    break;
                case STATE.RUN:
                    if (!Input.GetKey(KeyCode.LeftShift) || stamina <= 0)
                        this.next_step = STATE.MOVE;
                    break;
            }
        }

        // ���°� ��ȭ���� ��------------.
        while (this.next_step != STATE.NONE)
        { // ���°� NONE�̿� = ���°� ��ȭ�ߴ�.
            this.step = this.next_step;
            this.next_step = STATE.NONE;
            switch (this.step)
            {
                case STATE.IDLE:

                    break;
                case STATE.MOVE:
                    myCurrentSpeed = Speed;
                    Camera.GetComponent<CamControl>().isOnAim = false;
                    break;
                case STATE.AIM:
                    Camera.GetComponent<CamControl>().isOnAim = true;
                    break;
                case STATE.DASH:
                    Camera.GetComponent<CamControl>().isOnAim = false;
                    StaminaUse(dashStaminaAmount);
                    dashDesination = playerDirection;
                    dashDesination.y = 0.0f;
                    isDashed = false;
                    Debug.Log("Dash!");
                    break;
                case STATE.RUN:
                    myCurrentSpeed = RunSpeed;
                    Camera.GetComponent<CamControl>().isOnAim = false;
                    break;
            }
            this.step_timer = 0.0f;
        }
        // �� ��Ȳ���� �ݺ��� ��----------.
        switch (this.step)
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
                StaminaTickUse(runStaminaAmount);
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
            playerDirection *= myCurrentSpeed;

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
        {
            playerDirection.y -= GravityForce * Time.deltaTime;
        }

        Vector3 snapGround = Vector3.zero;
        if (player.isGrounded) snapGround = Vector3.down;

        player.Move(playerDirection * Time.deltaTime + snapGround);
    }

    private void dash()
    {
        Vector3 snapGround = Vector3.zero;
        if (player.isGrounded) snapGround = Vector3.down;
        player.Move(dashDesination + snapGround);
        isDashed = true;
    } //����
}