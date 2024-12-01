using UnityEngine;

public class SettingsManager2 : MonoBehaviour
{
    public GameObject settingsPanel; // �������ý���
    private bool isSettingsOpen = false; // ���ý����Ƿ��

    private void Update()
    {
        // ��鰴��1��
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSettings();
        }
    }

    public void ToggleSettings()
    {
        isSettingsOpen = !isSettingsOpen;

        // �򿪻�ر����ý���
        settingsPanel.SetActive(false);
    }
}

