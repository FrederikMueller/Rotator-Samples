using System.Collections.Generic;

public static class EffectDB
{
    public static Dictionary<EffectID, Effect> effectDB = new Dictionary<EffectID, Effect>();
    // TODO  try to extract total anim duration from animator somehow
    public static void InitDBTesting()
    {
        if (effectDB.Count == 0)
        {
            effectDB.Add(EffectID.Despawn, new Effect(EffectID.Despawn, .6f));
            effectDB.Add(EffectID.Inversion, new Effect(EffectID.Inversion, .95f));
            effectDB.Add(EffectID.ShockCombo, new Effect(EffectID.ShockCombo, .9f));
        }
    }
    public static Effect GetEffect(EffectID id) => effectDB[id];
    public static void ClearDB() => effectDB.Clear();
}