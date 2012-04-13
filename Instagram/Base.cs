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
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace Instagram.Api {
	public class Base {
		protected static ICache _cache;
		protected static readonly object threadlock = new object();

		public static T DeserializeObject<T>(string json) {
			return JsonConvert.DeserializeObject<T>(json);
		}

		public static string SerializeObject(object value) {
			return JsonConvert.SerializeObject(value);
		}

		public string RequestPostToUrl(string url, NameValueCollection postData, WebProxy proxy) {
			if(string.IsNullOrEmpty(url))
				return null;

			if(url.IndexOf("://", StringComparison.Ordinal) <= 0)
				url = "http://" + url.Replace(",", ".");

			try {
				using(var client = new WebClient()) {
					//proxy
					if(proxy != null)
						client.Proxy = proxy;

					//response
					byte[] response = client.UploadValues(url, postData);
					//out
					var enc = new UTF8Encoding();
					string outp = enc.GetString(response);
					return outp;
				}
			}
			catch(WebException ex) {
				string err = ex.Message;
			}
			catch(Exception ex) {
				string err = ex.Message;
			}

			return null;
		}

		public string RequestDeleteToUrl(string url, WebProxy proxy) {
			if(string.IsNullOrEmpty(url))
				return null;

			if(url.IndexOf("://", StringComparison.Ordinal) <= 0)
				url = "http://" + url.Replace(",", ".");

			try {
				var request = WebRequest.Create(url);

				//proxy
				if(proxy != null)
					request.Proxy = proxy;

				//type
				request.Method = "DELETE";

				//response
				var str = "";
				var resp = request.GetResponse();
				using (var receiveStream = resp.GetResponseStream())
				{
					var encode = Encoding.GetEncoding("utf-8");
					if (receiveStream != null)
					{
						var readStream = new StreamReader(receiveStream, encode);
						var read = new Char[256];
						var count = readStream.Read(read, 0, 256);
						while(count > 0) {
							str = str + new String(read, 0, count);
							count = readStream.Read(read, 0, 256);
						}
						readStream.Close();

						receiveStream.Close();
					}
				}

				//out
				return str;
			}
			catch(WebException ex) {
				string err = ex.Message;
			}
			catch(Exception ex) {
				string err = ex.Message;
			}

			return null;
		}

		public string RequestGetToUrl(string url, WebProxy proxy) {
			if(string.IsNullOrEmpty(url))
				return null;

			if(url.IndexOf("://", StringComparison.Ordinal) <= 0)
				url = "http://" + url.Replace(",", ".");

			try {
				using(var client = new WebClient()) {
					//proxy
					if(proxy != null)
						client.Proxy = proxy;

					//response
					byte[] response = client.DownloadData(url);
					//out
					var enc = new UTF8Encoding();
					string outp = enc.GetString(response);
					return outp;
				}
			}
			catch(WebException ex) {
				string err = ex.Message;
			}
			catch(Exception ex) {
				string err = ex.Message;
			}

			return null;
		}
	}
}
