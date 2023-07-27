using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class Arrow : MonoBehaviour
{
    [Header("*Component")]
    [SerializeField] GameManager GameManager;
    [SerializeField] KeyPadManager KeyPadManager;
    [SerializeField] EnemyManager EnemyManager;
    [SerializeField] ObjectPooling ObjectPooling;

    [Header("*Arrow")]
    [SerializeField] Arrow arrow;
    [SerializeField] public Image arrowRenderer;
    [SerializeField] public Text numText;
    [Space(10)]
    [SerializeField] GameObject arrowPool;

    [HideInInspector] public Key key;
    [HideInInspector] public StringReactiveProperty numString = new StringReactiveProperty();
    [HideInInspector] public IntReactiveProperty numInt = new IntReactiveProperty();
    [HideInInspector] public BoolReactiveProperty isArrowPick = new BoolReactiveProperty();

    private void Awake()
    {
        numInt // Arrow ������ ���� �� ������Ʈ
            .SubscribeToText(numText, x => string.Format(x.ToString()));

        numInt // Arrow ��Ȱ��ȭ => 1. Arrow ������ ���ڰ� 0�� ���
            .Where(x => x == 0)
            .Subscribe(_ =>
            {
                ArrowPick(arrow);
            });
    }

    public void NumDecrease() // �ؽ�Ʈ ���� ���̱�
    {
        numInt.Value--;
    }

    private void ArrowPick(Arrow arrow) // Arrow�� Ǯ�� �ֱ�
    {
        arrow.transform.SetParent(arrowPool.transform);
        ObjectPooling.ArrowObjectPick(arrow);
        EnemyManager.EnemyQueue.Peek().ArrowQueue.Dequeue();
    }

    public void SpriteEnum(int arrowNum) // Arrow�� Enum ����
    {
        switch (arrowNum)
        {
            case 0:
                key = Key.Up;
                break;
            case 1:
                key = Key.Down;
                break;
            case 2:
                key = Key.Left;
                break;
            case 3:
                key = Key.Right;
                break;
            case 4:
                key = Key.UpLeft;
                break;
            case 5:
                key = Key.UpRight;
                break;
            case 6:
                key = Key.DownLeft;
                break;
            case 7:
                key = Key.DownRight;
                break;
            default:
                break;
        }
    }
}
