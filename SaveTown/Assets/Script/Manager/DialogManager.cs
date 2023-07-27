using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;

[System.Serializable]
public struct DialogData
{
    public Text nameText;
    public Text dialogText;
    public Image characterImage;
}

public class DialogManager : MonoBehaviour
{
    [Header("*Component")]
    [SerializeField] GameManager GameManager;
    [SerializeField] ChapterManager ChapterManager;
    [SerializeField] GameSystem GameSystem;

    [Header("*Dialog")]
    [SerializeField] public GameObject dialog;
    [SerializeField] DialogData DialogData;
    [SerializeField] public ReactiveProperty<ScenarioBase> ScenarioBase = new ReactiveProperty<ScenarioBase>();

    private void Awake()
    {
        ScenarioBase
            .Where(Base => Base != null)
            .Subscribe(texting =>
            {
                if(ChapterManager.ReturnStage().StageBase.haveDone == true)
                {
                    GameSystem.Game.SetActive(true);
                    GameSystem.StartGame();
                }
                else
                {
                    StartCoroutine(DialogTexting(texting));
                }
            });

        ScenarioBase
            .Where(Base => Base == null)
            .Subscribe(_ =>
            {
                GameSystem.StartGame();
            });
    }

    private void DialogSetup(Fragment Scenario_Fragment)
    {
        GameSystem.Game.SetActive(true);
        dialog.SetActive(true);
        DialogData.nameText.text = Scenario_Fragment.Speaker;
        DialogData.characterImage.sprite = Scenario_Fragment.CharacterSprite;
    }

    public IEnumerator DialogTexting(ScenarioBase scenarioBase)
    {
        DialogData.dialogText.text = null;
        DialogSetup(scenarioBase.Fragments[0]);

        for (int i = 0; i < scenarioBase.Fragments.Count; i++)
        {
            int temp = i;
            var sequence = DOTween.Sequence();
            DialogData.dialogText.text = null;
            DialogSetup(scenarioBase.Fragments[temp]);
            Fragment newFragment = scenarioBase.Fragments[temp];
            sequence.Append(DialogData.dialogText.DOText(newFragment.Script, newFragment.Script.Length / 10)
                    .SetEase(Ease.Linear)
                    .OnUpdate(() =>
                    {
                        if (Input.GetMouseButtonDown(0) /*모바일용 추가*/ )
                        {
                            sequence.Complete();
                        }
                    }));

            yield return new WaitUntil(() =>
            {
                if (DialogData.dialogText.text == newFragment.Script)
                {
                    return true;
                }
                return false;
            });

            yield return new WaitForSeconds(0.2f);

            yield return new WaitUntil(() =>
            {
                if (Input.GetMouseButtonDown(0) /*모바일용 추가*/ )
                {
                    return true;
                }
                return false;
            });
            continue;
        }
        dialog.SetActive(false);
        GameSystem.StartGame();
    }
}
