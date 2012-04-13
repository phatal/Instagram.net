/**
 * Instagram.net
 * https://github.com/powerinbox/Instagram.net
 *
 * based on .NET-Instagram-API-Wrapper
 * Copyright (c) 2011 Diego Trin­cia­relli
 *
 * Additions are:
 * Copyright (c) 2012 IO Revolution Inc.
 */

using System;
using Instagram.Api.Utils;
using Newtonsoft.Json;

namespace Instagram.Api.Classes
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