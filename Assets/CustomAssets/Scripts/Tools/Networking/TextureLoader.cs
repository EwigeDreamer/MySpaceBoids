using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Singleton;
using UnityEngine.Networking;
using System.Threading.Tasks;

namespace MyTools.Network
{
    public static class TextureLoader
    {
        public static Coroutine LoadTexture(string url, System.Action<Texture2D> onLoad)
            => MyTools.Helpers.CorouWaiter.Start(LoadTextureRoutine(url, onLoad));
        static IEnumerator LoadTextureRoutine(string url, System.Action<Texture2D> onLoad)
        {
            if (onLoad == null) throw new System.ArgumentException("Parameter can't be null.", "onLoad");
            using (var www = UnityWebRequestTexture.GetTexture(url))
            {
                yield return www.SendWebRequest();
                if (www.isNetworkError || www.isHttpError)
                    Debug.LogError(www.error);
                else
                {
                    Texture2D tex = DownloadHandlerTexture.GetContent(www);
                    onLoad(tex);
                }
            }
            yield break;
        }
    }

    public static class TextureDataLoader
    {
        public static Coroutine LoadTextureData(string url, System.Action<TextureData> onLoad)
            => MyTools.Helpers.CorouWaiter.Start(LoadTextureDataRoutine(url, onLoad));
        static IEnumerator LoadTextureDataRoutine(string url, System.Action<TextureData> onLoad)
        {
            Texture2D tex = null;
            yield return TextureLoader.LoadTexture(url, a => tex = a);
            if (tex != null) onLoad(new TextureData(tex));
            else Debug.LogWarning("Texture was not loaded!");
            yield break;
        }
    }

    public class TextureData : System.IDisposable
    {
        Texture2D Texture { get; }
        public TextureData(Texture2D tex)
        {
            Texture = tex;
        }

        public virtual void Dispose()
        {
            Resources.UnloadAsset(Texture);
        }
    }

    public class SpriteData : TextureData
    {
        Sprite Sprite { get; }
        public SpriteData(Texture2D tex) : base(tex)
        {
            var rect = new Rect(0f, 0f, tex.width, tex.height);
            var pivot = new Vector2(0.5f, 0.5f);
            Sprite = Sprite.Create(tex, rect, pivot, 100f);
        }

        public override void Dispose()
        {
            Resources.UnloadAsset(Sprite);
            base.Dispose();
        }
    }
}