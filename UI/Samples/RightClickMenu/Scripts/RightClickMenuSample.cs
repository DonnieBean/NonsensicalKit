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
        lcmis.Add(new RightClickMenuItem(null,"第一个按钮",()=> { txt_Show.text="按下了第一个按钮"; })) ;
        lcmis.Add(new RightClickMenuItem("sampleSprite1", "第二个按钮",()=> { txt_Show.text = "按下了第二个按钮"; })) ;
        lcmis.Add(new RightClickMenuItem("sampleSprite2", "第三个按钮",()=> { txt_Show.text = "按下了第三个按钮"; })) ;
        lcmis.Add(new RightClickMenuItem("sampleSprite2", "第四个按钮",()=> { txt_Show.text = "按下了第四个按钮"; })) ;
    }

    private void Update()
    {
        if (ic.mouseRightKeyDown)
        {
            Publish((int)UIEnum.OpenRightClickMenu,lcmis);
        }
    }
}
