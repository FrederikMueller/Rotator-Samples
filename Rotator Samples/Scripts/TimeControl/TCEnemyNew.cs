using System;
using System.Collections.Generic;
using UnityEngine;

namespace TimeControl
{
    public class TCEnemyNew : TCBaseNew
    {
        #region Props & Fields

        // State
        // Clear and keep
        [HideInInspector] public Stack<IntMementoNew> PastInts = new Stack<IntMementoNew>();
        [HideInInspector] public Stack<FloatMementoNew> PastFloats = new Stack<FloatMementoNew>();
        [HideInInspector] public Stack<ByteMementoNew> PastBytes = new Stack<ByteMementoNew>();

        [HideInInspector] public Stack<IntMementoNew> FutureInts = new Stack<IntMementoNew>();
        [HideInInspector] public Stack<FloatMementoNew> FutureFloats = new Stack<FloatMementoNew>();
        [HideInInspector] public Stack<ByteMementoNew> FutureBytes = new Stack<ByteMementoNew>();
        // Clear and keep
        public List<IntTracker> IntTrackers = new List<IntTracker>();
        public List<FloatTracker> FloatTrackers = new List<FloatTracker>();
        public List<ByteTracker> ByteTrackers = new List<ByteTracker>();

        // Reset to zero
        private byte intEventIndex;
        private byte floatEventIndex;
        private byte byteEventIndex;

        #endregion Props & Fields

        protected override void Awake()
        {
            base.Awake();
        }

        #region Methods

        public void FullReset()
        {
            FullResetBase();

            PastInts.Clear();
            PastFloats.Clear();
            PastBytes.Clear();
            FutureInts.Clear();
            FutureFloats.Clear();
            FutureBytes.Clear();

            foreach (var t in IntTrackers) t.ClearEvent();
            IntTrackers.Clear();

            foreach (var t in FloatTrackers) t.ClearEvent();
            FloatTrackers.Clear();

            foreach (var t in ByteTrackers) t.ClearEvent();
            ByteTrackers.Clear();

            intEventIndex = 0;
            floatEventIndex = 0;
            byteEventIndex = 0;
        }
        public IntTracker AddInt(ref Action<int> action)
        {
            byte b = intEventIndex;
            intEventIndex++;

            var tracker = new IntTracker(b, this);
            action += tracker.RecordInput;
            IntTrackers.Add(tracker);

            if (IntTrackers.IndexOf(tracker) != b)
                Debug.Log("index mismatch on tracker");

            return tracker;
        }
        public FloatTracker AddFloat(ref Action<float> action)
        {
            byte b = floatEventIndex;
            floatEventIndex++;

            var tracker = new FloatTracker(b, this);
            action += tracker.RecordInput;
            FloatTrackers.Add(tracker);

            if (FloatTrackers.IndexOf(tracker) != b)
                Debug.Log("index mismatch on tracker");

            return tracker;
        }
        public ByteTracker AddByte(ref Action<byte> action)
        {
            byte b = byteEventIndex;
            byteEventIndex++;

            var tracker = new ByteTracker(b, this);
            action += tracker.RecordInput;
            ByteTrackers.Add(tracker);

            if (ByteTrackers.IndexOf(tracker) != b)
                Debug.Log("index mismatch on tracker");

            return tracker;
        }
        public override void ReadFutureStack()
        {
            while (FutureInts.Count > 0 && FutureInts.Peek().FrameNum == CurrentIndex)
            {
                var m = FutureInts.Pop();
                IntTrackers[m.StackTypeIdx].SendOutput(m.Diff);
            }

            while (FutureFloats.Count > 0 && FutureFloats.Peek().FrameNum == CurrentIndex)
            {
                var m = FutureFloats.Pop();
                FloatTrackers[m.StackTypeIdx].SendOutput(m.Diff);
            }
            while (FutureBytes.Count > 0 && FutureBytes.Peek().FrameNum == CurrentIndex)
            {
                var m = FutureBytes.Pop();
                ByteTrackers[m.StackTypeIdx].SendOutput(m.Diff);
            }
        }
        public override void ReadPastStack()
        {
            while (PastInts.Count > 0 && PastInts.Peek().FrameNum == CurrentIndex)
            {
                var m = PastInts.Pop();
                IntTrackers[m.StackTypeIdx].SendOutput(m.Diff);
            }
            while (PastFloats.Count > 0 && PastFloats.Peek().FrameNum == CurrentIndex)
            {
                var m = PastFloats.Pop();
                FloatTrackers[m.StackTypeIdx].SendOutput(m.Diff);
            }
            while (PastBytes.Count > 0 && PastBytes.Peek().FrameNum == CurrentIndex)
            {
                var m = PastBytes.Pop();
                ByteTrackers[m.StackTypeIdx].SendOutput(m.Diff);
            }
        }

        #endregion Methods
    }
}