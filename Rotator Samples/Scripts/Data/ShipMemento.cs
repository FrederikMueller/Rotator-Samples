using UnityEngine;

// I'm not sure whether several structs or classes with inheritence is more efficient
// Only use structs if the type has all these characteristics:
// It logically represents a single value, similar to primitive types (integer, double, and so on).
// It has an instance size smaller than 16 bytes.
// It is immutable.
// It will not have to be boxed frequently.

public class ShipMemento
{
    // 12+12+4 = 28 bytes
    // 2800 bytes per second per enemy
    // 2.8kB
    // 100 enemies stored for 1s = 0.28MB.
    // 2.8MB for 10s worth of mementos for 100 enemies or 1s worth of 1000 enemies
    // 280MB if you store 100s worth of mementso for 1000 enemies (would never happen in this game)

    public Vector3 rotation;
    public Vector3 position;
    public int hpDiff = 0;

    public ShipMemento(Vector3 pos, int hpDiff, Vector3 rotation)
    {
        this.rotation = rotation;
        position = pos;
        this.hpDiff = hpDiff;
    }
}
public struct MinMemento
{
    public int frame;
    public int diff;

    public MinMemento(int f, int d)
    {
        frame = f;
        diff = d;
    }
}
public class LeanMemento
{
    // 12 + 4 = 16 bytes
    public float zRot;
    public float xPos;
    public float yPos;
    public int hpDiff = 0;

    public LeanMemento(float x, float y, float zRotation, int hpDiff)
    {
        zRot = zRotation;
        xPos = x;
        yPos = y;
        this.hpDiff = hpDiff;
    }
}
public class PositionMemento
{
    // 8 bytes
    public float x;
    public float y;

    public PositionMemento(float x, float y)
    {
        this.x = x;
        this.y = y;
    }
}

public class ByteMemento
{
    // 3 bytes
    public byte b1;
    public byte b2;
    public byte be;
}

public class FloatMemento
{
    // 4 bytes
    public float scale;
    public FloatMemento(float scale) => this.scale = scale;
}

public class VectorIntMemento
{
    // 4 bytes
    public int intValue;
    public Vector3 position;
    public VectorIntMemento(Vector3 pos, int value)
    {
        this.position = pos;
        this.intValue = value;
    }
}
// Create any extra mementos at will, generic mementos are just a waste of time