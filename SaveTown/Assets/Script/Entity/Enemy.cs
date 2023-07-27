using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;

public class Enemy : MonoBehaviour
{
    [Header("*Component")]
    [SerializeField] GameManager GameManager;
    [SerializeField] KeyPadManager KeyPadManager;
    [SerializeField] EnemyManager EnemyManager;
    [SerializeField] ObjectPooling ObjectPooling;
    [SerializeField] GameSystem GameSystem;

    [Header("*Enemy")]
    [SerializeField] Enemy enemy;
    [SerializeField] GameObject arrowParent;
    [SerializeField] GameObject arrowPool;
    [SerializeField] RectTransform enemyTransform;
    [SerializeField] public Image enemyImage;

    [HideInInspector] public Queue<Arrow> ArrowQueue = new Queue<Arrow>();
    [HideInInspector] public Type type;
    [HideInInspector] public Sequence sequence;
    private List<int> RandomList = new List<int>();

    private void Awake()
    {
        enemy
            .ObserveEveryValueChanged(x => x.gameObject.activeSelf)
            .Where(x => x == true)
            .Subscribe(_ =>
            {
                sequence = DOTween.Sequence();
                if(enemy.type == Type.Nomal)
                {
                    CreateArrow();
                    sequence.Append(enemy.transform.DOMoveY(-2, GameSystem.i_enemySpeed).SetEase(Ease.Linear));
                }
                else
                {
                    CreateBossArrow();
                    sequence.Append(enemy.transform.DOMoveY(-2, GameSystem.i_bossSpeed).SetEase(Ease.Linear));
                }
            });

        arrowParent
            .ObserveEveryValueChanged(x => x.transform.childCount) // Enemy ��Ȱ��ȭ => 1. Enemy ������ Arrow�� 0�� ���
            .Where(x => x == 0)
            .Subscribe(_ =>
            {
                if (gameObject.activeSelf == true)
                {
                    if (enemyTransform.anchoredPosition.y > 95)
                    {
                        GameSystem.score.Value += 100;
                    }
                    if (enemyTransform.anchoredPosition.y > 95 && enemy.type == Type.Boss)
                    {
                        StartCoroutine(GameSystem.GameWin());
                        StartCoroutine(BossPick());
                    }
                    else
                    {
                        ObjectPooling.EnmeyObjectPick(enemy);
                        sequence.Kill();
                        EnemyManager.EnemyQueue.Dequeue();
                    }
                }
            });

        EnemyManager
            .ObserveEveryValueChanged(x => x.isEndGame)
            .Where(x => x == true)
            .Delay(System.TimeSpan.FromMilliseconds(100))
            .Subscribe(_ =>
            {
                Debug.Log("pick");
                StartCoroutine(EnemyPick());
            });
    }

    private void OnTriggerEnter2D(Collider2D other) // Enemy�� Wall�� ��Ҵ��� Ȯ��
    {
        if (other.CompareTag("Wall"))
        {
            if (enemy.type == Type.Boss)
            {
                StartCoroutine(GameSystem.GameLose());
            }
            else
            {
                sequence.Kill();
                GameSystem.hp.Value -= GameSystem.i_enemyDamage;
                StartCoroutine(EnemyPick());
            }
        }
    }

    public IEnumerator EnemyPick() // Enemy ��Ȱ��ȭ => 2. Enemy�� ���� �ε��� ���
    {
        ChildPick();
        yield return new WaitUntil(() =>
        {
            if (arrowParent.transform.childCount == 0)
            {
                return true;
            }
            return false;
        });
        ObjectPooling.EnmeyObjectPick(enemy);
        sequence.Kill();
        EnemyManager.EnemyQueue.Dequeue();
    }

    public IEnumerator BossPick()
    {
        yield return new WaitForSeconds(0.5f);
        ObjectPooling.EnmeyObjectPick(enemy);
        sequence.Kill();
        EnemyManager.EnemyQueue.Dequeue();
    }


    public void ChildPick() // Arrow ��Ȱ��ȭ => 2. Enemy�� ���� �ε��� ���
    {
        // ArrowQueue.Count�� ������ 3�� �̻� �������� ���ϴ� ���װ� ����.
        for (int i = 0; i < 8/*ArrowQueue.Count*/; i++)
        {
            ArrowQueue.Peek().transform.SetParent(arrowPool.transform);
            ObjectPooling.ArrowObjectPick(ArrowQueue.Peek());
            ArrowQueue.Dequeue();
        }
    }

    private void CreateArrow() // Arrow Ȱ��ȭ
    {
        for (int i = 0; i < GameSystem.i_arrowAmount; i++)
        {
            Arrow arrow = ObjectPooling.ArrowObjectPool();
            ArrowQueue.Enqueue(arrow);
            ArrowSetting(arrow);
            arrow.gameObject.SetActive(true);
        }
    }

    private void CreateBossArrow() // BossArrow Ȱ��ȭ
    {
        for (int i = 0; i < GameSystem.i_bossArrowAmount; i++)
        {
            Arrow arrow = ObjectPooling.ArrowObjectPool();
            ArrowQueue.Enqueue(arrow);
            BossArrowSetting(arrow);
            arrow.gameObject.SetActive(true);
        }
    }

    #region ArrowSetting
    private void ArrowSetting(Arrow arrow) // Arrow ����
    {
        arrow.transform.SetParent(arrowParent.transform);
        ArrowSprite(arrow);
        ArrowText(arrow);
    }

    private void ArrowSprite(Arrow arrow) // Arrow�� ��������Ʈ ����
    {
        int randomArrow = Random.Range(0, GameSystem.i_arrowSpriteNum);

        // �ߺ� ����      
        RandomList.Add(randomArrow);
        if (arrow != ArrowQueue.Peek())
        {
            while (randomArrow == RandomList[0])
            {
                randomArrow = Random.Range(0, GameSystem.i_arrowSpriteNum);
                RandomList.RemoveAt(1);
                RandomList.Add(randomArrow);

                if (randomArrow != RandomList[0])
                {
                    break;
                }
            }
            RandomList.RemoveAt(0);
        }   
        arrow.arrowRenderer.sprite = EnemyManager.arrowSprite[randomArrow];
        arrow.SpriteEnum(randomArrow);
    }

    private int ArrowText(Arrow arrow) // Arrow�� �ؽ�Ʈ ����
    {
        int randomNum = Random.Range(1, GameSystem.i_arrowTextNum);
        arrow.numInt.Value = randomNum;
        arrow.numText.text = arrow.numInt.Value.ToString();
        return randomNum;
    }
    #endregion

    #region BossArrowSetting
    private void BossArrowSetting(Arrow arrow) // BossArrow ����
    {
        arrow.transform.SetParent(arrowParent.transform);
        BossArrowSprite(arrow);
        BossArrowText(arrow);
    }

    private void BossArrowSprite(Arrow arrow) // BossArrow�� ��������Ʈ ����
    {
        int randomArrow = Random.Range(0, GameSystem.i_bossArrowSpriteNum);

        // �ߺ� ����    
        RandomList.Add(randomArrow);
        if (arrow != ArrowQueue.Peek())
        {
            while (randomArrow == RandomList[0])
            {
                randomArrow = Random.Range(0, GameSystem.i_bossArrowSpriteNum);
                RandomList.RemoveAt(1);
                RandomList.Add(randomArrow);

                if (randomArrow != RandomList[0])
                {
                    break;
                }
            }
            RandomList.RemoveAt(0);
        }
        arrow.arrowRenderer.sprite = EnemyManager.arrowSprite[randomArrow];
        arrow.SpriteEnum(randomArrow);
    }

    private int BossArrowText(Arrow arrow) // BossArrow�� �ؽ�Ʈ ����
    {
        int randomNum = Random.Range(1, GameSystem.i_bossArrowTextNum);
        arrow.numInt.Value = randomNum;
        arrow.numText.text = arrow.numInt.Value.ToString();
        return randomNum;
    }
    #endregion
}