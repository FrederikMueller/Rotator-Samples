using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace TimeControl
{
    public class TCPlayer : TCBase
    {
        #region Props & Fields

        private IHasHealth trackedObj;
        private int healthLastFrame;

        private LeanMemento[] history;
        private LeanMemento currentSnapshot;
        private Vector3 currentPos;
        private Vector3 currentRot;

        // Fields to track get => trackedClass.X
        public int Health { get => trackedObj.Health; set => trackedObj.Health = value; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }

        #endregion Props & Fields

        protected override void Awake()
        {
            base.Awake();
            trackedObj = GetComponent<IHasHealth>();
            healthLastFrame = Health;

            ForwardCalled += SetNewBeginning;

            currentPos = new Vector3(0, 0, 0);
            currentRot = new Vector3(0, 0, 0);
            RegisterObject();
        }
        public override void RegisterObject()
        {
            TimeKeeper.RegisterObject(this);
        }

        #region Methods

        private void SetNewBeginning()
        {
            BeginningIndex = CurrentIndex;
        }
        public override void WriteHistory()
        {
            history[CurrentIndex].xPos = transform.position.x;
            history[CurrentIndex].yPos = transform.position.y;
            history[CurrentIndex].zRot = transform.rotation.eulerAngles.z;
            history[CurrentIndex].hpDiff = Health - healthLastFrame;

            healthLastFrame = Health;
        }
        public override void ReadHistory()
        {
            currentSnapshot = history[CurrentIndex];
            currentPos.x = currentSnapshot.xPos;
            currentPos.y = currentSnapshot.yPos;
            currentRot.z = currentSnapshot.zRot;

            gameObject.transform.position = currentPos;
            gameObject.transform.localEulerAngles = currentRot;
            Health -= currentSnapshot.hpDiff;
        }
        protected override void CreateHistory()
        {
            history = new LeanMemento[3000];
            for (int i = 0; i < 3000; i++)
            {
                history[i] = new LeanMemento(0, 0, 0, 0);
            }
        }
        protected override void CheckForArrayResize(int amtToAdd)
        {
            if (CurrentIndex > history.Length - 1) // to avoid out of bounds because nowIndex gets increased afterwards.
            {
                Array.Resize(ref history, history.Length + amtToAdd);
                for (int i = 0; i < amtToAdd; i++)
                {
                    history[CurrentIndex + i] = new LeanMemento(0, 0, 0, 0);
                }
            }
        }

        #endregion Methods
    }
}