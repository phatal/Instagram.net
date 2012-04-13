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
		[JsonProperty(PropertyName = "comments")]
		public CommentList Comments { get; set; }

		[JsonProperty(PropertyName = "caption")]
		public Caption Caption { get; set; }

		[JsonProperty(PropertyName = "likes")]
		public LikesList Likes { get; set; }

		[JsonProperty(PropertyName = "link")]
		public string Link { get; set; }

		[JsonProperty(PropertyName = "user")]
		public User User;

		[JsonProperty(PropertyName = "created_time")]
		[JsonConverter(typeof(InstagramDateConverter))]
		public DateTime CreatedTime { get; set; }

		[JsonProperty(PropertyName = "images")]
		public ImagesList Images { get; set; }

		[JsonProperty(PropertyName = "type")]
		public string Type { get; set; }

		[JsonProperty(PropertyName = "filter")]
		public string Filter { get; set; }

		[JsonProperty(PropertyName = "tags")]
		public string[] Tags { get; set; }

		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }

		[JsonProperty(PropertyName = "location")]
		public Location Location { get; set; }

		[JsonProperty(PropertyName = "user_has_liked")]
		public bool UserHasLiked { get; set; }
	}
}