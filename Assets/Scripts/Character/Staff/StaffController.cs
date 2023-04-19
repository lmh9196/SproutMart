using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class StaffController : MonoBehaviour
{
    public CharData charData;

    public CropsData cropsData;

    [HideInInspector] public Staff[] staffChilds;
    Transform waitingPos;
    public Vector3 waitingDir;

    [HideInInspector] public List<TableArea> getAreaList;
    [HideInInspector] public List<TableArea> setAreaList;


    public List<Table> achiveTableList = new();

    private void Awake()
    {
        staffChilds = transform.GetComponentsInChildren<Staff>(true);

        if (charData.SetMenuType(CharData.MenuType.STACK) != null) { charData.SetUpgradePrice(CharData.MenuType.STACK, 5); }
        if (charData.SetMenuType(CharData.MenuType.STACK) != null) { charData.SetUpgradePrice(CharData.MenuType.COOLTIME, 5); }

        for (int i = 0; i < staffChilds.Length; i++) { staffChilds[i].gameObject.SetActive(false); }

        RegistWorkArea();

    }
    private void Start()
    {
        waitingPos = transform.Find("WaitingPos");

        for (int i = 0; i < staffChilds.Length; i++)
        {
            staffChilds[i].waitingPos = waitingPos.GetChild(i);
        }
     
    }

    void Update()
    {
        UpdateAchiveCheck();

        for (int i = 0; i < staffChilds.Length; i++)
        {
            if (i < charData.stat[(int)CharData.MenuType.COUNT].Level) { staffChilds[i].gameObject.SetActive(true); }
            else { staffChilds[i].gameObject.SetActive(false); }
        }
    }

    void UpdateAchiveCheck()
    {
        if (achiveTableList.Count > 0)
        {
            for (int i = 0; i < achiveTableList.Count; i++)
            {
                if (achiveTableList[i].data.IsUnlock && achiveTableList[i].transform.parent.gameObject.activeSelf) { charData.IsAchiveTrigger = true; break; }
                else { charData.IsAchiveTrigger = false; }
            }
        }
        else { charData.IsAchiveTrigger = true; }
    }

    void RegistWorkArea()
    {
        for (int i = 0; i < achiveTableList.Count; i++)
        {
            SetArea[] setAreas = achiveTableList[i].transform.GetComponentsInChildren<SetArea>(true);
            GetArea[] getAreas = achiveTableList[i].transform.GetComponentsInChildren<GetArea>(true);

            for (int j = 0; j < setAreas.Length; j++) 
            {
                if (setAreas[j].cropsData == cropsData) { setAreaList.Add(setAreas[j]); } 
            }
            for (int j = 0; j < getAreas.Length; j++) 
            {
                if (getAreas[j].cropsData == cropsData) { getAreaList.Add(getAreas[j]); }
            }
        }

        for (int i = 0; i < staffChilds.Length; i++)
        {
            staffChilds[i].getList = getAreaList;
            staffChilds[i].setList = setAreaList;
        }
    }
   
}
