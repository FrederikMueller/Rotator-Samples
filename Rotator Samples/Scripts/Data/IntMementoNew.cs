public readonly struct IntMementoNew
{
    public IntMementoNew(int framenum, int diff, byte typeidx)
    {
        FrameNum = framenum;
        Diff = diff;
        StackTypeIdx = typeidx;
    }
    public readonly int FrameNum;
    public readonly int Diff;
    public readonly byte StackTypeIdx;
}

public readonly struct FloatMementoNew
{
    public FloatMementoNew(int framenum, float diff, byte typeidx)
    {
        FrameNum = framenum;
        Diff = diff;
        StackTypeIdx = typeidx;
    }
    public readonly int FrameNum;
    public readonly float Diff;
    public readonly byte StackTypeIdx;
}

public readonly struct ByteMementoNew
{
    public ByteMementoNew(int framenum, byte diff, byte typeidx)
    {
        FrameNum = framenum;
        Diff = diff;
        StackTypeIdx = typeidx;
    }
    public readonly int FrameNum;
    public readonly byte Diff;
    public readonly byte StackTypeIdx;
}