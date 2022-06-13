using UnityEngine;

[CreateAssetMenu(fileName = "EnemyModel", menuName = "ScriptableObjects/EnemyModel")]
public class EnemyModel : ScriptableObject
{
    public string name;
    public Sprite sprite;
    public float scaleX;
    public float scaleY;
}