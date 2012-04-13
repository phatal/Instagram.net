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
	public class AccessToken : InstagramBaseObject
	{
		[JsonProperty(PropertyName = "access_token")]
		public string Token { get; set; }

		[JsonProperty(PropertyName = "user")]
		public User User { get; set; }

		public AccessToken()
		{
		}

		public AccessToken(string json) {
			var tk = Deserialize(json);
			Token = tk.Token;
			User = tk.User;
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
