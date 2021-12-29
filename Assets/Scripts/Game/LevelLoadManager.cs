using UnityEngine.SceneManagement;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections;

public class LevelLoadManager : MonoBehaviour
{
    string levelPrefix = "Level";
    // List<AsyncOperation> mSceneToLoad = new  List<AsyncOperation>();
    public static LevelLoadManager instance;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void LoadLevelOf(int inLevelIndex)
    {
        SceneManager.LoadScene(levelPrefix + inLevelIndex);
    }
    public void LoadLevelASyncOf(int inLevelIndex)
    {
        SceneManager.LoadSceneAsync(levelPrefix + inLevelIndex);
    }
    public async void BacktoHome()
    {
        // SceneManager.LoadSceneAsync("CloudLoadScene");
        AsyncOperation scene = SceneManager.LoadSceneAsync(levelPrefix + GameManager.Instance._playerCurrentLevel);
        //Scene.allowSceneActivation = false;
        do
        {
            await Task.Delay(100);
        } while (scene.progress < 0.9f);

        //StartCoroutine(Loading(Scene));

        //Make the GameToLoad GameManager Data
        StartCoroutine(Load(scene));
        GameManager.Instance._IsRefreshNeeded = true;

    }
    public async void LoadScene()
    {
        //SceneManager.LoadSceneAsync("CloudScene");
        AsyncOperation scene = SceneManager.LoadSceneAsync("1");
        scene.allowSceneActivation = false;
        do
        {
            await Task.Delay(100);
        } while (scene.progress < 0.9f);
        transform.GetChild(0).GetChild(0).GetComponent<Animator>().SetTrigger("AnimateCloud");

        StartCoroutine(Load(scene));
    }



    IEnumerator Load(AsyncOperation scene)
    {


        yield return new WaitForSeconds(2);
        scene.allowSceneActivation = true;

        //SceneManager.UnloadSceneAsync("CloudScene");

    }
}
