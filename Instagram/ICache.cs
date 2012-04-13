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

namespace Instagram.Api
{
	public interface ICache {
		void Add(string key, object obj);
		void Add(string key, object obj, int timeout);
		void Remove(string key);
		object Get(string key);
		T Get<T>(string key);
		bool Exists(string key);
	}
}
