using System;
using System.Collections;
using UnityEngine;

namespace Oculus.Interaction
{
    public class LockButton : MonoBehaviour
    {
        [SerializeField, Interface(typeof(IInteractableView))]
        private UnityEngine.Object _interactableView;
        private IInteractableView InteractableView { get; set; }
        public BookshelfManager bookshelfManager;
        protected bool _started = false;


        protected virtual void Awake()
        {
            InteractableView = _interactableView as IInteractableView;
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            this.BeginStart(ref _started);

            this.AssertField(InteractableView, nameof(InteractableView));
            UpdateLock();
            this.EndStart(ref _started);

        }
        protected virtual void OnEnable()
        {
            if (_started)
            {
                UpdateLock();
                InteractableView.WhenStateChanged += UpdateVisualState;
            }
        }

        private void UpdateVisualState(InteractableStateChangeArgs args)
        {
            UpdateLock();
        }

        protected virtual void UpdateLock()
        {
            if(InteractableView.State == InteractableState.Select)
            {
                bookshelfManager.ToggleLock();
            }
        }
    }

}
