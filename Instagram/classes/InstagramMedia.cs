using System;
using Instagram.api.Utils;
using Newtonsoft.Json;

namespace Instagram.api.classes
{

    [Serializable]
    public class InstagramMedia : InstagramBaseObject
    {

        public CommentList comments;
        public Caption caption;
        public LikesList likes;
        public string link;
        public User user;

        [JsonProperty(PropertyName = "created_time")]
        [JsonConverter(typeof(InstagramDateConverter))]
        public DateTime CreatedTime { get; set; }

        public ImagesList images;
        public string type;
        public string filter;
        public string[] tags;
        public string id;
        public Location location;
        public bool user_has_liked;

    }
}