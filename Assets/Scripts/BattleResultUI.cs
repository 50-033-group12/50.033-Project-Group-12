using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleResultUI : MonoBehaviour
{
    private static BattleResultUI _instance;

    public static BattleResultUI GetInstance()
    {
        return _instance;
    }

    [SerializeField] private Text textField;

    // Start is called before the first frame update
    void Start()
    {
        _instance = this;
        transform.localScale = Vector3.zero; // hide the UI
    }

    public void PlayerDied(int playerId)
    {
        var winner = playerId == 1 ? 2 : 1;
        textField.text = $"PLAYER {winner} WINS";
        MakePanelAppear();
    }

    public void Timeout()
    {
        textField.text = "DRAW!";
        MakePanelAppear();
    }

    void MakePanelAppear()
    {
        transform.localScale = Vector3.one;
        transform.position += new Vector3(0, 500, 0);
        LeanTween.moveY(gameObject, transform.position.y - 500f, 0.5f).setEaseOutCubic()
            .setOnComplete(() =>
            {
                LeanTween.scaleX(gameObject, 1, 2f).setOnComplete(() =>
                {
                    CurtainController.GetInstance().CloseCurtain(() =>
                    {
                        var thisCurtain = CurtainController.GetInstance();
                        DontDestroyOnLoad(thisCurtain.transform.parent);
                        thisCurtain.transform.parent.GetComponent<Canvas>().sortingOrder = 1;
                        foreach (Transform child in thisCurtain.transform.parent)
                        {
                            if (child.gameObject != thisCurtain.gameObject) Destroy(child.gameObject);
                        }

                        SceneManager.LoadScene("ThymioSelect");
                        thisCurtain.OpenCurtain(() => { Destroy(thisCurtain.transform.parent.gameObject); });
                    });
                });
            })
            ;
    }
}