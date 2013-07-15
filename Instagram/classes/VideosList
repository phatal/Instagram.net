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
	public class VideosList : InstagramBaseObject
	{
		[JsonProperty(PropertyName = "low_resolution")]
		public VideoLink LowResolution { get; set; }

		[JsonProperty(PropertyName = "thumbnail")]
		public VideoLink Thumbnail { get; set; }

		[JsonProperty(PropertyName = "standard_resolution")]
		public VideoLink StandardResolution { get; set; }
	}
}
