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

namespace Instagram.Api.Classes {
	[Serializable]
	public class Caption : InstagramBaseObject
	{
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }

		[JsonProperty(PropertyName = "created_time")]
		[JsonConverter(typeof (InstagramDateConverter))]
		public DateTime CreatedTime { get; set; }

		[JsonProperty(PropertyName = "text")]
		public string Text { get; set; }

		[JsonProperty(PropertyName = "from")]
		public User From { get; set; }
	}
}