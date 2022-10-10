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
        animator.SetBool("isRun", false);
        animator.SetBool("isJump", false);
        animator.SetBool("isAtk", false);
        animator.SetBool("isFall", false);
        animator.SetBool("isDead", false);
    }

    // Update is called once per frame
    void Update()
    {
        // �켱 ������ ���� switch�� �ƴ� if�� ��������� State ���� ������ �Ϸ�Ǹ� switch ���·� ���� ������ ��

        //if(ü���� �� �޾��� ��)
        //    {
        //    animator.SetBool("isDead", true);
        //}

        if (!modelContorl.GetIsGrounded())      // ���߿� �� ������ ��
        {
            animator.SetBool("isJump", true);   // Jump �ִϸ��̼� ����

            if(animator.GetCurrentAnimatorStateInfo(0).IsName("Jump")               // ���߿� �� �����鼭(���� ���ǹ�) ���� �ִϸ��̼��� �������� ���°�,
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


        //if (modelContorl.State == PlayerContorl.STATE.MOVE) // Move ������ ��
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) 
            || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))  // WASD �Է��� �߻��� ��
            animator.SetBool("isRun", true);                        // �̵� �ִϸ��̼� ����
        else animator.SetBool("isRun", false);

        // �ƹ�ư ���� ������ ��(����� ���� ��� ���Ƿ� ����)
        if (modelContorl.State == PlayerContorl.STATE.ATTACK || modelContorl.State == PlayerContorl.STATE.CHARGE_ATTACK || modelContorl.State == PlayerContorl.STATE.ELEMENT_ULT_SKILL)
            animator.SetBool("isAtk", true);    // ���� �ִϸ��̼� ����
        else animator.SetBool("isAtk", false);
    }
}
