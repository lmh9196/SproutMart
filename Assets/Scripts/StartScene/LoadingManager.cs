using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.Threading;
public class LoadingManager : MonoBehaviour
{
    public static LoadingManager instance = null;

    public Text text;
    public string SceneName;

    public Player player;

    float time;

    private void Awake()
    {
        if (instance == null) { instance = this; }

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        text.text = ((int)(time * 100)).ToString() + "%";
    }

    public void StartLoading(string targetScene)
    {
        GameManager.instance.canvasList.SetLoadingCanvas();

        StartCoroutine(LoadAsynSceneCoroutine(targetScene));
    }

    IEnumerator LoadAsynSceneCoroutine(string targetScene)
    {
        yield return null;

        AsyncOperation op;

      
        op = SceneManager.LoadSceneAsync(targetScene);
        op.allowSceneActivation = false;

        player.animDir.isMenualCtr = true;
        player.animDir.anim.SetFloat("fVertical", -1);
        player.animDir.anim.SetBool("isMove", true);

        MainCamera.instance.ChangeCamTarget(true);

        MainCamera.instance.transform.position = new Vector3(0, 0, -10);
        player.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.2f,10));



        float timer = 0.0f;

        while (!op.isDone)
        {

            yield return new WaitForSeconds(0.1f);

            timer += Time.deltaTime;

            if (op.progress < 0.9f) 
            {
                time = Mathf.Lerp(time, op.progress, timer); 
                if (time >= op.progress) 
                { 
                    timer = 0f; 
                } 
            }
            else
            {
                time = Mathf.Lerp(time, 1f, timer);
                if (time >= 1.0f)
                {
                    player.animDir.isMenualCtr = false;

                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
