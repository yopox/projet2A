using System;
using Microsoft.Xna.Framework.Content;

namespace mono.core
{
    public class TilemapLoader : ContentTypeReader<string>
    {
        protected override string Read(ContentReader input, string existingInstance)
        {
            return input.ReadString();
        }
    }
}
