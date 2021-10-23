using UI;
using UnityEngine;

public class TestSceneController : MonoBehaviour {
    private void Start() {
        UIController.Instance.ShowMainMenu(null, true);
    }
}
