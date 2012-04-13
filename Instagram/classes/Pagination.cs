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
using Newtonsoft.Json;

namespace Instagram.Api.Classes{
	[Serializable]
	public class Pagination : InstagramBaseObject {

		[JsonProperty(PropertyName = "next_url")]
		public string NextUrl { get; set; }

		[JsonProperty(PropertyName = "next_max_id")]
		public string NextMaxId { get; set; }

		[JsonProperty(PropertyName = "next_max_like_id")]
		public string NextMaxLikeId { get; set; }
	}
}