using UnityEngine;
using UnityEngine.SceneManagement;

public class MainLevel_TestFunc : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void btn_GoOut()
    {
        Debug.Log("외출하기()");
        SceneManager.LoadScene("OutingScene");
    }

    public void btn_CheckSchedule()
    {
        Debug.Log("스케줄확인하기()");
    }

    public void btn_ChcekStatus()
    {
        Debug.Log("상태확인()");
    }

    public void btn_Communication()
    {
        Debug.Log("대화하기()");
        SceneManager.LoadScene("Comunication_Scence");
    }
}