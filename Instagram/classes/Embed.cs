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
	public class Embed : InstagramBaseObject
	{
		[JsonProperty(PropertyName = "provider_url")]
		public string ProviderUrl { get; set; }

		[JsonProperty(PropertyName = "title")]
		public string Title { get; set; }

		[JsonProperty(PropertyName = "url")]
		public string Url { get; set; }

		[JsonProperty(PropertyName = "author_name")]
		public string AuthorName { get; set; }

		[JsonProperty(PropertyName = "media_id")]
		public string MediaId { get; set; }

		[JsonProperty(PropertyName = "author_id")]
		public int AuthorId { get; set; }

		[JsonProperty(PropertyName = "height")]
		public int Height { get; set; }

		[JsonProperty(PropertyName = "width")]
		public int Width { get; set; }

		[JsonProperty(PropertyName = "version")]
		public string Version { get; set; }

		[JsonProperty(PropertyName = "author_url")]
		public string AuthorUrl { get; set; }

		[JsonProperty(PropertyName = "provider_name")]
		public string ProviderName { get; set; }

		[JsonProperty(PropertyName = "type")]
		public string Type { get; set; }
	}
}
