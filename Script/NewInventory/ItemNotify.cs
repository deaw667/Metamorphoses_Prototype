using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemNotify : MonoBehaviour
{
    public Text itemAddedName;
    public Text itemaddedamount;
    // Start is called before the first frame update

    public void SetUpNotify(string name)
    {
        itemAddedName.text = name;
    }

}
