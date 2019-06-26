using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using mono.core.Definitions;
using mono.RenderEngine;
using System;
using System.Collections.Generic;

namespace mono.core.States
{
    static class Cutscene
    {
        static private Queue<CutsceneAction> actions; // File d'actions à effectuer
        static private CutsceneAction action; // Prochaine action à effectuer

        static AtlasName BGImage = AtlasName.NoAtlas; // Image de fond de la cinématique
        static private bool bgImageFading = false; // Etat du fondu de l'image de fond

        static private List<Tuple<string, string>> text = new List<Tuple<string, string>>(); // Texte à afficher
        static private float scale = 2f; // Niveau de zoom de la police d'écriture
        static private Vector2 size = Vector2.Zero; // Taille du texte à afficher
        static private int horizontalOffset = 96; // Offset horizontal d'affichage du texte
        static private int indString = 0; // Indice du texte dans _text jusqu'au quel on affiche 
        static private int indCharacter = 0; // Indice du charactère jusqu'au quel on affiche dans _text
        static private int counter = 0; // Compteur d'affichage des lettres
        static private int frameRefresh = 2; // Vitesse d'affichage des lettres en frame
        static private bool calculusSize = false; // état de finition du calcul de taille du texte
        static private bool newText = true; // nouveau texte à afficher

        // Temps d'attente pour le wait
        static private int deltaFrame = 0;

        // Texture d'affichage
        static Texture2D BackgroundTexture;
        static Texture2D ForegroundTexture;

        /// <summary>
        /// On récupère le script
        /// </summary>
        /// <param name="path">Chemin du script</param>
        public static void Load(string path)
        {
            actions = Util.ParseScript(path);

            //On récupère la prochaine action
            action = actions.Dequeue();
        }

        /// <summary>
        /// On met à jour la prochaine action de la cutscene
        /// </summary>
        /// <param name="gstate">On récupère l'état des controleurs</param>
        public static State Update(GameState gstate, GameTime gameTime)
        {
            // On update le fondu au blanc local si on a une nouvelle image
            if (bgImageFading)
                Util.FadeIn(ref bgImageFading);

            switch (action.Type)
            {
                // Récupération de l'image de fond
                case CutsceneActionType.Background:
                    BGImage = Util.ParseEnum<AtlasName>(action.Content);
                    Util.FadeIn(ref bgImageFading);
                    action = actions.Dequeue();
                    break;
                // Récupération du texte à afficher
                case CutsceneActionType.Text:
                    UpdateText(gstate);
                    break;
                case CutsceneActionType.NewPage:
                    UpdatePage(gstate);
                    break;
                case CutsceneActionType.Wait:
                    UpdateWait(gstate);
                    break;
                case CutsceneActionType.Sfx:
                    var sound = SoundManager.Content.Load<SoundEffect>("Music/SoundEffects/" + action.Content);
                    SoundEffectInstance sfx = sound.CreateInstance();
                    sfx.IsLooped = false;
                    sfx.Play();
                    action = actions.Dequeue();
                    break;
                case CutsceneActionType.Bgm:
                    SoundManager.PlayBGM(action.Content);
                    action = actions.Dequeue();
                    break;
                case CutsceneActionType.State:
                    return UpdateState();
            }
            return State.Cutscene;
        }

        /// <summary>
        /// Affichage de la cutscene
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="am"></param>
        /// <param name="GraphicsDevice"></param>
        public static void Draw(SpriteBatch spriteBatch, AssetManager am, GraphicsDevice GraphicsDevice)
        {
            // Dessin d'un fond noir
            spriteBatch.Draw(Util.GetTexture(GraphicsDevice, BackgroundTexture, Color.Black),
                Vector2.Zero, Color.Black);

            // Dessin de l'artwork
            if (BGImage != AtlasName.NoAtlas)
            {
                var texture = am.GetAtlas(BGImage).Texture;
                spriteBatch.Draw(texture, Vector2.Zero, Color.White);

                // Dessin d'un foreground avec de l'opacité
                spriteBatch.Draw(Util.GetTexture(GraphicsDevice, ForegroundTexture, new Color(0, 0, 0, 120)), Vector2.Zero, Color.White);
            }

            // On fait un fondu au noir si une nouvelle image est affichée
            if (bgImageFading)
                Util.DrawFading(spriteBatch, GraphicsDevice, -1 * Util.FadingSpeed);

            // On affiche un texte si il y en a
            if (text != null && text.Count != 0)
                DrawDialog(spriteBatch, GraphicsDevice);
        }

        /// <summary>
        /// Affichage du dialogue
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="GraphicsDevice"></param>
        static void DrawDialog(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice)
        {
            // Récupère la position pour centrer le dialogue
            Vector2 positionStr = new Vector2(horizontalOffset,
            Rendering.VirtualHeight / 2 - size.Y / 2);

            Vector2 offset = Vector2.Zero;

            // Affichage des blocs de textes complets
            for (int i = 0; i < indString; i++)
            {
                spriteBatch.DrawString(Util.Font,
                    text[i].Item2,
                    positionStr + offset,
                    Util.ColorStringDictionary[text[i].Item1],
                    0.0f, Vector2.Zero, scale, new SpriteEffects(), 0.0f);

                // On modifie l'offset en fonction de la taille du texte déjà affiché
                offset.Y += Util.Font.MeasureString(text[i].Item2).Y * scale;
            }

            // On affiche le texte du bloc non complet
            spriteBatch.DrawString(Util.Font,
                text[indString].Item2.Substring(0, indCharacter),
                positionStr + offset,
                Util.ColorStringDictionary[text[indString].Item1],
                0.0f, Vector2.Zero, scale, new SpriteEffects(), 0.0f);

            counter++;

            // On incrémente les indices de charactères et de chaines
            if (counter == frameRefresh)
            {
                if (indCharacter == text[indString].Item2.Length - 1 && indString < text.Count - 1)
                {
                    indString++;
                    indCharacter = 0;
                }
                else if (indCharacter != text[indString].Item2.Length - 1)
                {
                    indCharacter++;
                }

                switch (text[indString].Item2[indCharacter])
                {
                    case '\n':
                    case '.':
                    case '!':
                    case '?':
                    case ',':
                        counter = -frameRefresh;
                        break;
                    default:
                        counter = 0;
                        break;
                }
            }
        }

        /// <summary>
        /// Remise à zéro des variables
        /// </summary>
        private static void Reset()
        {
            deltaFrame = 0;
            indString = 0;
            indCharacter = 0;
            text.Clear();

            calculusSize = false;
            newText = true;
            size = Vector2.Zero;

            Util.FadeIn();
        }

        /// <summary>
        /// Update le texte de la cinématique
        /// </summary>
        private static void UpdateText(GameState gstate)
        {
            // Rajoute du texte à afficher si il y en a du nouveau
            if (newText)
            {
                text.AddRange(Util.ParseDialog(action.Content));
                SizeCalculus();
                newText = false;
            }
            // Calcul de la taille du texte

            // Récupération de la prochaine action si tout le texte est affiché
            if (indCharacter == text[indString].Item2.Length - 1 && indString == text.Count - 1)
            {
                action = actions.Dequeue();
                newText = true;
            }

            if (Util.JustPressed(gstate, Keys.A))
            {
                indString = text.Count - 1;
                indCharacter = text[indString].Item2.Length - 1;
            }
        }

        /// <summary>
        /// Calcule la taille du texte à afficher jusqu'au prochain newpage ou jusqu'à la fin du script
        /// </summary>
        private static void SizeCalculus()
        {
            // On calcule la taille une seule fois lorsqu'on récupère le fichier
            if (!calculusSize)
            {
                calculusSize = true;

                // On récupère toutes les actions depuis cette action texte comprise
                CutsceneAction[] listActions = new CutsceneAction[actions.Count + 1];
                listActions[0] = action;
                actions.CopyTo(listActions, 1);

                List<Tuple<string, string>> totalText = new List<Tuple<string, string>>();

                int j = 0;

                // On récupère tout le texte jusqu'au prochain newpage
                while (j < listActions.Length && listActions[j].Type != CutsceneActionType.NewPage)
                {
                    if (listActions[j].Type == CutsceneActionType.Text)
                    {
                        totalText.AddRange(Util.ParseDialog(listActions[j].Content));
                    }
                    j++;
                }

                //Calcul de la taille totale du texte à afficher
                for (int i = 0; i < totalText.Count; i++)
                {
                    //Calcul hauteur
                    size.Y += (int)(Util.Font.MeasureString(totalText[i].Item2).Y * scale);
                }
            }
        }

        /// <summary>
        /// Enlève le texte à l'écran
        /// </summary>
        /// <param name="gstate"></param>
        private static void UpdatePage(GameState gstate)
        {
            //Attente de l'appui du bouton pour changer d'action
            if (gstate.ksn.IsKeyDown(Keys.Space) && gstate.ksn.IsKeyDown(Keys.Space))
            {
                indString = 0;
                indCharacter = 0;
                text.Clear();
                calculusSize = false;
                size = Vector2.Zero;
                action = actions.Dequeue();
            }
        }

        /// <summary>
        /// Update l'état du jeu après un fading
        /// </summary>
        /// <returns></returns>
        private static State UpdateState()
        {
            // On lance le fading
            bool over = Util.FadeOut();
            // Lorsque le fading est fini, on change d'état
            if (over)
            {
                Reset();
                Util.NewState = true;
                return Util.ParseEnum<State>(action.Content);
            }

            return State.Cutscene;
        }

        /// <summary>
        /// Mets en pause la cutscene
        /// </summary>
        private static void UpdateWait(GameState gstate)
        {
            // On attend le nombre de frame présent dans le wait
            deltaFrame += 1;
            if (deltaFrame == Int32.Parse(action.Content) || Util.JustPressed(gstate, Keys.A))
            {
                deltaFrame = 0;
                action = actions.Dequeue();
            }
        }
    }
}