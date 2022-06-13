using DG.Tweening;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIHitbox : MonoBehaviour
    {
        [SerializeField] private GameObject UIElement;
        [SerializeField] private List<Vector3> transformPos = new List<Vector3>();
        [SerializeField] private List<GameObject> children = new List<GameObject>();
        [SerializeField] private bool isLeftSide;
        private int shipCount;
        private GameManager gameManager;

        private void Awake()
        {
            gameManager = FindObjectOfType<GameManager>();
            for (int i = 0; i < children.Count; i++)
            {
                transformPos.Add(children[i].transform.localPosition);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 8 && other.gameObject.GetComponent<PlayerCore>() == gameManager.CurrentlyActivePlayer)
            {
                UIElement.GetComponent<Image>().CrossFadeAlpha(0f, .3f, true);
                foreach (var element in children)
                {
                    if (isLeftSide)
                        element.transform.DOMoveX(-450, .2f);
                    else
                        element.transform.DOMoveX(1500, .2f);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == 8 && other.gameObject.GetComponent<PlayerCore>() == gameManager.CurrentlyActivePlayer)
            {
                shipCount--;
                if (shipCount <= 0)
                {
                    UIElement.GetComponent<Image>().CrossFadeAlpha(1f, .3f, true);
                    Observable
                        .Timer(TimeSpan.FromSeconds(.25f))
                        .Subscribe(_ => ResetPositions());
                }
            }
        }

        private void ResetPositions()
        {
            for (int i = 0; i < children.Count; i++)
            {
                children[i].transform.localPosition = transformPos[i];
            }
        }
    }
}