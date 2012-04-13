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

namespace Instagram.Api.Classes
{
	[Serializable]
	public class Counts : InstagramBaseObject
	{
		[JsonProperty(PropertyName = "media")]
		public int Media { get; set; }

		[JsonProperty(PropertyName = "follows")]
		public int Follows { get; set; }

		[JsonProperty(PropertyName = "followed_by")]
		public int FollowedBy { get; set; }
	}
}
