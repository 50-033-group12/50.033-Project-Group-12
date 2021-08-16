using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// it's a manager that manages UI managers....
public class UIManagerManager : MonoBehaviour
{
    [SerializeField] private UIManager player1UI;
    [SerializeField] private UIManager player2UI;
    
    private static UIManagerManager _instance;

    public static UIManagerManager GetInstance()
    {
        return _instance;
    }
    // Start is called before the first frame update
    void Start()
    {
        _instance = this;
    }
    
    public UIManager GetPlayerUI(int playerId)
    {
        if (playerId == 1)
        {
            return player1UI;
        }
        if (playerId == 2)
        {
            return player2UI;
        }
        return null;
    }
}
