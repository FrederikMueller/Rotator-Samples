using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace TimeControl
{
    public class TCEnemy : TCBase, IHasHealth
    {
        #region Props & Fields

        private IHasHealth trackedObj;
        private int healthLastFrame;

        private LeanMemento[] history;
        private LeanMemento currentSnapshot;
        private MinMemento[] minMementos;
        private short mmIdx;
        private Vector3 currentPos;
        private Vector3 currentRot;

        [SerializeField] private bool doubleHistory;

        // Fields to track get => trackedClass.X
        public int Health { get => trackedObj.Health; set => trackedObj.Health = value; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }

        #endregion Props & Fields

        protected override void Awake()
        {
            base.Awake();
            trackedObj = GetComponent<DefComponent>();
            currentPos = new Vector3(0, 0, 0);
            currentRot = new Vector3(0, 0, 0);
        }

        public override void RegisterObject()
        {
            TimeKeeper.RegisterObject(this);
        }
        public void ResetState()
        {
            healthLastFrame = Health;
            ResetIndexes();
        }

        #region Methods

        // Forwards
        public override void WriteHistory()
        {
            // Reading the Diff of the current snapshot / frame. Isnt this wrong? We're reading these values before we're writing the diff, which means that
            // the hp gain here will be reverted again

            int temp = history[CurrentIndex].hpDiff;

            //int temp = 0;
            //if (CurrentIndex == minMementos[mmIdx].frame)
            //{
            //    temp = minMementos[mmIdx].diff;
            //    minMementos[mmIdx].diff = Health - healthLastFrame;
            //    mmIdx++;
            //    Debug.Log("Used MinMemento!");
            //}
            //if (Health - healthLastFrame != 0)
            //    minMementos[idx] = new MinMemento(CurrentIndex, Health - healthLastFrame);

            history[CurrentIndex].hpDiff = Health - healthLastFrame;
            history[CurrentIndex].xPos = transform.position.x;
            history[CurrentIndex].yPos = transform.position.y;
            history[CurrentIndex].zRot = transform.rotation.eulerAngles.z;

            if (doubleHistory)
                ReadFutureHistoryDiffs(temp);

            healthLastFrame = Health;
        }
        private void ReadFutureHistoryDiffs(int temp)
        {
            if (CurrentIndex <= EndIndex)
                Health -= temp;

            //if (CurrentIndex == idx)
            //    Health -= minMementos[idx].diff;
        }
        // Backwards
        public override void ReadHistory()
        {
            if (IsRewinding)
            {
                currentSnapshot = history[CurrentIndex];

                if (doubleHistory)
                    ReadAndWriteHistory();
                else
                    OnlyReadHistory();
            }
        }
        private void OnlyReadHistory()
        {
            currentSnapshot = history[CurrentIndex];
            currentPos.x = currentSnapshot.xPos;
            currentPos.y = currentSnapshot.yPos;
            currentRot.z = currentSnapshot.zRot;

            gameObject.transform.position = currentPos;
            gameObject.transform.localEulerAngles = currentRot;
            Health -= currentSnapshot.hpDiff;
        }
        private void ReadAndWriteHistory()
        {
            // Saving hp changes caused by blue / inverted player
            int temp = currentSnapshot.hpDiff;
            //int temp = 0;
            //if (CurrentIndex == minMementos[mmIdx].frame)
            //{
            //    temp = minMementos[mmIdx].diff;
            //    minMementos[mmIdx].diff = Health - healthLastFrame;
            //    mmIdx--;
            //    Debug.Log("Used MinMemento!");
            //}

            history[CurrentIndex].hpDiff = Health - healthLastFrame;

            currentPos.x = currentSnapshot.xPos;
            currentPos.y = currentSnapshot.yPos;
            currentRot.z = currentSnapshot.zRot;
            gameObject.transform.position = currentPos;
            gameObject.transform.localEulerAngles = currentRot;

            Health -= temp;

            healthLastFrame = Health;
        }

        protected override void CreateHistory()
        {
            history = new LeanMemento[3000];
            for (int i = 0; i < 3000; i++)
            {
                history[i] = new LeanMemento(0, 0, 0, 0);
            }
            minMementos = new MinMemento[100];
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