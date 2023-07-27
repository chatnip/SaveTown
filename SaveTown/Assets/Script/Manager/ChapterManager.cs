using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using NaughtyAttributes;

public class ChapterManager : MonoBehaviour
{
    [Header("*Component")]
    [SerializeField] GameManager GameManager;
    [SerializeField] CharacterManager CharacterManager;
    [SerializeField] DialogManager DialogManager;
    [SerializeField] EnemyManager EnemyManager;
    [SerializeField] GameSystem GameSystem;

    [Header("*ChapterObjects")]
    [SerializeField] GameObject ChapterListObject;
    [SerializeField] Button GameStartButton;
    [SerializeField] Button HomeButton;
    [SerializeField] GameObject ChapterView;
    [SerializeField] public List<Button> ChapterSelectButtons = new List<Button>();

    [Header("*ChapterObjects")]
    [SerializeField] GameObject StageListObject;
    [SerializeField] Button StageExitButton;
    [SerializeField] GameObject StageView;
    [SerializeField] List<Button> StageSelectButtons = new List<Button>();

    [Header("*ChpaterSetting")]
    [SerializeField] public List<Chapter> Chapters;

    [HideInInspector] public int CurrentChapterNum;
    [HideInInspector] public int CurrentStageNum;

    public Chapter CurrentChapter { get { return Chapters[CurrentChapterNum]; } }
    public Stage CurrentStage { get { return CurrentChapter.Stages[CurrentStageNum]; } }

    private void Awake()
    {
        GameStartButton
            .OnClickAsObservable()
            .Subscribe(x =>
            {
                ChapterView.SetActive(true);
            });

        HomeButton
            .OnClickAsObservable()
            .Subscribe(x =>
            {
                ChapterView.SetActive(false);
            });

        StageExitButton
            .OnClickAsObservable()
            .Subscribe(x =>
            {
                StageView.SetActive(false);
            });


        foreach (Button Chapter in ChapterSelectButtons)
        {
            Chapter
                .OnClickAsObservable()
                .Select(ChapterNum => Chapter.transform.GetSiblingIndex())
                .Subscribe(ChapterNum =>
                {
                    this.CurrentChapterNum = ChapterNum;
                    foreach (Button Stage in StageSelectButtons)
                    {
                        if (Stage != StageSelectButtons[0])
                        {
                            Stage.interactable = false;
                        }
                    }
                    ChapterSetting();
                    ChapterView.SetActive(false);
                    StageView.SetActive(true);
                });

        }

        foreach (Button Stage in StageSelectButtons)
        {
            Stage
                .OnClickAsObservable()
                .Select(StageNum => Stage.transform.GetSiblingIndex())
                .Subscribe(StageNum =>
                {
                    this.CurrentStageNum = StageNum;
                    StageSetting();
                    StageView.SetActive(false);
                    GameSystem.Lobby.SetActive(false);
                });
        }

        GameSystem
            .ObserveEveryValueChanged(x => x.isClearGame)
            .Where(x => x == true)
            .Subscribe(_ =>
            {
                CurrentStage.StageBase.isClear = true;
            });
    }

    private void ChapterSetting()
    {
        StageView.SetActive(true);
        Chapter chapter = Chapters[CurrentChapterNum];
        GameSystem.gameBackGroundImage.sprite = chapter.BackGroundImage;
        GameSystem.gameBackGroundSound = chapter.BackGroundSound;

        for (int i = 0; i < CurrentChapter.Stages.Count; i++)
        {
            if (CurrentChapter.Stages[i].StageBase.isClear == true)
            {
                StageSelectButtons[i + 1].interactable = true;
            }
        }
    }

    public Stage ReturnStage()
    {
        Stage stage = Chapters[CurrentChapterNum].Stages[CurrentStageNum];
        return stage;
    }

    private void StageSetting()
    {
        Stage stage = ReturnStage();

        GameSystem.s_enemySprite = stage.StageBase.EnemySet.enemySprite;
        GameSystem.i_enemyAmount = stage.StageBase.EnemySet.enemyAmount;
        GameSystem.i_enemyTimeInterval = stage.StageBase.EnemySet.enemyTimeInterval;
        GameSystem.i_enemySpeed = stage.StageBase.EnemySet.enemySpeed - CharacterManager.SpeedDecreaseSkill;
        GameSystem.i_arrowAmount = stage.StageBase.EnemySet.arrowAmount;
        GameSystem.i_arrowSpriteNum = stage.StageBase.EnemySet.arrowSpriteNum;
        GameSystem.i_arrowTextNum = stage.StageBase.EnemySet.arrowTextNum - CharacterManager.NumDecreaseSkill;
        GameSystem.i_enemyDamage = stage.StageBase.EnemySet.enemyDamage - CharacterManager.DamageDecreaseSkill;

        GameSystem.s_bossEnemySprite = stage.StageBase.BossSet.bossEnemySprite;
        GameSystem.i_bossSpeed = stage.StageBase.BossSet.bossSpeed;
        GameSystem.i_bossArrowAmount = stage.StageBase.BossSet.BossArrowAmount;
        GameSystem.i_bossArrowSpriteNum = stage.StageBase.BossSet.bossArrowSpriteNum;
        GameSystem.i_bossArrowTextNum = stage.StageBase.BossSet.bossArrowTextNum;

        if(stage.StageBase.haveScenario == true)
        {
            GameSystem.StartDialog();          
        }
        else
        {
            GameSystem.Game.SetActive(true);
            GameSystem.StartGame();
        }
        stage.StageBase.haveDone = true;
    }

    public void firstChapterSetting()
    {
        ChapterSelectButtons[0].interactable = true;
        ChapterSelectButtons[1].interactable = false;
        ChapterSelectButtons[2].interactable = false;

        foreach(Stage chapter1 in Chapters[0].Stages)
        {
            chapter1.StageBase.isClear = false;
            chapter1.StageBase.haveDone = false;
        }
        foreach (Stage chapter2 in Chapters[1].Stages)
        {
            chapter2.StageBase.isClear = false;
            chapter2.StageBase.haveDone = false;
        }
        foreach (Stage chapter3 in Chapters[2].Stages)
        {
            chapter3.StageBase.isClear = false;
            chapter3.StageBase.haveDone = false;
        }
    }
}

[System.Serializable]
public struct Chapter
{
    [SerializeField] public Sprite BackGroundImage;
    [SerializeField] public AudioClip BackGroundSound;
    [SerializeField] public List<Stage> Stages;
}

[System.Serializable]
public struct Stage
{
    [SerializeField] public StageBase StageBase;
}