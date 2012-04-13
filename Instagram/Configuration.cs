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

using System.Net;

namespace Instagram.Api
{
	public class Configuration
	{
		public string AuthUrl { get; set; }
		public string ClientId { get; set; }
		public string ClientSecret { get; set; }
		public string ReturnUrl { get; set; }
		public string TokenRetrievalUrl { get; set; }
		public string ApiBaseUrl { get; set; }
		public string OEmbedUrl { get; set; }
		public string WebProxy { get; set; }

		public WebProxy Proxy {
			get {
				if (!string.IsNullOrEmpty(WebProxy))
				{
					var pcs = WebProxy.Split(new[] { ':' });
					return new WebProxy(pcs[0], int.Parse(pcs[1]));
				}
				return null;
			}
		}

		public Configuration() {
		}

		public Configuration(string authurl,string clientid,string clientsecret, string returnurl, string tokenretrievalurl,string apibaseurl, string webconfig, string oembedUrl) {
			AuthUrl = authurl;
			ClientId =clientid;
			ClientSecret=clientsecret;
			ReturnUrl=returnurl;
			TokenRetrievalUrl=tokenretrievalurl;
			ApiBaseUrl=apibaseurl;
			WebProxy = webconfig;
			OEmbedUrl = oembedUrl;
		}
	}
}
