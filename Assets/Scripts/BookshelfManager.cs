using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction.HandGrab;
using UnityEngine;

public class BookshelfManager : MonoBehaviour
{
    private HandGrabInteractable handGrab;
    public GameObject padlockTop;

    // Start is called before the first frame update
    void Start()
    {
        handGrab = GetComponent<HandGrabInteractable>();
    }

    public void ToggleLock()
    {
        if (handGrab != null)
        {
            if (handGrab.enabled)
            {
                handGrab.enabled = false;
                Vector3 currentPos = padlockTop.transform.localPosition;
                currentPos.y = currentPos.y - 0.1f;
                padlockTop.transform.localPosition = currentPos;
            }
            else
            {
                handGrab.enabled = true;
                Vector3 currentPos = padlockTop.transform.localPosition;
                currentPos.y = currentPos.y + 0.1f;
                padlockTop.transform.localPosition = currentPos;

            }
        }
    }
}
