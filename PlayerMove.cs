using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerMove : MonoBehaviour
{

    //**************************** 번호 입력하면 거기서 바로 시작됨    
    [Header("테스트중인 스테이지 번호")]
    public int testStage;



    [Header("**플레이어 기본 정보**")]

    public float Hp = 100;

    public float maxSpeed;   // 이동속도, 3.4f;

    public float jumpPower;  // 점프높이, 9.0f;

    public int jumpCount;    //남은 점프 횟수

    public int StageNum;   //이건 뭔가용?? - 스테이지 번호! 각각의 endpoint에 부여되어있어

    public Slider hpslider;

    public BoxCollider2D foot; //콜라이더 발부분만 따로

    /// 어훼이크
    public GameObject gameSceneObj;
    public GameScene gameSceneREF;
    public GameObject instance;
    public Rigidbody2D rigid;
    Animator anim;

    public SpriteRenderer spr;



    [Header("*****조이스틱*****")]

    // 조이스틱 오브젝트 저장할 변수
    public bl_Joystick js;

    // 조이스틱에 의해 움직여질 오브젝트의 속도
    public float joySpeed;




    [Header("점프 관련 Boolean***")]

    //땅에 닿았는가
    public bool isGround;

    //점프중인가
    bool isJumping;




    [Header("중력 관련 Boolean***")]

    //중력이 반전되었는가
    public bool isReverse;

    // GravityControl에 닿았는가
    public bool isGravityControl;




    [Header("발판 관련 Boolean***")]

    // true면 발판 밟고있다는 뜻
    public bool ismoving;

    // true면 안움직이고 있다는 뜻
    public bool playerstate;

    // 움직이는 플랫폼 얻어오기!
    GameObject movingPlatformREF;
    //발판위에서 움직였을때
    public bool ismovePlatform;




    [Header("*****세이브 포인트*****")]

    /// <summary>
    /// 스타트포인트 리스트
    /// </summary>
    public List<Transform> startPoints = new List<Transform>();

    /// <summary>
    /// 엔드포인트 스크립트 REF
    /// </summary>
    public EndPoint EndPointREF;

    // 중력컨트롤러에 닿았을때 추가로 더 가해줄 힘
    public float ReversePos;

    /// <summary>
    /// 스타트 포인트 스크립트 REF
    /// </summary>
    /// 
    public StartPoint StartPointREF;




    //-----------------------------------------------------------
    //-----------------------------------------------------------
    //-------------------유니티 제공 함수-------------------------
    //-----------------------------------------------------------
    //-----------------------------------------------------------



    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.gravityScale = 2.0f;   /// 중력 초기화 안해주면 망함..

        gameSceneREF = gameSceneObj.GetComponent<GameScene>();
        anim = GetComponent<Animator>();

        spr = GetComponent<SpriteRenderer>();

    }

    public void Start()
    {
        instance = GameObject.Find("GameInstance");
        //gameSceneObj.GetComponent<GameScene>();

        //**************** 빌드용도 일때만 활성화
        StartGame();

        //**************** 테스트 중일때만 활성화
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

        /// 발판 관련 
        // 정지상태 + 발판 밟았을때
        if (ismoving)
        {
            Vector3 curPos;


            curPos = Vector3.Lerp(transform.position, movingPlatformREF.transform.position,
            movingPlatformREF.GetComponent<TouchMonvigPlatform>().speed * Time.deltaTime);

            transform.position = new Vector3(transform.position.x, curPos.y);
            // 플레이어가 움직이는 발판을 쫄래쫄래 따라가도록 만듬

            ///하지만 발판이 플레이어를 빨아들이는 버그 발생... 수정 요망.

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
        // 중력반전됐을때 플레이어 회전값 조절
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




        // 스틱이 이동

        keyJump();




        hpslider.value = Hp / 100;



    }



    //-----------------------------------------------------------
    //-----------------------------------------------------------
    //-------------------사용자 지정 함수-------------------------
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

        // 기울기가 45도이하인 플랫폼에서만 발동,  벽에선 추가 점프가 한번밖에 안되도록 함ㅋ
        if (collision.contacts[0].normal.y > 0.7f || collision.contacts[0].normal.y < -0.7f)
        {
            isGround = true;
            anim.SetBool("IsJump", false);

            //움직이는 발판에 닿았을때 태그설정해줘야되요!!
            if (collision.gameObject.tag == "Platform")
            {
                ///isGround = true;
                jumpCount = 2;

                // 닿은 발판정보를 넣어주기
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

        // 기울기가 45도이하인 플랫폼에서만 발동,  벽에선 추가 점프가 한번밖에 안되도록 함ㅋ
        if (collision.contacts[0].normal.y > 0.7f || collision.contacts[0].normal.y < -0.7f)
        {

            isGround = true; ///어떻게 수정하는지 까먹어서pl
            jumpCount = 2;

        }


    }


    void OnCollisionExit2D(Collision2D collision)
    {
        //움직이는 플랫폼에서 탈출할때
        if (collision.gameObject.tag == "Platform")
        {
            ismoving = false;

            Debug.Log("아이스크림먹고싶어 -영생-");
            /// 나도... -선혜-

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

            OnHit(collision.transform.position, 30);  //값 30


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





        if (collision.gameObject.tag == "reverseGravity")  //파랑장판
        {
            isReverse = false;
        }


        if (collision.gameObject.tag == "ReverseGravity")  //초록장판
        {

            isReverse = true;
        }


        if (collision.gameObject.tag == "SuperReverseBlue") // 강력한 파랑장판
        {
            isReverse = false;
            rigid.velocity += new Vector2(0f, -15f);
        }

        if (collision.gameObject.tag == "SuperReverseGreen") // 강력한 초록장판
        {
            isReverse = true;
            rigid.velocity += new Vector2(0, 15f);

        }


        if (collision.gameObject.tag == "GravityControl")  //주황장판
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
                    gameSceneREF.StageText.SetText("현재 스테이지 " + ((StageNum / 10) - 1) + "-" + 10);

                }
                else
                {
                    gameSceneREF.StageText.SetText("현재 스테이지 " + (StageNum / 10) + "-" + (StageNum % 10));

                }

            }


            if (savepoint.stageNum <= 50) {


                if (StageNum >= PlayerPrefs.GetInt("SavedStage"))
                {

                    PlayerPrefs.SetInt("SavedStage", StageNum);


                    // 현재 저장되어있는 스테이지 정보
                    Debug.Log(PlayerPrefs.GetInt("SavedStage"));




                }
            }

            



        }




    }



    public void OnTriggerExit2D(Collider2D collision)
    {


    }





    /// 중력이 음수값일때 중력을 양수값으로 바꿔주고,
    /// 중력이 양수이며 isGravityControl true 일때는 2.0으로 바꿔줌
    IEnumerator reverseControl()
    {

        yield return new WaitForSeconds(0.1f);


        if (rigid.gravityScale > 0 && isGravityControl)   /// 1/2에서 파랑 닿았을때
        {
            isReverse = false;
            isGravityControl = false;

        }
        else if (rigid.gravityScale < 0 && isReverse)     /// 초록에서 파랑 닿았을때
        {
            isReverse = false;


        }
        else  ///평범상태에서 파랑닿음, 혹은 파랑에서 파랑닿음
        {
            rigid.gravityScale = 2.0f;


        }



    }


    /// 중력이 양수값일때 음수값으로 바꿔줌
    IEnumerator ReverseControl()
    {
        yield return new WaitForSeconds(0.1f);

        isReverse = true;





    }


    /// 중력을 1/2로 줄여줌 (중력 무조건 양수임)
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
        rigid.gravityScale = 2.0f;  /// 중력 초기화 안해주면 망함.. 혹시 몰라서 한번 더 넣음.


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
        //transform.position = startPoints[11].transform.position;  -->시작하면 무조건 여기로 감 (테스트용)

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