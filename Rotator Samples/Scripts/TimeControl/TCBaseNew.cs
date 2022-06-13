using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace TimeControl
{
    public abstract class TCBaseNew : MonoBehaviour, IDynamic
    {
        #region Props & Fields

        public TimeKeeper TimeKeeper { get; set; }

        // Events
        public event Action ForwardCalled;
        public event Action RewindCalled;
        public event Action ReachedEnd;
        public event Action ReachedBeginning;

        [ShowInInspector] public int BeginningIndex { get; protected set; }
        [ShowInInspector] public int CurrentIndex { get; protected set; }
        [ShowInInspector] public int EndIndex { get; protected set; }
        [ShowInInspector] public bool IsRewinding { get; protected set; }
        [ShowInInspector] public DynamicType DynamicType { get; set; }

        [SerializeField, Required] private DynamicType baseDynamicType;

        #endregion Props & Fields

        #region UnityCallbacks

        protected virtual void Awake()
        {
            TimeKeeper = FindObjectOfType<TimeKeeper>();
            SetDynamicType(baseDynamicType);
            EndIndex = -1;
        }

        public void RegisterObject() => TimeKeeper.RegisterObject(this);
        public void DeRegisterObject() => TimeKeeper.DeRegisterObject(this);

        #endregion UnityCallbacks

        #region Methods

        // Called in FixedUpdate by objects that implement TimeControl
        public void ProgressForwardsInHistory()
        {
            ReadFutureStack();

            if (CurrentIndex == EndIndex)
            {
                ReachedEnd?.Invoke();
            }
            // Reached the end of this object's recorded history. Relevant if you're reading from future history like for enemies hp diffs.
            // You need to know if they have future history information.

            CurrentIndex++;
        }
        public void ProgressBackwardsInHistory()
        {
            if (IsRewinding)
            {
                ReadPastStack();

                if (CurrentIndex == BeginningIndex)
                {
                    // Forward or despawn or whatever
                    ReachedBeginning?.Invoke();
                    SetRewinding(false);
                    return;
                }
                // Invoke hit beginning of history event

                CurrentIndex--;
            }
        }

        // Overrides, required
        public abstract void ReadFutureStack();
        public abstract void ReadPastStack();

        // Core Functionality
        public void SetRewinding(bool value)
        {
            IsRewinding = value;
        }
        public void SetDynamicType(DynamicType value)
        {
            DynamicType = value;
        }
        public void Rewind()
        {
            // InversionCalled
            // => Forward or Rewind depending on own state

            // only rewind ingame objects
            // Will always be called in LateUpdate only since GI can only be triggered there.
            CurrentIndex--;
            EndIndex = CurrentIndex;

            RewindCalled?.Invoke();
            SetRewinding(true);
        }
        public void Forward()
        {
            // Will always be called in LateUpdate only since GI can only be triggered there.
            if (CurrentIndex > 0)
                CurrentIndex++; // Post-crement correction

            ForwardCalled?.Invoke();
            SetRewinding(false);
        }
        public void ResetIndexes()
        {
            BeginningIndex = 0;
            EndIndex = -1;
            CurrentIndex = 0;
        }
        public void ResetEvents()
        {
            ForwardCalled = null;
            RewindCalled = null;
            ReachedEnd = null;
            ReachedBeginning = null;
        }
        public void FullResetBase()
        {
            ResetEvents();
            ResetIndexes();
            SetDynamicType(baseDynamicType);
            SetRewinding(false);
            DeRegisterObject();
        }

        #endregion Methods
    }
}