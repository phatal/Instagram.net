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
	public class ImagesList : InstagramBaseObject
	{
		[JsonProperty(PropertyName = "low_resolution")]
		public ImageLink LowResolution { get; set; }

		[JsonProperty(PropertyName = "thumbnail")]
		public ImageLink Thumbnail { get; set; }

		[JsonProperty(PropertyName = "standard_resolution")]
		public ImageLink StandardResolution { get; set; }
	}
}