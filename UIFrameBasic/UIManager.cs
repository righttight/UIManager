using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class UIManager
{
    
    public Dictionary<string,GameObject> dict_uiObject;

    /
    public Stack<BasePanel> stack_ui;

    
    public GameObject CanvasObj;


    private static UIManager instance;
    
    public static UIManager GetInstance() 
    {
        if (instance == null)
        {
            Debug.LogError("UIManager ʵ�岻���ڣ�");
            return instance;
        }
        else 
        {
            return instance;
        }
    }


    
    public UIManager() 
    {
        instance = this;
        dict_uiObject = new Dictionary<string, GameObject>();
        stack_ui = new Stack<BasePanel>();
    }
    
    public GameObject GetSingleObject(UIType ui_info) 
    {
        if (dict_uiObject.ContainsKey(ui_info.Name)) 
        {
            return dict_uiObject[ui_info.Name];
        }

        if (CanvasObj == null)
        {
            Debug.Log("����Canvas");
            CanvasObj = UIMethods.GetInstance().FindCanvas();
        }

        if (!dict_uiObject.ContainsKey(ui_info.Name)) 
        {
            if (CanvasObj == null)
            {
                return null;
            }
            else 
            {
                
                GameObject ui_obj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(ui_info.Path), CanvasObj.transform);
                return ui_obj;
            }
        }
        return null;
    }

    
    public void Push(BasePanel basePanel_push) 
    {
        Debug.Log("ִ��Push");
        if (stack_ui.Count > 0) 
        {
            
            stack_ui.Peek().OnDisable();
        }

        GameObject BasePanle_pushObj = GetSingleObject(basePanel_push.uiType);
        dict_uiObject.Add(basePanel_push.uiType.Name, BasePanle_pushObj);

        basePanel_push.ActiveObj = BasePanle_pushObj;

        if (stack_ui.Count == 0)
        {
            stack_ui.Push(basePanel_push);

        }
        else 
        {
            if (stack_ui.Peek().uiType.Name != basePanel_push.uiType.Name)
            {
                stack_ui.Push(basePanel_push);
            }
        }
        
        basePanel_push.OnStart();

    }


   
    public void Pop(bool isload)
    {
        if (isload == true)  
        {
            if (stack_ui.Count > 0)  
            {
                stack_ui.Peek().OnDisable();
                stack_ui.Peek().OnDestory();
                GameObject.Destroy(dict_uiObject[stack_ui.Peek().uiType.Name]);
                dict_uiObject.Remove(stack_ui.Peek().uiType.Name);
                stack_ui.Pop();
                Pop(true);
            }
        }

        if (isload == false)  
        {
            if (stack_ui.Count > 0)
            {
                stack_ui.Peek().OnDisable();
                stack_ui.Peek().OnDestory();
                GameObject.Destroy(dict_uiObject[stack_ui.Peek().uiType.Name]);
                dict_uiObject.Remove(stack_ui.Peek().uiType.Name);
                stack_ui.Pop();

                if (stack_ui.Count > 0)
                {
                    stack_ui.Peek().OnEnable();
                }

            }
        }
        
    }
}
