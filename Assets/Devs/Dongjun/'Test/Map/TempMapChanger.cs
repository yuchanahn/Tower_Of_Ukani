using UnityEngine;
using UnityEngine.SceneManagement;

public class TempMapChanger : MonoBehaviour
{
    [SerializeField] private Transform map1Pos;
    [SerializeField] private Transform map2Pos;
    [SerializeField] private GameObject map2GO;

    private void Start()
    {
        switch (CurMap.num)
        {
            case 1:
                GM.Player.transform.position = map1Pos.position;
                map2GO.SetActive(false);
                break;
            case 2:
                GM.Player.transform.position = map2Pos.position;
                map2GO.SetActive(true);
                break;
            default:
                GM.Player.transform.position = map1Pos.position;
                map2GO.SetActive(false);
                break;
        }
    }
    private void Update()
    {
        ReloadScene();
    }

    private void ReloadScene()
    {
        if (PlayerStats.Inst.IsDead && Input.GetKeyDown(KeyCode.Return))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void ToMap1()
    {
        CurMap.num = 1;
        ReloadScene();
    }
    public void ToMap2()
    {
        CurMap.num = 2;
        ReloadScene();
    }
}

public static class CurMap
{
    public static int num = 1;
}
