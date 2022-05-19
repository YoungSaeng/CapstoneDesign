using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerMove : MonoBehaviour
{

    //**************************** ��ȣ �Է��ϸ� �ű⼭ �ٷ� ���۵�    
    [Header("�׽�Ʈ���� �������� ��ȣ")]
    public int testStage;



    [Header("**�÷��̾� �⺻ ����**")]

    public float Hp = 100;

    public float maxSpeed;   // �̵��ӵ�, 3.4f;

    public float jumpPower;  // ��������, 9.0f;

    public int jumpCount;    //���� ���� Ƚ��

    public int StageNum;   //�̰� ������?? - �������� ��ȣ! ������ endpoint�� �ο��Ǿ��־�

    public Slider hpslider;

    public BoxCollider2D foot; //�ݶ��̴� �ߺκи� ����

    /// ������ũ
    public GameObject gameSceneObj;
    public GameScene gameSceneREF;
    public GameObject instance;
    public Rigidbody2D rigid;
    Animator anim;

    public SpriteRenderer spr;



    [Header("*****���̽�ƽ*****")]

    // ���̽�ƽ ������Ʈ ������ ����
    public bl_Joystick js;

    // ���̽�ƽ�� ���� �������� ������Ʈ�� �ӵ�
    public float joySpeed;




    [Header("���� ���� Boolean***")]

    //���� ��Ҵ°�
    public bool isGround;

    //�������ΰ�
    bool isJumping;




    [Header("�߷� ���� Boolean***")]

    //�߷��� �����Ǿ��°�
    public bool isReverse;

    // GravityControl�� ��Ҵ°�
    public bool isGravityControl;




    [Header("���� ���� Boolean***")]

    // true�� ���� ����ִٴ� ��
    public bool ismoving;

    // true�� �ȿ����̰� �ִٴ� ��
    public bool playerstate;

    // �����̴� �÷��� ������!
    GameObject movingPlatformREF;
    //���������� ����������
    public bool ismovePlatform;




    [Header("*****���̺� ����Ʈ*****")]

    /// <summary>
    /// ��ŸƮ����Ʈ ����Ʈ
    /// </summary>
    public List<Transform> startPoints = new List<Transform>();

    /// <summary>
    /// ��������Ʈ ��ũ��Ʈ REF
    /// </summary>
    public EndPoint EndPointREF;

    // �߷���Ʈ�ѷ��� ������� �߰��� �� ������ ��
    public float ReversePos;

    /// <summary>
    /// ��ŸƮ ����Ʈ ��ũ��Ʈ REF
    /// </summary>
    /// 
    public StartPoint StartPointREF;




    //-----------------------------------------------------------
    //-----------------------------------------------------------
    //-------------------����Ƽ ���� �Լ�-------------------------
    //-----------------------------------------------------------
    //-----------------------------------------------------------



    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.gravityScale = 2.0f;   /// �߷� �ʱ�ȭ �����ָ� ����..

        gameSceneREF = gameSceneObj.GetComponent<GameScene>();
        anim = GetComponent<Animator>();

        spr = GetComponent<SpriteRenderer>();

    }

    public void Start()
    {
        instance = GameObject.Find("GameInstance");
        //gameSceneObj.GetComponent<GameScene>();

        //**************** ����뵵 �϶��� Ȱ��ȭ
        StartGame();

        //**************** �׽�Ʈ ���϶��� Ȱ��ȭ
        //TestGame(testStage);



    }

    void OnEnable()
    {
        spr.flipX = false;

        anim.SetInteger("Input", 0);
       
    }

    void OnDisable()
    {
        spr.flipX = false;
    }

    private void FixedUpdate()
    {

        KeyboardMove();

        JoyMove();

        /// ���� ���� 
        // �������� + ���� �������
        if (ismoving)
        {
            Vector3 curPos;


            curPos = Vector3.Lerp(transform.position, movingPlatformREF.transform.position,
            movingPlatformREF.GetComponent<TouchMonvigPlatform>().speed * Time.deltaTime);

            transform.position = new Vector3(transform.position.x, curPos.y);
            // �÷��̾ �����̴� ������ �̷��̷� ���󰡵��� ����

            ///������ ������ �÷��̾ ���Ƶ��̴� ���� �߻�... ���� ���.

        }








    }

    private void LateUpdate()
    {

        if (isReverse)
        {
            hpslider.transform.position = Camera.main.WorldToScreenPoint
                            (transform.position + new Vector3(0, -0.9f, 0));

        }
        else
        {
            hpslider.transform.position = Camera.main.WorldToScreenPoint
                            (transform.position + new Vector3(0, 0.9f, 0));


        }



    }

    void Update()
    {
        // �߷¹��������� �÷��̾� ȸ���� ����
        if (rigid.gravityScale > 0)
        {
            this.transform.localEulerAngles = new Vector3(0, 0, 0);

        }

        if (rigid.gravityScale < 0)
        {
            this.transform.localEulerAngles = new Vector3(180, 0, 0);

        }

        if (isReverse)
        {

            rigid.gravityScale = Mathf.Lerp(rigid.gravityScale, -2f, Time.deltaTime * 4f);
        }

        if (!isReverse)
        {

            rigid.gravityScale = Mathf.Lerp(rigid.gravityScale, 2f, Time.deltaTime * 4f);
        }




        // ��ƽ�� �̵�

        keyJump();




        hpslider.value = Hp / 100;



    }



    //-----------------------------------------------------------
    //-----------------------------------------------------------
    //-------------------����� ���� �Լ�-------------------------
    //-----------------------------------------------------------
    //-----------------------------------------------------------



    public void Jump()
    {
        if (jumpCount > 0)
        {
            anim.SetBool("IsJump", true);
            if (!isReverse)
            {
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                isJumping = true;

                jumpCount -= 1;
                playerstate = false;


            }

            if (isReverse)
            {

                rigid.AddForce(Vector2.down * jumpPower, ForceMode2D.Impulse);
                isJumping = true;

                jumpCount -= 1;
                playerstate = false;


            }



        }



    }


    public void keyJump()
    {

        if (jumpCount > 0)
        {
            if (Input.GetButtonDown("Jump"))
            {
                anim.SetBool("IsJump", true);
                if (!isReverse)
                {
                    rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                    isJumping = true;
                    isGround = false;
                    jumpCount -= 1;
                    playerstate = false;


                }
                if (isReverse)
                {

                    rigid.AddForce(Vector2.down * jumpPower, ForceMode2D.Impulse);

                    isJumping = true;
                    isGround = false;
                    jumpCount -= 1;
                    playerstate = false;


                }



            }



        }



    }






    void OnCollisionEnter2D(Collision2D collision)
    {

        // ���Ⱑ 45�������� �÷��������� �ߵ�,  ������ �߰� ������ �ѹ��ۿ� �ȵǵ��� �Ԥ�
        if (collision.contacts[0].normal.y > 0.7f || collision.contacts[0].normal.y < -0.7f)
        {
            isGround = true;
            anim.SetBool("IsJump", false);

            //�����̴� ���ǿ� ������� �±׼�������ߵǿ�!!
            if (collision.gameObject.tag == "Platform")
            {
                ///isGround = true;
                jumpCount = 2;

                // ���� ���������� �־��ֱ�
                movingPlatformREF = collision.gameObject;

                ismoving = true;
                ismovePlatform = false;

                //contactPlatform = collision.gameObject;

                //platformPos = contactPlatform.transform.position;

                //distance = platformPos-transform.position;

                //if(collision.gameObject.tag != "Platform")
                //{

                //    contactPlatform = null;
                //}


            }


        }





        if (collision.gameObject.tag == "ground")
        {
            anim.SetBool("IsJump", false);
            isGround = true;
            jumpCount = 2;


        }

                


        if (collision.gameObject.tag == "SmartEnemy")
        {

            OnDamaged(collision.transform.position, 15);


        }
        



        if (collision.gameObject.tag == "Death")
        {

            gameObject.SetActive(false);
            Invoke("Revive", 2f);


        }


        

        if (collision.gameObject.tag == "EndPoint")
        {

            EndPointREF = collision.gameObject.GetComponent<EndPoint>();
            //gameScene.stageNum = EndPointREF.nextStageNum;



            gameSceneREF.StageClearActive();




        }




        if (collision.gameObject.tag == "Item")
        {
            if (Hp < 100)
            {
                Hp += 30;

                collision.gameObject.SetActive(false);
            }


        }







    }



    void OnCollisionStay2D(Collision collision)
    {

        // ���Ⱑ 45�������� �÷��������� �ߵ�,  ������ �߰� ������ �ѹ��ۿ� �ȵǵ��� �Ԥ�
        if (collision.contacts[0].normal.y > 0.7f || collision.contacts[0].normal.y < -0.7f)
        {

            isGround = true; ///��� �����ϴ��� ��Ծpl
            jumpCount = 2;

        }


    }


    void OnCollisionExit2D(Collision2D collision)
    {
        //�����̴� �÷������� Ż���Ҷ�
        if (collision.gameObject.tag == "Platform")
        {
            ismoving = false;

            Debug.Log("���̽�ũ���԰�;� -����-");
            /// ����... -����-

        }

        if (collision.gameObject.tag == "ground")
        {

            isGround = false;
        }


    }





    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Enemy")
        {

            OnDamaged(collision.transform.position, 5);


        }



        if (collision.gameObject.tag == "SuperEnemy")
        {

            OnHit(collision.transform.position, 30);  //�� 30


        }



        if (collision.gameObject.tag == "Lazer")
        {

            Hp -= 30;
            gameObject.layer = 6;
            GetComponent<SpriteRenderer>().color = Color.red;

            Invoke("OffDamaged", 2f);


        }





        if (collision.gameObject.tag == "item")
        {
            if (Hp < 100)
            {
                Hp += 30;

                collision.gameObject.SetActive(false);
            }


        }





        if (collision.gameObject.tag == "reverseGravity")  //�Ķ�����
        {
            isReverse = false;
        }


        if (collision.gameObject.tag == "ReverseGravity")  //�ʷ�����
        {

            isReverse = true;
        }


        if (collision.gameObject.tag == "SuperReverseBlue") // ������ �Ķ�����
        {
            isReverse = false;
            rigid.velocity += new Vector2(0f, -15f);
        }

        if (collision.gameObject.tag == "SuperReverseGreen") // ������ �ʷ�����
        {
            isReverse = true;
            rigid.velocity += new Vector2(0, 15f);

        }


        if (collision.gameObject.tag == "GravityControl")  //��Ȳ����
        {
            if (rigid.gravityScale != 1.0f)
            {

                isGravityControl = true;
                StartCoroutine(GravityControl());


            }



        }





        if (collision.gameObject.tag == "StartPoint")
        {

            StartPoint savepoint = collision.gameObject.GetComponent<StartPoint>();

            StageNum = savepoint.stageNum;
            
            if(StageNum <= 50)
            {
                
                if ((StageNum % 10) == 0)
                {
                    gameSceneREF.StageText.SetText("���� �������� " + ((StageNum / 10) - 1) + "-" + 10);

                }
                else
                {
                    gameSceneREF.StageText.SetText("���� �������� " + (StageNum / 10) + "-" + (StageNum % 10));

                }

            }


            if (savepoint.stageNum <= 50) {


                if (StageNum >= PlayerPrefs.GetInt("SavedStage"))
                {

                    PlayerPrefs.SetInt("SavedStage", StageNum);


                    // ���� ����Ǿ��ִ� �������� ����
                    Debug.Log(PlayerPrefs.GetInt("SavedStage"));




                }
            }

            



        }




    }



    public void OnTriggerExit2D(Collider2D collision)
    {


    }





    /// �߷��� �������϶� �߷��� ��������� �ٲ��ְ�,
    /// �߷��� ����̸� isGravityControl true �϶��� 2.0���� �ٲ���
    IEnumerator reverseControl()
    {

        yield return new WaitForSeconds(0.1f);


        if (rigid.gravityScale > 0 && isGravityControl)   /// 1/2���� �Ķ� �������
        {
            isReverse = false;
            isGravityControl = false;

        }
        else if (rigid.gravityScale < 0 && isReverse)     /// �ʷϿ��� �Ķ� �������
        {
            isReverse = false;


        }
        else  ///������¿��� �Ķ�����, Ȥ�� �Ķ����� �Ķ�����
        {
            rigid.gravityScale = 2.0f;


        }



    }


    /// �߷��� ������϶� ���������� �ٲ���
    IEnumerator ReverseControl()
    {
        yield return new WaitForSeconds(0.1f);

        isReverse = true;





    }


    /// �߷��� 1/2�� �ٿ��� (�߷� ������ �����)
    IEnumerator GravityControl()
    {
        yield return new WaitForSeconds(0.1f);

        if (rigid.gravityScale != 1.0f && isGravityControl)
        {
            isReverse = false;
            rigid.gravityScale = 1.0f;
        }



    }







    void OnDamaged(Vector2 targetPos, float force)
    {

        if (Hp <= 0)
        {
            gameObject.SetActive(false);

            Invoke("Revive", 2f);

        }
        else if (Hp > 0 && gameObject.layer == 8)
        {
            Hp -= 15;


            GetComponent<SpriteRenderer>().color = Color.red;

            int dircX = transform.position.x - targetPos.x > 0 ? 1 : -1;

            rigid.AddForce(new Vector2(dircX, 1) * force, ForceMode2D.Impulse);


        }


        gameObject.layer = 6;

        Invoke("OffDamaged", 1.0f);



    }


    void OnHit(Vector2 targetPos, float force)
    {
        if (Hp <= 0)
        {
            gameObject.SetActive(false);

            Invoke("Revive", 3f);

        }
        else if (Hp > 0 && gameObject.layer == 8)
        {
            Hp -= 30;


            GetComponent<SpriteRenderer>().color = Color.red;

            int dircX = transform.position.x - targetPos.x > 0 ? 1 : -1;
            int dircY = transform.position.y - targetPos.y > 0 ? 1 : -1;

            rigid.AddForce(new Vector2(dircX, dircY) * force, ForceMode2D.Impulse);


        }

        gameObject.layer = 6;


        Invoke("OffDamaged", 2.5f);



    }


    void OffDamaged()
    {

        gameObject.layer = 8;

        GetComponent<SpriteRenderer>().color = Color.white;



    }



    public void Revive()
    {
        transform.position = startPoints[StageNum].transform.position;
        rigid.gravityScale = 2.0f;  /// �߷� �ʱ�ȭ �����ָ� ����.. Ȥ�� ���� �ѹ� �� ����.


        if (StageNum == 19 || StageNum == 68)
        {

            gameObject.SetActive(true);



            Hp = 100;

            isReverse = true;


        }
        else
        {
            gameObject.SetActive(true);



            Hp = 100;

            isReverse = false;


        }



    }



    public void NextStage()
    {
        isReverse = false;
        isGravityControl = false;

        transform.position = startPoints[EndPointREF.nextStageNum].transform.position;
        gameSceneREF.stageClearUnActive();

        Hp = 100;



    }







    public void JoyMove()
    {

        float h = js.Horizontal;


        if (h < 0)
        {
            spr.flipX = true;
            playerstate = false;
            anim.SetInteger("Input", -1);


        }

        else if (h > 0)
        {
            spr.flipX = false;
            playerstate = false;
            anim.SetInteger("Input", 1);


        }

        else if (h == 0)
        {
            playerstate = true;
            anim.SetInteger("Input", 0);


        }


        Vector3 curPos = transform.position;
        Vector3 nexPos = new Vector3(h, 0, 0) * joySpeed * Time.deltaTime;

        transform.position = curPos + nexPos;



    }











    public void StartGame()
    {

        transform.position = startPoints[instance.GetComponent<Instance>().stageNum].transform.position;
        //transform.position = startPoints[11].transform.position;  -->�����ϸ� ������ ����� �� (�׽�Ʈ��)

        if (instance.GetComponent<Instance>().stageNum == 8)
        {

            isReverse = true;

        }


    }



    public void TestGame(int testStage)
    {

        transform.position = startPoints[testStage].transform.position;



    }



    public void KeyboardMove()
    {

        float h = Input.GetAxisRaw("Horizontal");


        if (h < 0)
        {
            playerstate = false;
            anim.SetInteger("Input", -1);

        }

        else if (h > 0)
        {
            playerstate = false;
            anim.SetInteger("Input", 1);

        }

        else if (h == 0)
        {
            playerstate = true;
            anim.SetInteger("Input", 0);

        }


        Vector3 pos = this.transform.position;

        Vector3 nexpos = new Vector3(h, 0, 0) * maxSpeed * Time.deltaTime;

        transform.position = pos + nexpos;


    }




}