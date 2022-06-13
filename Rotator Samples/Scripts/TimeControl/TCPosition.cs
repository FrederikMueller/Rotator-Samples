using System;
using UnityEngine;

namespace TimeControl
{
    public class TCPosition : TCBase
    {
        #region Props & Fields

        // Only Position tracking
        private PositionMemento[] history;
        private PositionMemento currentSnapshot;
        private Vector3 currentPos;
        public Vector3 Position { get; set; }

        #endregion Props & Fields

        private void Start()
        {
            currentPos = new Vector3(-10, 10);
        }
        public override void RegisterObject()
        {
            TimeKeeper.RegisterObject(this);
        }

        #region Methods

        public override void WriteHistory()
        {
            history[CurrentIndex].x = transform.position.x;
            history[CurrentIndex].y = transform.position.y;
        }
        public override void ReadHistory()
        {
            currentSnapshot = history[CurrentIndex];
            currentPos.x = currentSnapshot.x;
            currentPos.y = currentSnapshot.y;

            gameObject.transform.position = currentPos;
        }
        protected override void CreateHistory()
        {
            history = new PositionMemento[3000];
            for (int i = 0; i < 3000; i++)
            {
                history[i] = new PositionMemento(0, 0);
            }
        }
        protected override void CheckForArrayResize(int amtToAdd)
        {
            if (CurrentIndex > history.Length - 1) // to avoid out of bounds because nowIndex gets increased afterwards.
            {
                Array.Resize(ref history, history.Length + amtToAdd);
                for (int i = 0; i < amtToAdd; i++)
                {
                    history[CurrentIndex + i] = new PositionMemento(0, 0);
                }
            }
        }

        #endregion Methods
    }
}