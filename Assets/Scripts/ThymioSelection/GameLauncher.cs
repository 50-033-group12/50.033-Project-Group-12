using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace ThymioSelection
{
    public class GameLauncher : MonoBehaviour
    {
        [SerializeField] private ThymioSelectPlayer player1Selection;
        [SerializeField] private ThymioSelectPlayer player2Selection;
        private bool _fired = false;

        void Start()
        {
            LoadoutManager.Reset();
        }
        void Update()
        {
            if (player1Selection.IsReady() && player2Selection.IsReady() && !_fired)
            {
                _fired = true;
                LoadoutManager.JoinPlayer(1, player1Selection.GetComponent<PlayerInput>().devices[0]);
                var player1Loadout = player1Selection.GetLoadout();
                LoadoutManager.ChoosePrimary(1, player1Loadout.Item1);
                LoadoutManager.ChooseSecondary(1, player1Loadout.Item2);
                LoadoutManager.ChooseUltimate(1, player1Loadout.Item3);

                LoadoutManager.JoinPlayer(2, player2Selection.GetComponent<PlayerInput>().devices[0]);

                var player2Loadout = player2Selection.GetLoadout();
                LoadoutManager.ChoosePrimary(2, player2Loadout.Item1);
                LoadoutManager.ChooseSecondary(2, player2Loadout.Item2);
                LoadoutManager.ChooseUltimate(2, player2Loadout.Item3);

                CurtainController.GetInstance().CloseCurtain(() =>
                {
                    var curtain = CurtainController.GetInstance();
                    DontDestroyOnLoad(curtain.transform.parent);
                    curtain.transform.parent.GetComponent<Canvas>().sortingOrder = 1;
                    foreach (Transform child in curtain.transform.parent)
                    {
                        if (child.gameObject != curtain.gameObject) Destroy(child.gameObject);
                    }

                    SceneManager.LoadScene("DeskLevel", LoadSceneMode.Single);
                    curtain.OpenCurtain(() => { Destroy(curtain.transform.parent.gameObject); });
                });
            }
        }
    }
}