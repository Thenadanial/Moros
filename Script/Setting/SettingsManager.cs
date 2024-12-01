using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public GameObject settingsPanel; // �������ý���
    public GameObject settingsPanel1; // �������ý���
    private bool isSettingsOpen = false; // ���ý����Ƿ��

    private void Update()
    {
        // ��鰴��Esc��
        if (Input.GetKeyDown(KeyCode.Escape) && !settingsPanel1.activeInHierarchy)
        {
            ToggleSettings();
        }
    }

    public void ToggleSettings()
    {
        isSettingsOpen = !isSettingsOpen;

        // �򿪻�ر����ý���
        settingsPanel.SetActive(isSettingsOpen);

        // ��ͣ��ָ���Ϸ
        Time.timeScale = isSettingsOpen ? 0 : 1;

    }

    public void CloseSettings()
    {
        isSettingsOpen = false;
        settingsPanel.SetActive(false);
        Time.timeScale = 1;
    }
}

