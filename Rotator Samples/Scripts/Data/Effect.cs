public class Effect
{   // Naming Conventions for files and animations:
    // Animation: EffectID as string. +REV | +Inverted | +InvertedREV.  "DespawnREV" "DespawnInvertedREV"
    // Soundfiles: EffectID as string "Despawn"
    public EffectID ID { get; set; }
    public string AnimationName { get; set; }
    public string SoundID { get; set; }
    public float AnimDuration { get; set; }

    public Effect(EffectID iD, float animDuration)
    {
        ID = iD;
        AnimDuration = animDuration;
        AnimationName = iD.ToString();
        SoundID = iD.ToString();
    }
    public Effect(EffectID iD, string animationName, float animDuration, EffectID soundID)
    {
        ID = iD;
        AnimationName = animationName;
        AnimDuration = animDuration;

        SoundID = soundID.ToString();
    }
}