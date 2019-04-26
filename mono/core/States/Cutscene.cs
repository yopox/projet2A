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
        static CutsceneAction nextAction; // Prochaine action à effectuer

        static AtlasName bgImage; // Image de fond de la cinématique

        static private List<Tuple<string, string>> _text; // Texte à afficher
        static float scale = 2f; // Niveau de zoom de la police d'écriture
        static private Vector2 _size; // Taille du texte à afficher
        static private int _indString = 0; // Indice du texte dans _text jusqu'au quel on affiche 
        static private int _indCharacter = 0; // Indice du charactère jusqu'au quel on affiche dans _text
        static private int _counter = 0; // Compteur d'affichage des lettres
        static private int _frameRefresh = 4; // Vitesse d'affichage des lettres en frame

        static private int _deltaTime = 0;

        /// <summary>
        /// On récupère le script
        /// </summary>
        /// <param name="path">Chemin du script</param>
        public static void Load(string path)
        {
            // actions = Util.ParseScript(path);
            actions = new Queue<CutsceneAction>();
            actions.Enqueue(new CutsceneAction(CutsceneActionType.Text,
                "[w]Salut !"));
            actions.Enqueue(new CutsceneAction(CutsceneActionType.NewPage, ""));
            actions.Enqueue(new CutsceneAction(CutsceneActionType.Text,
                "[w]Ligne 2"));
            actions.Enqueue(new CutsceneAction(CutsceneActionType.Wait, "5000"));
            actions.Enqueue(new CutsceneAction(CutsceneActionType.State, "Main"));

            //On récupère la prochaine action
            nextAction = actions.Dequeue();
        }
        /// <summary>
        /// On met à jour la prochaine action de la cutscene
        /// </summary>
        /// <param name="gstate">On récupère l'état des controleurs</param>
        public static State Update(GameState gstate, GameTime gameTime)
        {
            switch (nextAction.type)
            {
                case CutsceneActionType.Background:
                    bgImage = Util.ParseEnum<AtlasName>(nextAction.content);
                    break;
                case CutsceneActionType.Text:
                    _text = Util.ParseDialog(nextAction.content);
                    _size = Vector2.Zero;

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

                    if(_indCharacter == _text[_indString].Item2.Length && _indString == _text.Count - 1)
                    {
                        //TODO : enlever le compte
                        if(actions.Count != 0)
                            nextAction = actions.Dequeue();
                    }

                    break;
                case CutsceneActionType.NewPage:
                    //Attente de l'appuie du boutton pour changer d'action
                    if (gstate.ksn.IsKeyDown(Keys.Space) && gstate.ksn.IsKeyDown(Keys.Space))
                    {
                        _indString = 0;
                        _indCharacter = 0;
                        nextAction = actions.Dequeue();
                    }
                    break;
                case CutsceneActionType.Wait:
                    _deltaTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if(_deltaTime > Int32.Parse(nextAction.content))
                    {
                        _deltaTime = 0;
                        //TODO : enlever le compte
                        if (actions.Count != 0)
                            nextAction = actions.Dequeue();
                    }
                    break;
                case CutsceneActionType.Sfx:
                    break;
                case CutsceneActionType.Bgm:
                    break;
                case CutsceneActionType.State:
                    return Util.ParseEnum<State>(nextAction.content);
            }
            return State.Cutscene;
        }

        public static void Draw(SpriteBatch spriteBatch, AssetManager am, GraphicsDevice GraphicsDevice)
        {
            Rendering.BeginDraw(spriteBatch);
            spriteBatch.Draw(Util.GetRectangleTexture(GraphicsDevice, Color.Black, Rendering.VirtualWidth, Rendering.VirtualHeight),
                Vector2.Zero, Color.Black);
            var texture = am.GetAtlas(bgImage).Texture;
            //spriteBatch.Draw(texture, Vector2.Zero, Color.White);

            if (_text != null)
                DrawDialog(spriteBatch, GraphicsDevice);
        }

        static void DrawDialog(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice)
        {

            Vector2 positionStr = new Vector2(Rendering.VirtualWidth / 2 - _size.X / 2,
                Rendering.VirtualHeight / 2 - _size.Y / 2);

            Vector2 offset = Vector2.Zero;

            for (int i = 0; i < _indString; i++)
            {
                spriteBatch.DrawString(Util.font,
                _text[i].Item2,
                positionStr + offset,
                Util.ColorStringDictionary[_text[i].Item1],
                0.0f, Vector2.Zero, scale, new SpriteEffects(), 0.0f);

                offset.Y += Util.font.MeasureString(_text[i].Item2).Y * scale;
            }

            Console.WriteLine(_indString);
            Console.WriteLine(_indCharacter);
            spriteBatch.DrawString(Util.font,
            _text[_indString].Item2.Substring(0, _indCharacter),
            positionStr + offset,
            Util.ColorStringDictionary[_text[_indString].Item1],
            0.0f, Vector2.Zero, scale, new SpriteEffects(), 0.0f);

            _counter++;

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
    }
}
