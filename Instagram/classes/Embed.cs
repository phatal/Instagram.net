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

namespace Instagram.Api.Classes
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
