using UI;
using UnityEngine;

public class TestSceneController : MonoBehaviour {
    [SerializeField] private bool _cleanOnStart = true;
    private void Start() {
        if (_cleanOnStart)
            PlayerPrefs.DeleteAll();

        UIController.Instance.ShowMainMenu(null, true);
    }
}
