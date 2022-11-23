using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    private GameObject player;
    public float xmove = 0; 
    public float ymove = 0;  
    public float distanceMoveMod = 8.0f;
    public float distanceAimMod = 2.0f;

    Vector3 revDistanceMove;
    Vector3 revDistanceAim;
    Vector3 playerCenterCustom = new Vector3(0f, 1.0f, 0f);
    private float transitionSpeed = 30.0f;

    public bool isOnAim = false;

    public STATE step = STATE.NONE; // ���� ����.
    public STATE next_step = STATE.NONE; // ���� ����.
    public float step_timer = 0.0f; // Ÿ�̸�
    public float mouseSenseX;
    public float mouseSenseY;
    public enum STATE
    {
        NONE = -1,
        MOVE = 0,
        AIM = 1,

        NUM = 3, // ���� ����
    };

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        revDistanceMove = new Vector3(0.0f, -2.0f, distanceMoveMod);
        revDistanceAim = new Vector3(-1.2f, -0.5f, distanceAimMod);

        this.step = STATE.MOVE;
        this.next_step = STATE.MOVE;
        mouseSenseX = 1;
        mouseSenseY = 1;
    }

    void Update()
    {
        xmove += Input.GetAxis("Mouse X") * mouseSenseX;
        ymove -= Input.GetAxis("Mouse Y") * mouseSenseY;

        ymove = Mathf.Clamp(ymove, -55.0f, 55.0f);
        transform.rotation = Quaternion.Euler(ymove, xmove, 0);

        this.step_timer += Time.deltaTime;
        if (this.next_step == STATE.NONE)
        {
            switch (this.step)
            {
                case STATE.MOVE:
                    if (isOnAim) this.next_step = STATE.AIM;
                    break;
                case STATE.AIM:
                    if (!isOnAim) this.next_step = STATE.MOVE;
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
                case STATE.MOVE:

                    break;
                case STATE.AIM:

                    break;
            }
            this.step_timer = 0.0f;
        }
        // �� ��Ȳ���� �ݺ��� ��----------.
        switch (this.step)
        {
            case STATE.MOVE:
                moveCamMod();
                break;
            case STATE.AIM:
                aimCamMod();
                break;
        }
    }

    void moveCamMod()
    {
        //transform.position = player.transform.position - transform.rotation * revDistanceMove;
        this.transform.position = Vector3.Lerp(transform.position,
            (player.transform.position + playerCenterCustom) - this.transform.rotation * revDistanceMove,
            Time.deltaTime * transitionSpeed);

        RaycastHit hitinfo;
        if (Physics.Linecast((player.transform.position + playerCenterCustom), this.transform.position, out hitinfo, 1 << LayerMask.NameToLayer("Ground")) ||
            Physics.Linecast((player.transform.position + playerCenterCustom), this.transform.position, out hitinfo, 1 << LayerMask.NameToLayer("Water")))
        {
            this.transform.position = hitinfo.point;
        }
    }

    void aimCamMod()
    {
        //transform.position = player.transform.position - transform.rotation * revDistanceAim;
        this.transform.position = Vector3.Lerp(transform.position,
            (player.transform.position + playerCenterCustom) - this.transform.rotation * revDistanceAim,
            Time.deltaTime * transitionSpeed);

        RaycastHit hitinfo;
        if (Physics.Linecast((player.transform.position + playerCenterCustom), this.transform.position, out hitinfo, 1 << LayerMask.NameToLayer("Ground")) ||
            Physics.Linecast((player.transform.position + playerCenterCustom), this.transform.position, out hitinfo, 1 << LayerMask.NameToLayer("Water")))
        {
            this.transform.position = hitinfo.point;
        }
    }
}
