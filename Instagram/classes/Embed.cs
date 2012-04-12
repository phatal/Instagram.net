
using System;

namespace Instagram.api.classes
{
    [Serializable]
    public class Embed : InstagramBaseObject
    {
        public string provider_url;
        public string title;
        public string url;
        public string author_name;
        public string media_id;
        public int author_id;
        public int height;
        public int width;
        public string version;
        public string author_url;
        public string provider_name;
        public string type;
    }
}
