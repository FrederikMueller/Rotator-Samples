using UnityEngine;
using TimeControl;

internal class ScuffedTest : MonoBehaviour
{
    public TCPosition timeController;

    private void Awake()
    {
        timeController = GetComponent<TCPosition>();
    }
    private void FixedUpdate()
    {
        if (timeController.IsRewinding)
        {
            timeController.ProgressBackwardsInHistory();
        }
        else
        {
            var pos = new Vector3(Random.Range(-.04f, -.041f), Random.Range(-.04f, .041f), 0);
            transform.position += pos;
            timeController.ProgressForwardsInHistory();
        }
    }
}