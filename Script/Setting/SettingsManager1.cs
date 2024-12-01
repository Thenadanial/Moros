using UnityEngine;

public class SettingsManager1 : MonoBehaviour
{
    public GameObject settingsPanel; // �������ý���
    private bool isSettingsOpen = false; // ���ý����Ƿ��

    private void Update()
    {
        // ��鰴��1��
        if (Input.GetMouseButtonDown(0))
        {
            ToggleSettings();
        }
    }

    public void ToggleSettings()
    {
        isSettingsOpen = !isSettingsOpen;

        // �򿪻�ر����ý���
        settingsPanel.SetActive(true);
    }
}

