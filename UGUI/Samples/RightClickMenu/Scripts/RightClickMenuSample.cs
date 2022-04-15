using NonsensicalKit;
using NonsensicalKit.Manager;
using NonsensicalKit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightClickMenuSample : NonsensicalMono
{
    [SerializeField] private Text txt_Show;
    [SerializeField] private Sprite sampleSprite1;
    [SerializeField] private Sprite sampleSprite2;

    private InputCenter ic;
    private SpriteManager sm;
    private List<RightClickMenuItem> lcmis;

    protected override void Awake()
    {
        base.Awake();
        ic = InputCenter.AutoInstance;
        sm = SpriteManager.AutoInstance;
        lcmis = new List<RightClickMenuItem>();

        sm.SetSprite("sampleSprite1",()=> { return sampleSprite1; });
        sm.SetSprite("sampleSprite2",()=> { return sampleSprite2; });
        lcmis.Add(new RightClickMenuItem(null,"��һ����ť",()=> { txt_Show.text="�����˵�һ����ť"; })) ;
        lcmis.Add(new RightClickMenuItem("sampleSprite1", "�ڶ�����ť",()=> { txt_Show.text = "�����˵ڶ�����ť"; })) ;
        lcmis.Add(new RightClickMenuItem("sampleSprite2", "��������ť",()=> { txt_Show.text = "�����˵�������ť"; })) ;
        lcmis.Add(new RightClickMenuItem("sampleSprite2", "���ĸ���ť",()=> { txt_Show.text = "�����˵��ĸ���ť"; })) ;
    }

    private void Update()
    {
        if (ic.mouseRightKeyDown)
        {
            Publish((int)UIEnum.OpenRightClickMenu,lcmis);
        }
    }
}
