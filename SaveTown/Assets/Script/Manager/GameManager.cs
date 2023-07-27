using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class GameManager : Manager<GameManager>
{

    [Header("*Components")]
    [SerializeField] CharacterManager CharacterManager;
    [SerializeField] ChapterManager ChapterManager;

    [Header("*Particle")]
    [SerializeField] ParticleSystem particle;
    [SerializeField] Transform particlePos;

    public void Awake()
    {
        var clickStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(0));

        clickStream
            .Subscribe(_ =>
            {
                Vector3 mPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
                mPosition.z = 0;
                particlePos.position = mPosition;
                particle.Play();
            });
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void DevelopMode()
    {
        foreach (Button button in ChapterManager.ChapterSelectButtons)
        {
            button.interactable = true;
        }

        foreach (Stage chapter1 in ChapterManager.Chapters[0].Stages)
        {
            chapter1.StageBase.isClear = true;
            chapter1.StageBase.haveDone = true;
        }
        foreach (Stage chapter2 in ChapterManager.Chapters[1].Stages)
        {
            chapter2.StageBase.isClear = true;
            chapter2.StageBase.haveDone = true;
        }
        foreach (Stage chapter3 in ChapterManager.Chapters[2].Stages)
        {
            chapter3.StageBase.isClear = true;
            chapter3.StageBase.haveDone = true;
        }
    }

    public void ResetMode()
    {
        ChapterManager.firstChapterSetting();
        CharacterManager.firstCharacterSetting();
        ChapterManager.ChapterSelectButtons[0].interactable = true;
        ChapterManager.ChapterSelectButtons[1].interactable = false;
        ChapterManager.ChapterSelectButtons[2].interactable = false;
    }
}
