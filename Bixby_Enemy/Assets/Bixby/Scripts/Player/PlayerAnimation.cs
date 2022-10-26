using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    PlayerContorl modelContorl; // �÷��̾� ���� PlayerContorl ��ũ��Ʈ
    Animator animator;          // Animator ������Ʈ

    // Start is called before the first frame update
    void Start()
    {
        modelContorl = this.gameObject.GetComponent<PlayerContorl>();
        animator = this.gameObject.transform.GetChild(1).GetComponent<Animator>();

        // Animator ���� �ʱ�ȭ
        initializeAnimParameter();
    }

    // Update is called once per frame
    void Update()
    {
        //if(ü���� �� �޾��� ��)
        //    {
        //    animator.SetBool("isDead", true);
        //}

        if (!modelContorl.GetIsGrounded() && !animator.GetCurrentAnimatorStateInfo(0).IsName("Landing"))      // ���߿� �� �����̸� ���� ���°� �ƴ� ��
        {
            animator.SetBool("isCombat", true);
            animator.SetBool("isJump", true);   // Jump �ִϸ��̼� ����

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump")               // ���߿� �� �����鼭(���� ���ǹ�) ���� �ִϸ��̼��� �������� ���°�,
                && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.475f)  // �ִϸ��̼� ���� ��Ȳ�� 47.5&�� �Ѿ�� ��
            {
                animator.SetBool("isFall", true);   // Fall �ִϸ��̼� ����
            }
        }
        else                                    // ���� �ִ� ������ ��
        {
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))               // ���� �ִϸ��̼��� �������� ������ ��
            {
                animator.SetBool("isLanding", true);
            }
            else if(animator.GetCurrentAnimatorStateInfo(0).IsName("Falling"))      // �߶� �ִϸ��̼��� �������� ������ ��
            {
                animator.SetBool("isLanding", true);
            }
            else                                                                    // ���� �ִ� ���°�, ����, �߶� �ִϸ��̼��� �ƴ� ��
            {
                animator.SetBool("isJump", false);
                animator.SetBool("isFall", false);
                animator.SetBool("isLanding", false);
            }
        }

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Dash") &&                // Dash �ִϸ��̼��� ����Ǵ� ���¿���
            Input.GetKeyDown(KeyCode.LeftShift))                                    // LeftShift �Է��� �߻��� ��
        {
            animator.Play("Dash", 0);
        }

        if(animator.GetCurrentAnimatorStateInfo(1).IsName("Charge") &&              // Charge �ִϸ��̼��� ����Ǵ� ���¿���
            !Input.GetKey(KeyCode.Mouse0))                                         // �� Ŭ�� �Է��� ���� ������ ���� ��
        {
            animator.SetBool("isCharge", false);                                    // �ִϸ��̼� ���� ���� ���� ��
        }

        switch (modelContorl.State)
        {
            case PlayerContorl.STATE.IDLE:                              // ������ ����
                animator.SetBool("isCombat", false);                    // ������ ���� Ȱ��ȭ
                animator.SetBool("isAim", false);                       // ���� �ִϸ��̼� ����
                break;

            case PlayerContorl.STATE.MOVE:                              // �̵� ����
                animator.SetBool("isCombat", true);                     // ������ ���� ��Ȱ��ȭ
                animator.SetBool("isAtk", false);                       // ���� �ִϸ��̼� ����
                animator.SetBool("isAim", false);                       // ���� �ִϸ��̼� ����
                animator.SetBool("isSwimming", false);                  // ���� �ִϸ��̼� ����
                animator.SetBool("isDash", false);                      // ��� �ִϸ��̼� ����

                moveCheck();

                break;

            case PlayerContorl.STATE.DASH:                              // ��� ����
                animator.SetBool("isDash", true);                       // ��� �ִϸ��̼� ����
                break;

            case PlayerContorl.STATE.RUN:                               // �޸��� ����
                animator.SetBool("isDash", false);                      // ��� �ִϸ��̼� ����
                moveCheck();
                break;

            case PlayerContorl.STATE.SWIMMING:                          // ���� ����
                animator.SetBool("isSwimming", true);                   // ���� �ִϸ��̼� ����
                break;

            case PlayerContorl.STATE.ATTACK:                            // �� ���� ����
                animator.SetBool("isCombat", true);                     // ������ ���� ��Ȱ��ȭ
                animator.SetBool("isAtk", true);                        // ���� �ִϸ��̼� ����

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Normal Attack")                 // ���� �ִϸ��̼��� �������� ���°�,
                && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f                   // �ִϸ��̼� ���� ��Ȳ�� 90&�� �Ѿ��
                && Input.GetMouseButton(0))                                                         // ���콺 �� Ŭ�� �Է��� Ȱ��ȭ�Ǿ� ���� ��
                {
                    animator.SetBool("isCharge", true);                 // ���� ���� Ȱ��ȭ
                }

                moveCheck();

                break;

            case PlayerContorl.STATE.CHARGE_ATTACK:                     // �� ���� ����
                if(Input.GetMouseButtonUp(0))                           // ���콺 �� Ŭ�� �Է��� �����ƴٸ�
                    animator.SetBool("isCharge", false);                // ���� ���� ����
                else animator.SetBool("isCharge", true);                // ���� ���� ����

                moveCheck();

                break;

            case PlayerContorl.STATE.ELEMENT_ULT_SKILL:                 // �ñر� ��� ����
                animator.SetBool("isCombat", true);                     // ������ ���� ��Ȱ��ȭ
                animator.SetBool("isAtk", true);                        // ���� �ִϸ��̼� ����
                break;

            case PlayerContorl.STATE.AIM:                               // ���� ����
                animator.SetBool("isAim", true);                        // ���� �ִϸ��̼� ����
                animator.SetBool("isAtk", false);                       // ���� �ִϸ��̼� ����

                moveCheck();
                break;
        }
    }

    void initializeAnimParameter()        // �ִϸ��̼� �з����� �ʱ�ȭ �Լ�
    {
        animator.SetBool("isRun", false);
        animator.SetBool("isJump", false);
        animator.SetBool("isAtk", false);
        animator.SetBool("isFall", false);
        animator.SetBool("isCombat", false);
        animator.SetBool("isCharge", false);
        animator.SetBool("isAim", false);
        animator.SetBool("isLanding", false);
        animator.SetBool("isSwimming", false);
        animator.SetBool("isDash", false);
    }


    void moveCheck()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A)
        || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))  // WASD �Է��� �߻��� ��
        {
            animator.SetBool("isRun", true);                    // �̵� �ִϸ��̼� ����
        }
        else animator.SetBool("isRun", false);                  // �̵� �ִϸ��̼� ����

        if (animator.GetCurrentAnimatorStateInfo(1).IsName("Aim"))   // ���� ���� ������ ��
            animator.SetLayerWeight(1, 0.6f);                   // ����ġ ����
        else animator.SetLayerWeight(1, 0.9f);                   // ����ġ ����
    }
}
