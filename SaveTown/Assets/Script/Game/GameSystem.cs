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
    [HideInInspector] public Sprite s_enemySprite;     // Enemy ��������Ʈ
    [HideInInspector] public int i_enemyAmount;        // Enemy�� ����
    [HideInInspector] public int i_enemyTimeInterval;  // Enemy�� ������ ����
    [HideInInspector] public int i_enemySpeed;         // Enemy�� �ӵ�
    [HideInInspector] public int i_arrowAmount;        // Arrow�� ����
    [HideInInspector] public int i_arrowSpriteNum;     // Arrow�� ��� ����
    [HideInInspector] public int i_arrowTextNum;       // Arrow�� ���� �ؽ�Ʈ
    [HideInInspector] public int i_enemyDamage;        // Enemy�� �ִ� ������

    [HideInInspector] public Sprite s_bossEnemySprite;     // Boss ��������Ʈ
    [HideInInspector] public int i_bossSpeed;              // Boss�� �ӵ�
    [HideInInspector] public int i_bossArrowAmount;        // BossArrow�� ����
    [HideInInspector] public int i_bossArrowSpriteNum;     // BossArrow�� ��� ����
    [HideInInspector] public int i_bossArrowTextNum;       // BossArrow�� ���� �ؽ�Ʈ
    #endregion

    [HideInInspector] public IntReactiveProperty time = new IntReactiveProperty();      // �ð�
    [HideInInspector] public IntReactiveProperty score = new IntReactiveProperty();     // ����
    [HideInInspector] public IntReactiveProperty hp = new IntReactiveProperty();        // ü��
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
