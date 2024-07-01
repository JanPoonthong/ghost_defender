using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    static public LoadScene ChangeScene;
    [SerializeField] private GameObject _loadingScenePrefab;
    private GameObject _loadingSceneInstance;
    private Transform _canvasTransform;

    private void Awake()
    {
        ChangeScene = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Load(string sceneName)
    {
        StartCoroutine(LoadingScene(sceneName));
    }

    private IEnumerator LoadingScene(string sceneName)
    {

        if (_loadingSceneInstance == null)
        {
            _loadingSceneInstance = Instantiate(_loadingScenePrefab, _canvasTransform);
            DontDestroyOnLoad(_loadingSceneInstance);
        }

        _loadingSceneInstance.SetActive(true);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(0.5f);
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }

        yield return new WaitForSeconds(4f);
        _loadingSceneInstance.SetActive(false);
    }
}
