using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectNation : MonoBehaviour
{
    [SerializeField]
    private string nextScene = "Map01";

    [SerializeField]
    private int index = 0;

    public void SelectEuropeNation(int i)
    {
        index = i;
        Settings.playerNationId = index;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(nextScene);
    }
}
