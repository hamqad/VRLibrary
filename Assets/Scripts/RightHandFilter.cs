using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightHandFilter : MonoBehaviour, IGameObjectFilter
{
    [SerializeField] private string rightHandTag = "RightHand";

    public bool Filter(GameObject gameObject)
    {
        return gameObject.CompareTag(rightHandTag);
    }

}
