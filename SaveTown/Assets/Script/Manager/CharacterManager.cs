using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class CharacterManager : MonoBehaviour
{
    [Header("*Components")]
    [SerializeField] ChapterManager ChapterManager;

    [Header("*Character")]
    [SerializeField] Button CharacterButton;
    [SerializeField] List<Button> CharacterButtons = new List<Button>();
    [SerializeField] Text SkillText;

    [HideInInspector] public int CurrentCharacterNum;
    [HideInInspector] public int SpeedDecreaseSkill = 0;
    [HideInInspector] public int NumDecreaseSkill = 0;
    [HideInInspector] public int DamageDecreaseSkill = 0;

    private void Awake()
    {
        CharacterButton
            .OnClickAsObservable()
            .Subscribe(_ =>
            {
                // 가온 해금 조건
                if (ChapterManager.Chapters[1].Stages[0].StageBase.isClear == true)
                {
                    CharacterButtons[1].interactable = true;
                }
                else
                {
                    CharacterButtons[1].interactable = false;
                }
                // 하나 해금 조건
                if (ChapterManager.Chapters[2].Stages[0].StageBase.isClear == true)
                {
                    CharacterButtons[2].interactable = true;
                }
                else
                {
                    CharacterButtons[2].interactable = false;
                }
            });

        foreach (Button Character in CharacterButtons)
        {
            Character
                .OnClickAsObservable()
                .Select(CharacterNum => Character.transform.GetSiblingIndex())
                .Subscribe(CharacterNum =>
                {
                    if (Character != CharacterButtons[CharacterNum])
                    {
                        Character.image.color = Color.gray;
                    }
                    CurrentCharacterNum = CharacterNum;
                    CharacterSetting();
                });
        }
        CharacterSetting();
    }

    public void CharacterSetting()
    {
        switch (CurrentCharacterNum)
        {
            case 0:
                SpeedDecreaseSkill = 0;
                NumDecreaseSkill = 0;
                DamageDecreaseSkill = 0;
                SkillText.text = "낡은 새총 | 평범하다.";
                break;
            case 1:
                SpeedDecreaseSkill = 2;
                NumDecreaseSkill = 0;
                DamageDecreaseSkill = 0;
                SkillText.text = "사랑의 매 | 적의 속도가 줄어든다.";
                break;
            case 2:
                SpeedDecreaseSkill = 0;
                NumDecreaseSkill = 0;
                DamageDecreaseSkill = 100;
                SkillText.text = "해초 시약 | 적의 공격력이 줄어든다.";
                break;
            default:
                break;
        }
    }

    public void firstCharacterSetting()
    {
        CurrentCharacterNum = 0;
        foreach (Button CharacterButton in CharacterButtons)
        {
            CharacterButton.interactable = false;
        }
        CharacterButtons[0].interactable = true;
    }
}