using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using System;

public class KeyPadManager : MonoBehaviour
{
    [Header("*Component")]
    [SerializeField] GameManager GameManager;
    [SerializeField] EnemyManager EnemyManager;
    [SerializeField] ObjectPooling ObjectPooling;

    [Header("*Pool")]
    [SerializeField] GameObject enemyPool;
    [SerializeField] GameObject arrowPool;

    [Header("*Key")]
    [SerializeField] Button UpBtn;
    [SerializeField] Button DownBtn;
    [SerializeField] Button LeftBtn;
    [SerializeField] Button RightBtn;

    [Space(10)]
    [SerializeField] Button UpLeftBtn;
    [SerializeField] Button UpRightBtn;
    [SerializeField] Button DownLeftBtn;
    [SerializeField] Button DownRightBtn;

    [Header("*Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip clickSound;

    private void Awake() // 누른 버튼과 스프라이트가 일치하는지 확인
    {
        #region BasicKey
        UpBtn
            .OnClickAsObservable()
            .Subscribe(_ =>
            {
                audioSource.PlayOneShot(clickSound);
                if (EnemyManager.EnemyQueue.Peek().ArrowQueue.Peek().key == Key.Up)
                {
                    EnemyManager.EnemyQueue.Peek().ArrowQueue.Peek().NumDecrease();
                }
            });

        DownBtn
            .OnClickAsObservable()
            .Subscribe(_ =>
            {
                audioSource.PlayOneShot(clickSound);
                if (EnemyManager.EnemyQueue.Peek().ArrowQueue.Peek().key == Key.Down)
                {
                    EnemyManager.EnemyQueue.Peek().ArrowQueue.Peek().NumDecrease();
                }
            });

        LeftBtn
            .OnClickAsObservable()
            .Subscribe(_ =>
            {
                audioSource.PlayOneShot(clickSound);
                if (EnemyManager.EnemyQueue.Peek().ArrowQueue.Peek().key == Key.Left)
                {
                    EnemyManager.EnemyQueue.Peek().ArrowQueue.Peek().NumDecrease();
                }
            });

        RightBtn
            .OnClickAsObservable()
            .Subscribe(_ =>
            {
                audioSource.PlayOneShot(clickSound);
                if (EnemyManager.EnemyQueue.Peek().ArrowQueue.Peek().key == Key.Right)
                {
                    EnemyManager.EnemyQueue.Peek().ArrowQueue.Peek().NumDecrease();
                }
            });
        #endregion

        #region AddKey
        UpLeftBtn
            .OnClickAsObservable()
            .Subscribe(_ =>
            {
                audioSource.PlayOneShot(clickSound);
                if (EnemyManager.EnemyQueue.Peek().ArrowQueue.Peek().key == Key.UpLeft)
                {
                    EnemyManager.EnemyQueue.Peek().ArrowQueue.Peek().NumDecrease();
                }
            });

        UpRightBtn
            .OnClickAsObservable()
            .Subscribe(_ =>
            {
                audioSource.PlayOneShot(clickSound);
                if (EnemyManager.EnemyQueue.Peek().ArrowQueue.Peek().key == Key.UpRight)
                {
                    EnemyManager.EnemyQueue.Peek().ArrowQueue.Peek().NumDecrease();
                }
            });

        DownLeftBtn
            .OnClickAsObservable()
            .Subscribe(_ =>
            {
                audioSource.PlayOneShot(clickSound);
                if (EnemyManager.EnemyQueue.Peek().ArrowQueue.Peek().key == Key.DownLeft)
                {
                    EnemyManager.EnemyQueue.Peek().ArrowQueue.Peek().NumDecrease();
                }
            });

        DownRightBtn
            .OnClickAsObservable()
            .Subscribe(_ =>
            {
                audioSource.PlayOneShot(clickSound);
                if (EnemyManager.EnemyQueue.Peek().ArrowQueue.Peek().key == Key.DownRight)
                {
                    EnemyManager.EnemyQueue.Peek().ArrowQueue.Peek().NumDecrease();
                }
            });
        #endregion
    }
}

[System.Serializable]
public enum Key
{
    Up,
    Down,
    Left,
    Right,
    UpLeft,
    UpRight,
    DownLeft,
    DownRight
}