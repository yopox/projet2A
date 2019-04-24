using Microsoft.Xna.Framework;
using mono.core.Definitions;
using System.Collections.Generic;

namespace mono.core.States
{
    static class Cutscene
    {
        static Queue<CutsceneAction> actions;
        static Atlas bgImage;
        static float fontZoom = 2f;
        static CutsceneAction nextAction;
        static bool activeAction;
        static Color activeColor;

        static void Prepare(Queue<CutsceneAction> act, Atlas atlas)
        {
            actions = act;
            bgImage = atlas;
        }

        static void Update()
        {
            if (!activeAction)
            {
                nextAction = actions.Dequeue();
                switch (nextAction.type)
                {
                    case CutsceneActionType.Background:
                        bgImage = Util.ParseEnum<AtlasName>(nextAction.content);
                        break;
                    case CutsceneActionType.Text:
                        break;
                    case CutsceneActionType.NewPage:
                        break;
                    case CutsceneActionType.Wait:
                        break;
                    case CutsceneActionType.Sfx:
                        break;
                    case CutsceneActionType.Bgm:
                        break;
                    case CutsceneActionType.State:
                        break;
                }
            }
        }
    }
}
