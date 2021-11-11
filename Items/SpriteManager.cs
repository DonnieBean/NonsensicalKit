using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NonsensicalKit.Manager
{
    public class SpriteManager : MonoSingleton<SpriteManager>
    {
        private Dictionary<string, SpriteInfo> crtSprites;


        public void SetSprite(string spriteName, Func<Sprite> spriteCreateMethod )
        {
            crtSprites.Add(spriteName,new SpriteInfo(spriteCreateMethod, 0));
        }

        public bool TyrGetSprite(NonsensicalMono user,string spriteName,out Sprite sprite)
        {
            sprite = null;
            if (crtSprites.ContainsKey(spriteName))
            {
                if (crtSprites[spriteName].Sprite == null)
                {
                    crtSprites[spriteName].Sprite = crtSprites[spriteName].SpriteCreateMethod();
                }
                crtSprites[spriteName].Count++;
                sprite = crtSprites[spriteName].Sprite;

                user.DestroyAction +=()=> RecoverySprite(spriteName);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void RecoverySprite(string spriteName)
        {
            crtSprites[spriteName].Count--;
            if (crtSprites[spriteName].Count==0)
            {
                Destroy(crtSprites[spriteName].Sprite);
                crtSprites[spriteName].Sprite = null;
            }
        }

        class SpriteInfo
        {
            public Sprite Sprite;
            public Func<Sprite> SpriteCreateMethod;
            public int Count;

            public SpriteInfo(Func<Sprite> spriteCreateMethod, int count)
            {
                SpriteCreateMethod = spriteCreateMethod;
                Count = count;
            }
        }
    }
    
}


