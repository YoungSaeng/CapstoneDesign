using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameScene : MonoBehaviour
{

    public TextMeshProUGUI itemText; // ?

    public GameObject PauseBtn;  //�Ͻ����� ��ư
    public GameObject pauseGroup; //�Ͻ����� â

    public GameObject settingGroup; // ȯ�漳�� â

    public GameObject stageClearGroup; // �������� Ŭ���� â
    public GameObject chapterClearGroup; // é�� Ŭ���� â

    public GameObject instance;
    public GameObject player;

    public List<Transform> savePoints = new List<Transform>();
    
    public TextMeshProUGUI StageText; // ���� �������� �˷��ִ� �ؽ�Ʈ


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

        // ****************************���۽� �������� ���� ������ �ڵ����� �ΰ�� �̵���
        if (instance == null)
        {
            SceneManager.LoadScene("1_LogoScene");
        }

        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        /// Ż��
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
        ///Pause���� �������� ����, ������ ���� �ٷ� �������� ��
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
        Debug.Log("0�� �� �ҷ�����");
    }

    public void ReadytoNextStage(int nextstageNum)
    {

    }





}
