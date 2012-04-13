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

namespace Instagram.Api.Classes
{
	[System.Serializable]
	public class InstagramBaseObject {
		protected InstagramApiWrapper InstagramApi { get { return InstagramApiWrapper.GetInstance(); } }
	}
}
