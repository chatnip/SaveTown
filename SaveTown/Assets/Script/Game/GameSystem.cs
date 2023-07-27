using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using NaughtyAttributes;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameSystem : MonoBehaviour
{
    [Header("*Component")]
    [SerializeField] GameManager GameManager;
    [SerializeField] ChapterManager ChapterManager;
    [SerializeField] DialogManager DialogManager;
    [SerializeField] EnemyManager EnemyManager;
    [SerializeField] ObjectPooling ObjectPooling;

    [Header("*UI")]
    [SerializeField] Image health;
    [SerializeField] Text scoreText;
    [SerializeField] Text timeText;
    [SerializeField] Image warningImage;

    [Header("*Canvas")]
    [SerializeField] public GameObject Game;
    [SerializeField] public GameObject Lobby;

    [Header("*BackGround")]
    [SerializeField] public Image gameBackGroundImage;
    [SerializeField] public AudioSource gameAudioSource;
    [HideInInspector] public AudioClip gameBackGroundSound;


    [Header("*ResultWindow")]
    [SerializeField] GameObject resultWindow;
    [SerializeField] Image resultImage;
    [SerializeField] Text resultScoreText;
    [SerializeField] Button goToLobbyButton;
    [Space(10)]
    [SerializeField] public Sprite winSprite;
    [SerializeField] public Sprite loseSprite;

    #region Setting
    [HideInInspector] public Sprite s_enemySprite;     // Enemy 스프라이트
    [HideInInspector] public int i_enemyAmount;        // Enemy의 갯수
    [HideInInspector] public int i_enemyTimeInterval;  // Enemy가 나오는 간격
    [HideInInspector] public int i_enemySpeed;         // Enemy의 속도
    [HideInInspector] public int i_arrowAmount;        // Arrow의 갯수
    [HideInInspector] public int i_arrowSpriteNum;     // Arrow의 모양 갯수
    [HideInInspector] public int i_arrowTextNum;       // Arrow의 숫자 텍스트
    [HideInInspector] public int i_enemyDamage;        // Enemy가 주는 데미지

    [HideInInspector] public Sprite s_bossEnemySprite;     // Boss 스프라이트
    [HideInInspector] public int i_bossSpeed;              // Boss의 속도
    [HideInInspector] public int i_bossArrowAmount;        // BossArrow의 갯수
    [HideInInspector] public int i_bossArrowSpriteNum;     // BossArrow의 모양 갯수
    [HideInInspector] public int i_bossArrowTextNum;       // BossArrow의 숫자 텍스트
    #endregion

    [HideInInspector] public IntReactiveProperty time = new IntReactiveProperty();      // 시간
    [HideInInspector] public IntReactiveProperty score = new IntReactiveProperty();     // 점수
    [HideInInspector] public IntReactiveProperty hp = new IntReactiveProperty();        // 체력
    private int maxHP = 1000;

    [HideInInspector] public bool isClearGame;

    private void Awake()
    {
        Lobby.SetActive(true);
        Game.SetActive(false);

        time
            .SubscribeToText(timeText, x => x.ToString());

        score
            .Subscribe(score =>
            {
                SetScore(score);
            });

        hp
            .Subscribe(hp =>
            {
                if (hp > 0)
                {
                    SetHPbar(hp);
                    return;
                }
                SetHPbar(0);
                StartCoroutine(GameLose());
            });

        time
            .Where(x => x == 0)
            .Subscribe(x =>
            {
                StartCoroutine(EnemyManager.CreateBoss());
                StartCoroutine(WarningAnimation());
            });

        goToLobbyButton
            .OnClickAsObservable()
            .Subscribe(lobbySetting =>
            {
                resultWindow.SetActive(false);
                Game.SetActive(false);
                Lobby.SetActive(true);
                EnemyManager.isEndGame = false;
            });
    }

    public void StartGame()
    {
        gameAudioSource.clip = gameBackGroundSound;
        gameAudioSource.Play();
        isClearGame = false;
        hp.Value = maxHP;
        score.Value = 0;
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
        StartCoroutine(TimerCoroutine());
        StartCoroutine(EnemyManager.EnemyAmount());
    }

    public void StartDialog()
    {
        if (ChapterManager.ReturnStage().StageBase.haveDone == true)
        {
            Game.SetActive(true);
            StartGame();
        }
        else
        {
            Stage stage = ChapterManager.Chapters[ChapterManager.CurrentChapterNum].Stages[ChapterManager.CurrentStageNum];
            DialogManager.ScenarioBase.Value = stage.StageBase.scenario.ScenarioBase;
        }
    }

    private IEnumerator TimerCoroutine()
    {
        time.Value = 60;
        while (time.Value > 0)
        {
            time.Value--;
            yield return new WaitForSeconds(1);
        }
    }

    protected virtual void SetHPbar(int currentHP)
    {
        float HpBar_X_Scale = (float)currentHP / maxHP;
        HpBar_X_Scale = Mathf.Clamp(HpBar_X_Scale, 0, 1);
        health.transform.localScale = new Vector3(HpBar_X_Scale, 1f, 1f);
    }

    protected virtual void SetScore(int currentScore)
    {
        scoreText.text = currentScore.ToString();
    }

    private IEnumerator WarningAnimation()
    {
        warningImage.gameObject.SetActive(true);
        warningImage
            .DOFade(0, 1);
        yield return new WaitForSeconds(1f);
        warningImage.gameObject.SetActive(false);
        warningImage
            .DOFade(1, 1);
    }

    #region ResultWindow
    public IEnumerator GameWin()
    {
        EnemyManager.isEndGame = true;
        isClearGame = true;
        DOTween.KillAll();
        yield return new WaitForSeconds(0.1f);
        ShowResultWindow(winSprite);

        if (ChapterManager.Chapters[0].Stages[5].StageBase.isClear == true)
        {
            ChapterManager.ChapterSelectButtons[1].interactable = true;
        }
    }

    public IEnumerator GameLose()
    {
        EnemyManager.isEndGame = true;
        DOTween.KillAll();
        yield return new WaitForSeconds(0.1f);
        ShowResultWindow(loseSprite);
    }

    private void ShowResultWindow(Sprite resultSprite)
    {
        resultImage.sprite = resultSprite;
        int num = score.Value;
        resultScoreText.text = num.ToString();
        resultWindow.SetActive(true);
    }
    #endregion
}
