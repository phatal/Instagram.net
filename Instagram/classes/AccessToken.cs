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
	public class AccessToken : InstagramBaseObject
	{
		public string access_token;
		public User user;

		public AccessToken()
		{
		}

		public AccessToken(string json) {
			var tk = Deserialize(json);
			access_token = tk.access_token;
			user = tk.user;
		}

		public string GetJson() {
			return Serialize(this);
		}

		public static string Serialize(AccessToken token) {
			var json =  Base.SerializeObject(token);
			return json;
		}

		public static AccessToken Deserialize(string json) {
			var token = Base.DeserializeObject<AccessToken>(json);
			return token;
		}
	}
}
