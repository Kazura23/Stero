using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DialogueManager : ManagerParent
{
    #region Variables

    [System.Serializable]
    public class DialogueType
    {
        public string character;
        public string[] quotes;
    }

    public List<DialogueType> dial;



    #endregion

    #region Public Methods
    #endregion

    #region Private Methods
    protected override void InitializeManager()
    {
       
    }
    #endregion

}
