using com.guinealion.animatedBook;
using Oculus.Interaction;
using UnityEngine;
using System;

namespace Oculus.Interaction.HandGrab
{
    public class PageOpener : MonoBehaviour,ITransformer
    {
        public Grabbable grabbable;
        public LightweightBookHelper bookHelper;
        public bool isRight = true;
        private HandGrabInteractable _grabInteractable;
        private Vector3 _startGrabPosition, hand1Position;
        private float handsDistance, openAmmount, currentProgress;
        private AudioSource[] audioSources;
        private AudioSource startFlip, endFlip;


        void Start()
        {
            grabbable = GetComponent<Grabbable>();
            bookHelper = GetComponentInParent<LightweightBookHelper>();
            audioSources = GetComponentsInParent<AudioSource>();
            startFlip = audioSources[1];
            endFlip = audioSources[2];
        }
        public void Initialize(IGrabbable grabbable)
        {
            _grabInteractable = grabbable.Transform.GetComponent<HandGrabInteractable>();

        }
        public void BeginTransform()
        {
            _startGrabPosition = grabbable.GrabPoints[0].position;
            if (!((isRight && (bookHelper.Progress == bookHelper.PageAmmount - 1)) || (!isRight && (bookHelper.Progress == 0))))
            {
                startFlip.Play();
            }
        }

        public void UpdateTransform()
        {
            hand1Position = grabbable.GrabPoints[0].position;
            if (isRight)
            {
                handsDistance = (float)((_startGrabPosition.x - hand1Position.x) * 2);
            }
            else
            {
                handsDistance = (float)((hand1Position.x - _startGrabPosition.x) * 2);
            }
            openAmmount = Mathf.Clamp(handsDistance / 0.5f, 0f, 1f);
            if (isRight)
            {
                currentProgress = Mathf.FloorToInt(bookHelper.Progress);
                bookHelper.Progress = currentProgress + openAmmount;

            }
            else
            {
                currentProgress = Mathf.CeilToInt(bookHelper.Progress);
                bookHelper.Progress = currentProgress - openAmmount;

            }



        }

        public void EndTransform()
        {
            float decimalProgress = bookHelper.Progress - Mathf.FloorToInt(bookHelper.Progress);
            if (!((isRight && (bookHelper.Progress == bookHelper.PageAmmount)) || (!isRight && (bookHelper.Progress == 0))))
            {
                endFlip.Play();
            }
            if (decimalProgress >= 0.5)
            {
                bookHelper.NextPage(0.5f);
            }
            else if (decimalProgress < 0.5)
            {
                bookHelper.GoToPage(Mathf.FloorToInt(bookHelper.Progress), true, 0.5f);
            }

        }
    }
}
