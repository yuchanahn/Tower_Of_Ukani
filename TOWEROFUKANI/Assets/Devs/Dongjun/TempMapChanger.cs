using UnityEngine;
using UnityEngine.SceneManagement;

public class TempMapChanger : MonoBehaviour
{
    [SerializeField] private Transform map1Pos;
    [SerializeField] private Transform map2Pos;
    [SerializeField] private GameObject map2GO;

    private void Start()
    {
        if (CurMap.num == 2)
        {
            GM.Player.transform.position = map2Pos.position;
            map2GO.SetActive(true);
        }
        else
            GM.Player.transform.position = map1Pos.position;
    }
    private void Update()
    {
        if (PlayerStats.Heath.Value <= 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ToMap1()
    {
        CurMap.num = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ToMap2()
    {
        CurMap.num = 2;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

public static class CurMap
{
    public static int num = 1;
}
