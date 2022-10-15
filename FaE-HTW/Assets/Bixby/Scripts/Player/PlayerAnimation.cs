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

        if (!modelContorl.GetIsGrounded())      // ���߿� �� ������ ��
        {
            animator.SetBool("isCombat", true);
            animator.SetBool("isJump", true);   // Jump �ִϸ��̼� ����

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump")               // ���߿� �� �����鼭(���� ���ǹ�) ���� �ִϸ��̼��� �������� ���°�,
                && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)  // �ִϸ��̼� ���� ��Ȳ�� 90&�� �Ѿ�� ��
            {
                //animator.SetBool("isFall", true);   // Fall �ִϸ��̼� ����
            }
        }
        else
        {
            animator.SetBool("isJump", false);
            animator.SetBool("isFall", false);
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
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A)
                || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))  // WASD �Է��� �߻��� ��
                {
                    animator.SetBool("isRun", true);                    // �̵� �ִϸ��̼� ����
                }
                else animator.SetBool("isRun", false);                  // �̵� �ִϸ��̼� ����

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

                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A)
                || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))  // WASD �Է��� �߻��� ��
                {
                    animator.SetBool("isRun", true);                    // �̵� �ִϸ��̼� ����
                }
                else animator.SetBool("isRun", false);                  // �̵� �ִϸ��̼� ����

                break;

            case PlayerContorl.STATE.CHARGE_ATTACK:                     // �� ���� ����
                if(Input.GetMouseButtonUp(0))                           // ���콺 �� Ŭ�� �Է��� �����ƴٸ�
                    animator.SetBool("isCharge", false);                // ���� ���� ����
                else animator.SetBool("isCharge", true);                // ���� ���� ����

                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A)
                || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))  // WASD �Է��� �߻��� ��
                {
                    animator.SetBool("isRun", true);                    // �̵� �ִϸ��̼� ����
                }
                else animator.SetBool("isRun", false);                  // �̵� �ִϸ��̼� ����

                break;

            case PlayerContorl.STATE.ELEMENT_ULT_SKILL:                 // �ñر� ��� ����
                animator.SetBool("isCombat", true);                     // ������ ���� ��Ȱ��ȭ
                animator.SetBool("isAtk", true);                        // ���� �ִϸ��̼� ����
                break;

            case PlayerContorl.STATE.AIM:                               // ���� ����
                animator.SetBool("isAim", true);                        // ���� �ִϸ��̼� ����

                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A)
                || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))  // WASD �Է��� �߻��� ��
                {
                    animator.SetBool("isRun", true);                    // �̵� �ִϸ��̼� ����
                }
                else animator.SetBool("isRun", false);                  // �̵� �ִϸ��̼� ����
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
    }
}
