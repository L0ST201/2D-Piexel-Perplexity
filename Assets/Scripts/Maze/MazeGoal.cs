using UnityEngine;

public class MazeGoal : MonoBehaviour
{
    public Sprite closedGoalSprite;
    public Sprite openedGoalSprite;
    private MazeDirectives mazeDirectives;

    void Start()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = closedGoalSprite;
        mazeDirectives = FindObjectOfType<MazeDirectives>();
    }

    public void OpenGoal()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = openedGoalSprite;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            transform.parent.SendMessage("OnGoalReached", SendMessageOptions.DontRequireReceiver);
        }
    }

}
