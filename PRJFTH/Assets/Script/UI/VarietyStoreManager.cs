using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VarietyStoreManager : MonoBehaviour
{
    public void OnClickReturn()
    {
        SceneManager.LoadScene("OutingScene");
    }
}
