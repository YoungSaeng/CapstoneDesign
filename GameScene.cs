using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameScene : MonoBehaviour
{

    public TextMeshProUGUI itemText; // ?

    public GameObject PauseBtn;  //일시정지 버튼
    public GameObject pauseGroup; //일시정지 창

    public GameObject settingGroup; // 환경설정 창

    public GameObject stageClearGroup; // 스테이지 클리어 창
    public GameObject chapterClearGroup; // 챕터 클리어 창

    public GameObject instance;
    public GameObject player;

    public List<Transform> savePoints = new List<Transform>();
    
    public TextMeshProUGUI StageText; // 현재 스테이지 알려주는 텍스트


    public int stageNum;


    void Start()
    {
        PauseBtn.SetActive(true);
        instance = GameObject.Find("GameInstance");
        player = GameObject.Find("Player");

        StageText.GetComponent<TextMeshProUGUI>();
           
    }
    void Awake()
    {
        PauseBtn.SetActive(true);
        instance = GameObject.Find("GameInstance");
        player = GameObject.Find("Player");

        // ****************************시작시 스테이지 정보 없으면 자동으로 로고씬 이동됨
        if (instance == null)
        {
            SceneManager.LoadScene("1_LogoScene");
        }

        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        /// 탈출
        if (Input.GetButtonDown("Cancel")) // && !isOver
        {
            PauseActive();
        }
    }





    public void EnableItemText()
    {

        itemText.gameObject.SetActive(true);

    }


    public void PauseActive()
    {
        Time.timeScale = 0f;
        pauseGroup.SetActive(true);

    }
    public void PauseUnActive()
    {
        Time.timeScale = 1.0f;
        pauseGroup.SetActive(false);

    }


    public void SettingActive()
    {
        settingGroup.SetActive(true);

    }
    public void SettingUnActive()
    {
        settingGroup.SetActive(false);

    }


    public void StageClearActive()
    {
        stageClearGroup.SetActive(true);
        Time.timeScale = 0.0f;        

    }
    public void stageClearUnActive()
    {
        stageClearGroup.SetActive(false);
        Time.timeScale = 1.0f;

    }


    public void BackToMain()
    {
        ///Pause에서 메인으로 갈때, 딜레이 없이 바로 메인으로 감
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
        Debug.Log("0번 씬 불러오기");
    }

    public void ReadytoNextStage(int nextstageNum)
    {

    }





}
