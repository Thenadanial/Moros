using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance { get; private set; }

    private CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 1f;

    // �洢ÿ���������͵�ı�ǩ��λ��
    private Dictionary<string, SceneTransitionDestination[]> sceneDestinations = new Dictionary<string, SceneTransitionDestination[]>();
    //private Dictionary<string, SceneTransitionDestination> destinations = new Dictionary<string, SceneTransitionDestination>();

    private SceneTransitionDestination.DestinationTag storedDestinationTag;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        fadeCanvasGroup = GetComponentInChildren<CanvasGroup>();
        if (fadeCanvasGroup == null)
        {
            Debug.LogError("FadeCanvasGroup is missing! Ensure a CanvasGroup is attached to the TransitionManager or its children.");
        }
        else
        {
            fadeCanvasGroup.alpha = 0f;
        }

        // ע�᳡�������¼�
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    private void OnDestroy()
    {
        // �Ƴ��¼�����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Cache the destination when switching scenes
    public void SetDestinationTag(SceneTransitionDestination.DestinationTag destinationTag)
    {
        storedDestinationTag = destinationTag;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ÿ���³������غ���´��͵�����
        UpdateSceneDestinations(scene.name, storedDestinationTag);
    }

    private void UpdateSceneDestinations(string sceneName, SceneTransitionDestination.DestinationTag destinationTag)
    {
        StartCoroutine(WaitAndPlacePlayer(destinationTag));
    }

    private IEnumerator WaitAndPlacePlayer(SceneTransitionDestination.DestinationTag destinationTag)
    {
        GameObject player = null;

        // �ȴ�ֱ����Ҷ��󱻼���
        while (player == null)
        {
            player = GameObject.FindWithTag("Player");
            yield return null; // �ȴ���һ֡
        }

        // ��ȡĿ�괫�͵�
        var destinationPoints = FindObjectsOfType<SceneTransitionDestination>();
        foreach (var point in destinationPoints)
        {
            if (point.destinationTag == destinationTag)
            {
                player.transform.position = point.transform.position;
                yield break;
            }
        }

        Debug.LogWarning("No matching destination point found for tag: " + destinationTag);
    }


    // �л�����
    public void ChangeScene(string sceneName, SceneTransitionDestination.DestinationTag destinationTag)
    {
        StartCoroutine(FadeAndSwitchScene(sceneName, destinationTag));
    }

    // ���뵭��Ч���ͳ����л�
    private IEnumerator FadeAndSwitchScene(string sceneName, SceneTransitionDestination.DestinationTag destinationTag)
    {
        yield return StartCoroutine(Fade(1));

        // �����³���
        SceneManager.LoadScene(sceneName);

        yield return new WaitForEndOfFrame();  // ȷ��������ȫ����

        // ����ɫ���õ�Ŀ��λ��
        SetPlayerPosition(sceneName, destinationTag);

        yield return StartCoroutine(Fade(0));
    }

    // ������ҵ�λ��
    private void SetPlayerPosition(string sceneName, SceneTransitionDestination.DestinationTag destinationTag)
    {
        if (sceneDestinations.ContainsKey(sceneName))
        {
            SceneTransitionDestination[] destinations = sceneDestinations[sceneName];

            foreach (var destination in destinations)
            {
                if (destination.destinationTag == destinationTag)
                {
                    // �ҵ�Ŀ���ǩ�����ý�ɫλ��
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    if (player != null)
                    {
                        player.transform.position = destination.transform.position;
                        destination.OnPlayerReach(); // ���õ����¼�
                    }
                    break;
                }
            }
        }
    }

    // ���Ƶ��뵭��
    private IEnumerator Fade(float targetAlpha)
    {
        if (fadeCanvasGroup == null) yield break;

        // ���� CanvasGroup ��ȷ����ɼ�
        fadeCanvasGroup.gameObject.SetActive(true);

        float startAlpha = fadeCanvasGroup.alpha;
        float time = 0f;

        while (time < fadeDuration)
        {
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            time += Time.deltaTime;
            yield return null;
        }

        fadeCanvasGroup.alpha = targetAlpha;

        // ����ȫ������Alpha Ϊ 0��ʱ�����Խ��� CanvasGroup
        if (targetAlpha == 0)
        {
            fadeCanvasGroup.gameObject.SetActive(false);
        }
    }


    public void TransitionToScene(string sceneName, SceneTransitionDestination.DestinationTag destinationTag)
    {
        // Store the destination tag before loading the scene
        TransitionManager.Instance.SetDestinationTag(destinationTag);
        StartCoroutine(FadeAndSwitchScene(sceneName, destinationTag));
    }
}
