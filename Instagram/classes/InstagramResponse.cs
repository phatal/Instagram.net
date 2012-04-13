﻿/**
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
	public class InstagramResponse<T> {
		public Pagination pagination;
		public Metadata meta;
		public T data;
	}
}
