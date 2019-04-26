using Microsoft.Xna.Framework;
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
        static Queue<CutsceneAction> actions; // File d'actions à effectuer
        static CutsceneAction action; // Prochaine action à effectuer

        static AtlasName bgImage; // Image de fond de la cinématique

        static private List<Tuple<string, string>> _text; // Texte à afficher
        static float scale = 2f; // Niveau de zoom de la police d'écriture
        static private Vector2 _size; // Taille du texte à afficher
        static private int _indString = 0; // Indice du texte dans _text jusqu'au quel on affiche 
        static private int _indCharacter = 0; // Indice du charactère jusqu'au quel on affiche dans _text
        static private int _counter = 0; // Compteur d'affichage des lettres
        static private int _frameRefresh = 4; // Vitesse d'affichage des lettres en frame

        // Temps d'attente pour le wait
        static private int _deltaFrame = 0;

        // Texture d'affichage
        static Texture2D BackgroundTexture;
        static Texture2D ForegroundTexture;

        // Gestion du fondu noir de sortie de cutscene
        static private bool _fadingOut = false;
        static private bool _isFadingOutOver = false;
        static private int _fadingColorOpacity = 0;
        static private int _fadingSpeed = 8;


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
            switch (action.type)
            {
                // Récupération de l'image de fond
                case CutsceneActionType.Background:
                    bgImage = Util.ParseEnum<AtlasName>(action.content);
                    action = actions.Dequeue();
                    break;
                // Récupération du texte à afficher
                case CutsceneActionType.Text:
                    _text = Util.ParseDialog(action.content);
                    _size = Vector2.Zero;

                    // On calcule la taille une seule fois lorsqu'on récupère le fichier
                    if (_indString != 0 || _indCharacter != 0)
                    {
                        //Calcul de la taille totale du texte à afficher
                        for (int i = 0; i < _text.Count; i++)
                        {
                            //Calcul hauteur
                            _size.Y += (int)(Util.font.MeasureString(_text[i].Item2).Y * scale);

                            //Calcul du maximum de la largeur
                            if ((int)(Util.font.MeasureString(_text[i].Item2).X * scale) > _size.X)
                                _size.X = (int)(Util.font.MeasureString(_text[i].Item2).X * scale);
                        }
                    }

                    // Récupération de la prochaine action si tout le texte est affiché
                    if (_indCharacter == _text[_indString].Item2.Length && _indString == _text.Count - 1)
                        action = actions.Dequeue();

                    break;
                case CutsceneActionType.NewPage:
                    //Attente de l'appuie du boutton pour changer d'action
                    if (gstate.ksn.IsKeyDown(Keys.Space) && gstate.ksn.IsKeyDown(Keys.Space))
                    {
                        _indString = 0;
                        _indCharacter = 0;
                        action = actions.Dequeue();
                        _text.Clear();
                    }
                    break;
                case CutsceneActionType.Wait:
                    // On attend le nombre de frame présent dans le wait
                    _deltaFrame += 1;
                    if (_deltaFrame == Int32.Parse(action.content))
                    {
                        _deltaFrame = 0;
                        action = actions.Dequeue();
                    }
                    break;
                case CutsceneActionType.Sfx:
                    break;
                case CutsceneActionType.Bgm:
                    SoundManager.PlayBGM(action.content);
                    action = actions.Dequeue();
                    break;
                case CutsceneActionType.State:
                    // On lance le fading
                    Util.fadingOut = true;
                    // Lorsque le fading est fini, on change d'état
                    if (Util.fadingOpacity > 240)
                    {
                        Reset();
                        return Util.ParseEnum<State>(action.content);
                    }
                    break;
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
            Rendering.BeginDraw(spriteBatch);

            // Dessin d'un fond noir
            spriteBatch.Draw(Util.GetTexture(GraphicsDevice, BackgroundTexture, Color.Black),
                Vector2.Zero, Color.Black);

            // Dessin de l'artwork
            var texture = am.GetAtlas(bgImage).Texture;
            spriteBatch.Draw(texture, Vector2.Zero, Color.White);

            // Dessin d'un foreground avec de l'opacité
            spriteBatch.Draw(Util.GetTexture(GraphicsDevice, ForegroundTexture, new Color(0, 0, 0, 120)), Vector2.Zero, Color.White);

            // On affiche un texte si il y en a
            if (_text.Count != 0)
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
            Vector2 positionStr = new Vector2(Rendering.VirtualWidth / 2 - _size.X / 2,
            Rendering.VirtualHeight / 2 - _size.Y / 2);

            Vector2 offset = Vector2.Zero;

            // Affichage des blocs de textes complets
            for (int i = 0; i < _indString; i++)
            {
                spriteBatch.DrawString(Util.font,
                _text[i].Item2,
                positionStr + offset,
                Util.ColorStringDictionary[_text[i].Item1],
                0.0f, Vector2.Zero, scale, new SpriteEffects(), 0.0f);

                // On modifie l'offset en fonction de la taille du texte déjà affiché
                offset.Y += Util.font.MeasureString(_text[i].Item2).Y * scale;
            }

            // On affiche le texte du bloc non complet
            spriteBatch.DrawString(Util.font,
            _text[_indString].Item2.Substring(0, _indCharacter),
            positionStr + offset,
            Util.ColorStringDictionary[_text[_indString].Item1],
            0.0f, Vector2.Zero, scale, new SpriteEffects(), 0.0f);

            _counter++;

            // On incrémente les indices de charactères et de chaines
            if (_counter == _frameRefresh)
            {
                if (_indCharacter == _text[_indString].Item2.Length && _indString < _text.Count - 1)
                {
                    _indString++;
                    _indCharacter = 0;
                }
                else if (_indCharacter != _text[_indString].Item2.Length)
                {
                    _indCharacter++;
                }
                _counter = 0;
            }
        }

        /// <summary>
        /// Remise à zéro des variables
        /// </summary>
        private static void Reset()
        {
            _deltaFrame = 0;
            _indString = 0;
            _indCharacter = 0;
            _text.Clear();

            Util.fadingIn = true;
            Util.fadingOut = false;
        }
    }
}