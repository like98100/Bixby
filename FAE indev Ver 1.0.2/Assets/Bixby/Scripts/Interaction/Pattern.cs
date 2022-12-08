using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern : MonoBehaviour
{
    //private RaycastHit hit;
    public Vector3 HitPoint;
    public Vector3 HitNormal;
    public Vector3 PatternNormal;

    public NORMAL nor = NORMAL.NONE;

    public enum NORMAL
    {
        NONE = 0,
        X_minus = 1,
        X_plus = 2,
        Y_minus = 3,
        Y_plus = 4,
        Z_minus = 5,
        Z_plus = 6,
    };

    public bool state = false; //패턴을 맞췄는지 아닌지 판단
    public float isYRot = -90.0f; //맞춰야하는 y값의 로테이션
    public float yEuler; //y회전값

    // Start is called before the first frame update

    private void Awake()
    {

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rotationPattern();

        yEuler = round90(transform.localRotation.eulerAngles.y);

        if (yEuler == 270 || yEuler == -90)
        {
            state = true;
        }
        else
            state = false;
    }

    void rotationPattern()
    {
        PatternNormal = Quaternion.AngleAxis(transform.parent.eulerAngles.y, -Vector3.up) * HitNormal;

        //공격을 받으면
        if (HitNormal != new Vector3(0,0,0))
        {
            if (Mathf.Round(PatternNormal.x) == -1)
            {
                //상태 변경
                nor = NORMAL.X_minus;
            }
            else if (Mathf.Round(PatternNormal.x) == 1)

            {
                //상태 변경
                nor = NORMAL.X_plus;
            }
            else if (Mathf.Round(PatternNormal.y) == -1)
            {
                //상태 변경
                nor = NORMAL.Y_minus;
            }
            else if (Mathf.Round(PatternNormal.y) == 1)
            {
                //상태 변경
                nor = NORMAL.Y_plus;
            }
            else if (Mathf.Round(PatternNormal.z) == -1)               
            {
                //상태 변경
                nor = NORMAL.Z_minus;
            }
            else if (Mathf.Round(PatternNormal.z) == 1)
            {
                //상태 변경
                nor = NORMAL.Z_plus;
            }
            else
            {
                nor = NORMAL.NONE;
            }

            HitNormal = new Vector3(0, 0, 0);
        }

        switch (nor)
        {
            case NORMAL.NONE:
                break;
            case NORMAL.X_minus:
                //회전
                if (rotating == false)
                {
                    if (clockWise)
                        StartCoroutine(rotPattern(CLOCKWISE, 1));
                    else
                        StartCoroutine(rotPattern(ANTI_CLOCKWISE, 1));
                }
                //노말벡터 초기화
                //상태 초기화
                nor = NORMAL.NONE;
                break;
            case NORMAL.X_plus:
                //회전
                if (rotating == false)
                {
                    if (clockWise)
                        StartCoroutine(rotPattern(CLOCKWISE, 2));
                    else
                        StartCoroutine(rotPattern(ANTI_CLOCKWISE, 2));
                }
                //노말벡터 초기화
                //상태 초기화
                nor = NORMAL.NONE;
                break;
            case NORMAL.Y_minus:
                break;
            case NORMAL.Y_plus:
                break;
            case NORMAL.Z_minus:
                //회전
                
                //노말벡터 초기화
                //상태 초기화
                nor = NORMAL.NONE;
                break;
            case NORMAL.Z_plus:
                //회전
               
                //노말벡터 초기화
                //상태 초기화
                nor = NORMAL.NONE;
                break;
            default:
                break;
        }
    }

    bool rotating;

    public bool clockWise;
    Vector3 CLOCKWISE = Vector3.up;
    Vector3 ANTI_CLOCKWISE = Vector3.down;

    Vector3 a = Vector3.right;
    Vector3 b = Vector3.left;

    float rotateTime = 1f;

    float round90(float f)
    {
        float r = f % 90;
        return (r < 45) ? f - r : f - r + 90;
    }

    private IEnumerator rotPattern(Vector3 wise, int num)
    {
        //y+
        if (num == 1)
        {
            rotating = true;

            this.transform.Rotate(new Vector3(0, wise.y, 0));

            float elapsedTime = 0.0f;

            Quaternion currentRotation = this.transform.localRotation;
            Vector3 targetEulerAngles = this.transform.localRotation.eulerAngles;
            targetEulerAngles.y += (88.0f) * wise.y;

            Quaternion targetRotation = Quaternion.Euler(targetEulerAngles);

            while (elapsedTime < rotateTime)
            {
                transform.localRotation
                    = Quaternion.Euler(Vector3.Lerp(
                        currentRotation.eulerAngles, targetRotation.eulerAngles, elapsedTime / rotateTime)
                    );

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            targetEulerAngles.y = round90(targetEulerAngles.y);
            this.transform.localRotation = Quaternion.Euler(targetEulerAngles);

            rotating = false;
        }
        //y-
        else if (num == 2)
        {
            rotating = true;

            this.transform.Rotate(new Vector3(0, -wise.y, 0));

            float elapsedTime = 0.0f;

            Quaternion currentRotation = this.transform.localRotation;
            Vector3 targetEulerAngles = this.transform.localRotation.eulerAngles;
            targetEulerAngles.y += (88.0f) * -wise.y;

            Quaternion targetRotation = Quaternion.Euler(targetEulerAngles);

            while (elapsedTime < rotateTime)
            {
                transform.localRotation
                    = Quaternion.Euler(Vector3.Lerp(
                        currentRotation.eulerAngles, targetRotation.eulerAngles, elapsedTime / rotateTime)
                    );

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            targetEulerAngles.y = round90(targetEulerAngles.y);
            this.transform.localRotation = Quaternion.Euler(targetEulerAngles);

            rotating = false;
        }
        else if (num == 3)
        {

        }
        else if (num == 4)
        {

        }
        //x+
        else if (num == 5)
        {

        }
        //x-
        else if (num == 6)
        {

        }
    }


    public void GetP(Vector3 hitPiont, Vector3 hitNormal)
    {
        HitPoint = hitPiont;
        HitNormal = hitNormal;
    }
}
