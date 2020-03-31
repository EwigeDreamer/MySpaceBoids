namespace MyTools.Extensions.Texture
{
    using System.Collections.Generic;
    using UnityEngine;
    using System;
    public static class TextureEx
    {
        public static Texture2D ToTexture2D(this Texture texture)
        {
            return Texture2D.CreateExternalTexture(
                texture.width,
                texture.height,
                TextureFormat.RGB24,
                false, false,
                texture.GetNativeTexturePtr());
        }
    }
}
