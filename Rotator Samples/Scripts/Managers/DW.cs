using System.Collections.Generic;

public static class DW
{
    public static DWObject dWObject;
    private static Dictionary<string, int> log = new Dictionary<string, int>();
    private static int currentFreeSlot;

    public static void Clear()
    {
        currentFreeSlot = 0;
        log.Clear();
    }

    public static void Log(string name, int i)
    {
        if (log.ContainsKey(name))
            dWObject.texts[log[name]].text = $"{name}: {i}";
        else
        {
            log.Add(name, currentFreeSlot);
            dWObject.texts[log[name]].text = $"{name}: {i}";

            currentFreeSlot++;
        }
    }
    public static void Log(string name, float f)
    {
        if (log.ContainsKey(name))
            dWObject.texts[log[name]].text = $"{name}: {f}";
        else
        {
            log.Add(name, currentFreeSlot);
            dWObject.texts[log[name]].text = $"{name}: {f}";

            currentFreeSlot++;
        }
    }
}