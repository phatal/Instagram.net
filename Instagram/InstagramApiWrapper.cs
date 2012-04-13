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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Instagram.Api.Classes;

namespace Instagram.Api
{
	public class InstagramApiWrapper : Base
	{

		private static InstagramApiWrapper _sharedInstance;
		private static Configuration _sharedConfiguration;

		public Configuration Configuration {
			get { return _sharedConfiguration; }
			set { _sharedConfiguration = value; }
		}

		private InstagramApiWrapper() {
		}

		public static InstagramApiWrapper GetInstance(Configuration configuration, ICache cache)
		{
			lock (threadlock)
			{
				if (_sharedInstance == null)
				{
					_sharedInstance = new InstagramApiWrapper();
					_cache = cache;
					_sharedInstance.Configuration = configuration;
				}
			}

			return _sharedInstance;
		}

		public static InstagramApiWrapper GetInstance(Configuration configuration) {
			lock (threadlock) {
				if(_sharedInstance == null)
					_sharedInstance = new InstagramApiWrapper {Configuration = configuration};
			}

			return _sharedInstance;
		}

		public static InstagramApiWrapper GetInstance() {
			if (_sharedInstance == null) {
				if (_sharedConfiguration == null)
					throw new ApplicationException("API Uninitialized");

				_sharedInstance = new InstagramApiWrapper();
			}
			return _sharedInstance;
		}

		#region Authentication

		public string AuthGetUrl(string scope){
			if (string.IsNullOrEmpty(scope))
				scope = "basic";

			return Configuration.AuthUrl + "?client_id=" + Configuration.ClientId + "&redirect_uri=" + Configuration.ReturnUrl + "&response_type=code&scope=" + scope;
		}

		public AccessToken AuthGetAccessToken(string code)
		{
			var json = RequestPostToUrl(Configuration.TokenRetrievalUrl, new NameValueCollection
										{
												{"client_id" , Configuration.ClientId},
												{"client_secret" , Configuration.ClientSecret},
												{"grant_type" , "authorization_code"},
												{"redirect_uri" , Configuration.ReturnUrl},
												{"code" , code}
										},
										Configuration.Proxy);

			return string.IsNullOrEmpty(json) ? null : AccessToken.Deserialize(json);
		}

		#endregion

		#region User

		public InstagramResponse<User> UserDetails(string userid, string accessToken)
		{
			if (userid == "self")
				return CurrentUserDetails(accessToken);

			var url = Configuration.ApiBaseUrl + "users/" + userid + "?access_token=" + accessToken;
			if (string.IsNullOrEmpty(accessToken))
				url = Configuration.ApiBaseUrl + "users/" + userid + "?client_id=" + Configuration.ClientId;

			if (_cache != null)
				if (_cache.Exists(url))
					return _cache.Get<InstagramResponse<User>>("users/" + userid);

			var json = RequestGetToUrl(url,Configuration.Proxy);
			if (string.IsNullOrEmpty(json))
				return null;

			var res = DeserializeObject<InstagramResponse<User>>(json);

			if (!string.IsNullOrEmpty(accessToken)) {
				//CurrentUserIsFollowing(userid, accessToken);

				res.Data.IsFollowed = CurrentUserIsFollowing(res.Data.Id,accessToken);
			}

			if (_cache != null)
				_cache.Add("users/" + userid, res, 600);

			return res;
		}

		public InstagramResponse<User[]> UsersSearch(string query, string count, string accessToken)
		{
			var url = Configuration.ApiBaseUrl + "users/search?access_token=" + accessToken;
			if (string.IsNullOrEmpty(accessToken))
				url = Configuration.ApiBaseUrl + "users/search?client_id=" + Configuration.ClientId;

			if (_cache != null)
				if (_cache.Exists(url))
					return _cache.Get<InstagramResponse<User[]>>(url);

			if (!string.IsNullOrEmpty(query)) url = url + "&q=" + query;
			if (!string.IsNullOrEmpty(count)) url = url + "&count=" + count;
			var json = RequestGetToUrl(url, Configuration.Proxy);
			if (string.IsNullOrEmpty(json))
				return null;

			var res = DeserializeObject<InstagramResponse<User[]>>(json);

			if (_cache != null)
				_cache.Add(url, res,300);

			return res;
		}

		public User[] UsersPopular(string accessToken)
		{
			var media = MediaPopular(accessToken,true).Data;
			return UsersInMediaList(media);
		}

		public InstagramResponse<InstagramMedia[]> UserRecentMedia(string userid, string min_id, string max_id, string count, string min_timestamp, string max_timestamp, string accessToken)
		{
			var url = Configuration.ApiBaseUrl + "users/" + userid + "/media/recent?access_token=" + accessToken;
			if (string.IsNullOrEmpty(accessToken))
				url = Configuration.ApiBaseUrl + "users/" + userid + "/media/recent?client_id=" + Configuration.ClientId;

			if (!string.IsNullOrEmpty(min_id))
				url = url + "&min_id=" + min_id;

			if (!string.IsNullOrEmpty(max_id))
				url = url + "&max_id=" + max_id;

			if (!string.IsNullOrEmpty(count))
				url = url + "&count=" + count;

			if (!string.IsNullOrEmpty(min_timestamp))
				url = url + "&min_timestamp=" + min_timestamp;

			if (!string.IsNullOrEmpty(max_timestamp))
				url = url + "&max_timestamp=" + max_timestamp;

			var json = RequestGetToUrl(url, Configuration.Proxy);

			return string.IsNullOrEmpty(json) ? null : DeserializeObject<InstagramResponse<InstagramMedia[]>>(json);
		}

		public InstagramResponse<User> CurrentUserDetails(string accessToken)
		{
			var url = Configuration.ApiBaseUrl + "users/self?access_token=" + accessToken;

			if (_cache != null)
				if (_cache.Exists("users/self/" + accessToken))
					return _cache.Get<InstagramResponse<User>>("users/self/" + accessToken);

			var json = RequestGetToUrl(url, Configuration.Proxy);
			if (string.IsNullOrEmpty(json))
				return null;

			var res = DeserializeObject<InstagramResponse<User>>(json);
			res.Data.IsSelf = true;

			if (_cache != null)
				_cache.Add("users/self/" + accessToken, res, 600);

			return res;
		}

		public InstagramResponse<InstagramMedia[]> CurrentUserRecentMedia(string min_id, string max_id, string count, string min_timestamp, string max_timestamp, string accessToken)
		{
			return UserRecentMedia("self", min_id, max_id, count, min_timestamp, max_timestamp, accessToken);
		}

		public InstagramResponse<InstagramMedia[]> CurrentUserFeed(string min_id, string max_id, string count, string accessToken)
		{
			var url = Configuration.ApiBaseUrl + "users/self/feed?access_token=" + accessToken;

			if (!string.IsNullOrEmpty(min_id)) url = url + "&min_id=" + min_id;
			if (!string.IsNullOrEmpty(max_id)) url = url + "&max_id=" + max_id;
			if (!string.IsNullOrEmpty(count)) url = url + "&count=" + count;

			var json = RequestGetToUrl(url, Configuration.Proxy);

			return string.IsNullOrEmpty(json) ? null : DeserializeObject<InstagramResponse<InstagramMedia[]>>(json);
		}

		public InstagramResponse<InstagramMedia[]> CurrentUserLikedMedia(string max_like_id, string count, string accessToken)
		{
			var url = Configuration.ApiBaseUrl + "users/self/media/liked?access_token=" + accessToken;
			if (!string.IsNullOrEmpty(max_like_id)) url = url + "&max_like_id=" + max_like_id;
			if (!string.IsNullOrEmpty(count)) url = url + "&count=" + count;

			var json = RequestGetToUrl(url, Configuration.Proxy);

			return string.IsNullOrEmpty(json) ? null : DeserializeObject<InstagramResponse<InstagramMedia[]>>(json);
		}

		public User[] UsersInMediaList(InstagramMedia[] media)
		{
			var users = new List<User>();

			foreach (var instagramMedia in media.Where(instagramMedia => !users.Contains(instagramMedia.User)))
				users.Add(instagramMedia.User);

			return users.ToArray();
		}

		#endregion

		#region Relationships

		public InstagramResponse<User[]> UserFollows(string userid, string accessToken, string max_user_id)
		{
			var url = Configuration.ApiBaseUrl + "users/" + userid + "/follows?access_token=" + accessToken;
			if (string.IsNullOrEmpty(accessToken))
				url = Configuration.ApiBaseUrl + "users/" + userid + "/follows?client_id=" + Configuration.ClientId;

			if (!string.IsNullOrEmpty(max_user_id)) url = url + "&cursor=" + max_user_id;

			//if(_cache!=null)
			//	if (_cache.Exists(userid + "/follows"))
			//		return _cache.Get<InstagramResponse<User[]>>(userid + "/follows");

			var json = RequestGetToUrl(url, Configuration.Proxy);
			if (string.IsNullOrEmpty(json))
				return null;

			var res = DeserializeObject<InstagramResponse<User[]>>(json);
			//https://api.instagram.com/v1/users/530914/follows?access_token=530914.0c0b99a.56e7a173b9af43eba8a60759904f6fc4&cursor=32754039"
			//if (_cache != null)
			//	_cache.Add(userid + "/follows", res);

			return res;
		}

		public InstagramResponse<User[]> UserFollowedBy(string userid, string accessToken, string max_user_id)
		{
			var url = Configuration.ApiBaseUrl + "users/" + userid + "/followed-by?access_token=" + accessToken;
			if (string.IsNullOrEmpty(accessToken))
				url = Configuration.ApiBaseUrl + "users/" + userid + "/followed-by?client_id=" + Configuration.ClientId;

			if (!string.IsNullOrEmpty(max_user_id)) url = url + "&cursor=" + max_user_id;

			//if (_cache != null)
			//	if (_cache.Exists(userid + "/followed-by"))
			//		return _cache.Get<InstagramResponse<User[]>>(userid + "/followed-by");

			var json = RequestGetToUrl(url, Configuration.Proxy);
			if (string.IsNullOrEmpty(json))
				return null;

			var res = DeserializeObject<InstagramResponse<User[]>>(json);

			//if (_cache != null)
			//	_cache.Add(userid + "/followed-by", res);

			return res;
		}

		public InstagramResponse<User[]> CurrentUserFollows(string accessToken, string max_user_id)
		{
			var url = Configuration.ApiBaseUrl + "users/self/follows?access_token=" + accessToken;

			if (!string.IsNullOrEmpty(max_user_id))
				url = url + "&cursor=" + max_user_id;

			var json = RequestGetToUrl(url, Configuration.Proxy);

			return string.IsNullOrEmpty(json) ? null : DeserializeObject<InstagramResponse<User[]>>(json);
		}

		public InstagramResponse<User[]> CurrentUserFollowedBy(string accessToken, string max_user_id)
		{
			var url = Configuration.ApiBaseUrl + "users/self/followed-by?access_token=" + accessToken;

			if (!string.IsNullOrEmpty(max_user_id)) url = url + "&cursor=" + max_user_id;

			var json = RequestGetToUrl(url, Configuration.Proxy);

			return string.IsNullOrEmpty(json) ? null : DeserializeObject<InstagramResponse<User[]>>(json);
		}

		public InstagramResponse<User[]> CurrentUserRequestedBy(string accessToken)
		{
			var url = Configuration.ApiBaseUrl + "users/self/requested-by?access_token=" + accessToken;

			var json = RequestGetToUrl(url, Configuration.Proxy);

			return string.IsNullOrEmpty(json) ? null : DeserializeObject<InstagramResponse<User[]>>(json);
		}

		public InstagramResponse<Relation> CurrentUserRelationshipWith(string recipient_userid, string accessToken)
		{
			var url = Configuration.ApiBaseUrl + "users/" + recipient_userid + "/relationship?access_token=" + accessToken;
			var json = RequestGetToUrl(url, Configuration.Proxy);

			return string.IsNullOrEmpty(json) ? null : DeserializeObject<InstagramResponse<Relation>>(json);
		}

		public void CurrentUserFollow(string userid, string[] recipient_userids, string accessToken)
		{
			foreach (var recipientUserid in recipient_userids)
				CurrentUserFollow(userid,recipientUserid, accessToken);
		}

		public bool CurrentUserFollow(string userid,string recipient_userid, string accessToken)
		{
			if (_cache != null)
			{
				_cache.Remove(userid + "/follows");
				_cache.Remove("users/self/" + accessToken);
			}

			return CurrentUserSetRelationship(userid, recipient_userid, "follow", accessToken).Meta.Code == "200";
		}

		public bool CurrentUserFollowToggle(string userid, string recipient_userid, string accessToken)
		{
			if (_cache != null) {
				_cache.Remove("self" + "/follows");
				_cache.Remove(userid + "/follows");
				_cache.Remove("users/" + recipient_userid);
				_cache.Remove("users/self/" + accessToken);
			}

			if (CurrentUserIsFollowing(recipient_userid,accessToken))
				return CurrentUserSetRelationship(userid,recipient_userid, "unfollow", accessToken).Meta.Code == "200";

			return CurrentUserSetRelationship(userid,recipient_userid, "follow", accessToken).Meta.Code == "200";
		}

		public void CurrentUserUnfollow(string userid, string[] recipient_userids, string accessToken)
		{
			foreach (var recipientUserid in recipient_userids)
			{
				CurrentUserUnfollow(userid, recipientUserid, accessToken);
			}

			if (_cache != null)
				_cache.Remove(userid + "/follows");
		}

		public bool CurrentUserUnfollow(string userid, string recipient_userid, string accessToken)
		{
			if (_cache != null)
			{
				_cache.Remove(userid + "/follows");
				_cache.Remove("users/self/" + accessToken);
			}

			return CurrentUserSetRelationship(userid,recipient_userid, "unfollow", accessToken).Meta.Code == "200";
		}

		public bool CurrentUserBlock(string userid, string recipient_userid, string accessToken)
		{
			return CurrentUserSetRelationship(userid,recipient_userid, "block", accessToken).Meta.Code == "200";
		}

		public void CurrentUserApprove(string userid, string[] recipient_userids, string accessToken)
		{
			foreach (var recipientUserid in recipient_userids)
			{
				CurrentUserApprove(userid,recipientUserid, accessToken);
			}
		}

		public bool CurrentUserApprove(string userid, string recipient_userid, string accessToken)
		{
			return CurrentUserSetRelationship(userid,recipient_userid, "approve", accessToken).Meta.Code == "200";
		}

		public bool CurrentUserDeny(string userid, string recipient_userid, string accessToken)
		{
			return CurrentUserSetRelationship(userid,recipient_userid, "deny", accessToken).Meta.Code == "200";
		}

		public bool CurrentUserUnblock(string userid, string recipient_userid, string accessToken)
		{
			return CurrentUserSetRelationship(userid,recipient_userid,"unblock",accessToken).Meta.Code == "200";
		}

		public InstagramResponse<Relation> CurrentUserSetRelationship(string userid, string recipient_userid, string relationship_key, string accessToken)
		{
			var url = Configuration.ApiBaseUrl + "users/" + recipient_userid + "/relationship?access_token=" + accessToken;
			url = url + "&action=" + relationship_key;
			var json = RequestPostToUrl(url, new NameValueCollection { { "action", relationship_key } }, Configuration.Proxy);
			if (string.IsNullOrEmpty(json)) //error
				return new InstagramResponse<Relation>{Meta = new Metadata{ Code = "400" }};

			var res = DeserializeObject<InstagramResponse<Relation>>(json);

			if (_cache != null)
			{
				_cache.Remove("users/self/" + accessToken);
				_cache.Remove(userid + "/follows");
				_cache.Remove("users/" + recipient_userid);
			}

			return res;
		}

		public bool CurrentUserIsFollowing( string recipient_userid, string accessToken)
		{
			//outgoing_status: Your relationship to the user: "follows", "requested", "none".
			//incoming_status: A user's relationship to you : "followed_by", "requested_by", "blocked_by_you", "none".
			var r = CurrentUserRelationshipWith(recipient_userid, accessToken).Data;

			return r.OutgoingStatus == "follows";
		}

		public bool CurrentUserIsFollowedBy( string recipient_userid, string accessToken)
		{
			//outgoing_status: Your relationship to the user: "follows", "requested", "none".
			//incoming_status: A user's relationship to you : "followed_by", "requested_by", "blocked_by_you", "none".
			var r = CurrentUserRelationshipWith(recipient_userid, accessToken).Data;

			return r.IncomingStatus == "followed_by";
		}

		#endregion

		#region Media

		public InstagramResponse<InstagramMedia> MediaDetails(string mediaid, string accessToken)
		{
			var url = Configuration.ApiBaseUrl + "media/" + mediaid + "?access_token=" + accessToken;
			if (string.IsNullOrEmpty(accessToken))
				url = Configuration.ApiBaseUrl + "media/" + mediaid + "?client_id=" + Configuration.ClientId;

			if (_cache != null)
				if (_cache.Exists("media/" + mediaid))
					return _cache.Get<InstagramResponse<InstagramMedia>>("media/" + mediaid);

			var json = RequestGetToUrl(url, Configuration.Proxy);
			if (string.IsNullOrEmpty(json))
				return null;

			var res = DeserializeObject<InstagramResponse<InstagramMedia>>(json);
			if (_cache != null)
				_cache.Add("media/" + mediaid, res, 60);

			return res;
		}

		public InstagramResponse<InstagramMedia[]> MediaSearch(string lat, string lng, string distance, string min_timestamp, string max_timestamp, string accessToken)
		{
			if (!string.IsNullOrEmpty(lat) && string.IsNullOrEmpty(lng) || !string.IsNullOrEmpty(lng) && string.IsNullOrEmpty(lat))
				throw new ArgumentException("if lat or lng are specified, both are required.");

			string url = Configuration.ApiBaseUrl + "media/search?access_token=" + accessToken;
			if (string.IsNullOrEmpty(accessToken))
				url = Configuration.ApiBaseUrl + "media/search?client_id=" + Configuration.ClientId;

			if (!string.IsNullOrEmpty(lat)) url = url + "&lat=" + lat;
			if (!string.IsNullOrEmpty(lng)) url = url + "&lng=" + lng;
			if (!string.IsNullOrEmpty(distance)) url = url + "&distance=" + distance;
			if (!string.IsNullOrEmpty(min_timestamp)) url = url + "&min_timestamp=" + min_timestamp;
			if (!string.IsNullOrEmpty(max_timestamp)) url = url + "&max_timestamp=" + max_timestamp;

			if (_cache != null)
				if (_cache.Exists(url))
					return _cache.Get<InstagramResponse<InstagramMedia[]>>(url);

			string json = RequestGetToUrl(url, Configuration.Proxy);
			if (string.IsNullOrEmpty(json))
				return null;

			var res = DeserializeObject<InstagramResponse<InstagramMedia[]>>(json);

			if (_cache != null)
				_cache.Add(url, res, 60);

			return res;
		}
		public InstagramResponse<InstagramMedia[]> MediaPopular(string accessToken, bool usecache)
		{
			string url = Configuration.ApiBaseUrl + "media/popular?access_token=" + accessToken;
			if(string.IsNullOrEmpty(accessToken))
				url = Configuration.ApiBaseUrl + "media/popular?client_id=" + Configuration.ClientId;

			if (_cache != null && usecache)
				if (_cache.Exists("media/popular"))
					return _cache.Get<InstagramResponse<InstagramMedia[]>>("media/popular" );

			string json = RequestGetToUrl(url, Configuration.Proxy);
			if (string.IsNullOrEmpty(json))
				return null;

			var res = DeserializeObject<InstagramResponse<InstagramMedia[]>>(json);
			if (_cache != null )
				_cache.Add("media/popular" , res, 600);

			return res;
		}

		#endregion

		#region Comments

		public InstagramResponse<Comment[]> Comments(string mediaid, string accessToken)
		{
			var url = Configuration.ApiBaseUrl + "media/" + mediaid + "/comments?access_token=" + accessToken;
			if (string.IsNullOrEmpty(accessToken))
				url = Configuration.ApiBaseUrl + "media/" + mediaid + "/comments?client_id=" + Configuration.ClientId;

			var json = RequestGetToUrl(url, Configuration.Proxy);

			return string.IsNullOrEmpty(json) ? null : DeserializeObject<InstagramResponse<Comment[]>>(json);
		}

		public void CommentAdd(string[] mediaids, string text, string accessToken)
		{
			foreach (var mediaid in mediaids)
				CommentAdd(mediaid, text,accessToken);
		}

		public bool CommentAdd(string mediaid, string text, string accessToken)
		{
			var url = Configuration.ApiBaseUrl + "media/" + mediaid + "/comments?access_token=" + accessToken;
			var post = new NameValueCollection
						{
								{"text",text},
								{"access_token", accessToken}
						};
			var json = RequestPostToUrl(url, post, Configuration.Proxy);
			if (string.IsNullOrEmpty(json))
				return true;

			var res = DeserializeObject<InstagramResponse<Comment>>(json);

			if (_cache != null)
				_cache.Remove("media/" + mediaid);

			return res.Meta.Code == "200";
		}

		public bool CommentDelete(string mediaid, string commentid, string accessToken)
		{
			var url = Configuration.ApiBaseUrl + "media/" + mediaid + "/comments/"+ commentid +"?access_token=" + accessToken;
			var json = RequestDeleteToUrl(url, Configuration.Proxy);
			if (string.IsNullOrEmpty(json))
				return false;

			var res = DeserializeObject<InstagramResponse<Comment>>(json);

			if (_cache != null)
				_cache.Remove("media/" + mediaid);

			return res.Meta.Code == "200";
		}

		#endregion

		#region Likes

		public InstagramResponse<User[]> Likes(string mediaid, string accessToken)
		{
			var url = Configuration.ApiBaseUrl + "media/" + mediaid + "/likes?access_token=" + accessToken;
			if (string.IsNullOrEmpty(accessToken))
				url = Configuration.ApiBaseUrl + "media/" + mediaid + "/likes?client_id=" + Configuration.ClientId;

			if (_cache != null)
				if (_cache.Exists("media/" + mediaid + "/likes"))
					return _cache.Get<InstagramResponse<User[]>>("media/" + mediaid + "/likes");

			var json = RequestGetToUrl(url, Configuration.Proxy);
			if (string.IsNullOrEmpty(json))
				return null;

			var res = DeserializeObject<InstagramResponse<User[]>>(json);

			if (_cache != null)
				_cache.Add("media/" + mediaid + "/likes", res, 60);

			return res;
		}

		public void LikeAdd(string[] mediaids, string accessToken) {
			foreach(var mediaid in mediaids)
				LikeAdd(mediaid,null, accessToken);
		}

		public bool LikeAdd(string mediaid,string userid, string accessToken)
		{
			var url = Configuration.ApiBaseUrl + "media/" + mediaid + "/likes?access_token=" + accessToken;
			var post = new NameValueCollection
						{
								{"access_token", accessToken}
						};
			var json = RequestPostToUrl(url, post, Configuration.Proxy);
			if (string.IsNullOrEmpty(json))
				return true;

			var res = DeserializeObject<InstagramResponse<User[]>>(json);

			if (_cache != null) {
				_cache.Remove("media/" + mediaid);
				_cache.Remove("media/popular" );
				_cache.Remove("media/" + mediaid + "/likes");
				_cache.Remove("users/self/" + accessToken);
				_cache.Remove("users/" + userid);
				if (!string.IsNullOrEmpty(userid)) {
					_cache.Remove("users/" + userid + "/media/recent");
					_cache.Remove("users/self/feed?access_token=" + accessToken);
				}
			}

			return res.Meta.Code == "200";
		}

		public void LikeDelete(string[] mediaids, string accessToken)
		{
			foreach (var mediaid in mediaids)
				LikeDelete(mediaid, null, accessToken);
		}

		public bool LikeDelete(string mediaid, string userid, string accessToken)
		{
			var url = Configuration.ApiBaseUrl + "media/" + mediaid + "/likes?access_token=" + accessToken;

			var json = RequestDeleteToUrl(url, Configuration.Proxy);
			if (string.IsNullOrEmpty(json))
				return true;

			var res = DeserializeObject<InstagramResponse<User[]>>(json);

			if (_cache != null)
			{
				_cache.Remove("media/popular");
				_cache.Remove("media/" + mediaid);
				_cache.Remove("media/" + mediaid + "/likes");
				_cache.Remove("users/self/" + accessToken);
				_cache.Remove("users/" + userid);
				if (!string.IsNullOrEmpty(userid))
				{
					_cache.Remove("users/" + userid + "/media/recent");
					_cache.Remove("users/self/feed?access_token=" + accessToken);
				}
			}

			return res.Meta.Code == "200";
		}

		public bool LikeToggle(string mediaid, string userid, string accessToken) {

			InstagramMedia media = MediaDetails(mediaid, accessToken).Data;

			return media.UserHasLiked ? LikeDelete(mediaid, userid, accessToken) : LikeAdd(mediaid, userid, accessToken);
		}

		public bool UserIsLiking(string mediaid, string userid, string accessToken)
		{
			var userlinking = Likes(mediaid, accessToken).Data;

			return userlinking.Any(user => user.Id.ToString().Equals(userid));
		}

		#endregion

		#region Tags

		public InstagramResponse<Tag> TagDetails(string tagname, string accessToken)
		{
			if (tagname.Contains("#"))
				tagname = tagname.Replace("#", "");

			var url = Configuration.ApiBaseUrl + "tags/" + tagname + "?access_token=" + accessToken;
			if (string.IsNullOrEmpty(accessToken))
				url = Configuration.ApiBaseUrl + "tags/" + tagname + "?client_id=" + Configuration.ClientId;

			var json = RequestGetToUrl(url, Configuration.Proxy);

			return string.IsNullOrEmpty(json) ? null : DeserializeObject<InstagramResponse<Tag>>(json);
		}

		public InstagramResponse<Tag[]> TagSearch(string query, string accessToken)
		{
			if (query.Contains("#"))
				query = query.Replace("#", "");

			var url = Configuration.ApiBaseUrl + "tags/search?access_token=" + accessToken;
			if (string.IsNullOrEmpty(accessToken))
				url = Configuration.ApiBaseUrl + "tags/search?client_id=" + Configuration.ClientId;

			if (!string.IsNullOrEmpty(query)) url = url + "&q=" + query;

			if (_cache != null)
				if (_cache.Exists(url))
					return _cache.Get<InstagramResponse<Tag[]>>(url);

			string json = RequestGetToUrl(url, Configuration.Proxy);
			if (string.IsNullOrEmpty(json))
				return null;

			var res = DeserializeObject<InstagramResponse<Tag[]>>(json);

			if (_cache != null)
				_cache.Add(url, res, 300);

			return res;
		}

		public Tag[] TagPopular(string accessToken) {
			var pop = MediaPopular(accessToken,true).Data;
			return TagsInMediaList(pop);
		}

		public InstagramResponse<InstagramMedia[]> TagMedia(string tagname, string min_id, string max_id, string accessToken)
		{
			if (tagname.Contains("#"))
				tagname = tagname.Replace("#", "");

			var url = Configuration.ApiBaseUrl + "tags/" + tagname + "/media/recent?access_token=" + accessToken;
			if (string.IsNullOrEmpty(accessToken))
				url = Configuration.ApiBaseUrl + "tags/" + tagname + "/media/recent?client_id=" + Configuration.ClientId;


			if (!string.IsNullOrEmpty(min_id)) url = url + "&min_id=" + min_id;
			if (!string.IsNullOrEmpty(max_id)) url = url + "&max_id=" + max_id;

			if (_cache != null)
				if (_cache.Exists(url))
					return _cache.Get<InstagramResponse<InstagramMedia[]>>(url);

			var json = RequestGetToUrl(url, Configuration.Proxy);
			if (string.IsNullOrEmpty(json))
				return null;

			var res = DeserializeObject<InstagramResponse<InstagramMedia[]>>(json);

			if (_cache != null)
				_cache.Add(url, res, 300);

			return res;
		}

		public static Tag[] TagsInMediaList(InstagramMedia[] media) {
			var t = new List<string>();

			foreach (var tag in from instagramMedia in media from tag in instagramMedia.Tags where !t.Contains(tag) select tag)
				t.Add(tag);

			return TagsFromStrings(t.ToArray());
		}

		public static Tag[] TagsFromStrings(string[] tags)
		{
			var taglist = new List<Tag>(tags.Length);
			taglist.AddRange(tags.Select(s => new Tag
			{
				MediaCount = 0, Name = s
			}));

			return taglist.ToArray();
		}

		#endregion

		#region Locations

		public Location LocationDetails(string locationid, string accessToken)
		{
			var url = Configuration.ApiBaseUrl + "locations/" + locationid + "?access_token=" + accessToken;
			if (string.IsNullOrEmpty(accessToken))
				url = Configuration.ApiBaseUrl + "locations/" + locationid + "?client_id=" + Configuration.ClientId;

			var json = RequestGetToUrl(url, Configuration.Proxy);
			if (string.IsNullOrEmpty(json))
				return null;

			var res = DeserializeObject<InstagramResponse<Location>>(json);
			return res.Data;
		}

		public InstagramMedia[] LocationMedia(string locationid,string min_id,string max_id, string min_timestamp, string max_timestamp, string accessToken)
		{
			var url = Configuration.ApiBaseUrl + "locations/" + locationid + "/media/recent?access_token=" + accessToken;
			if (string.IsNullOrEmpty(accessToken))
				url = Configuration.ApiBaseUrl + "locations/" + locationid + "/media/recent?client_id=" + Configuration.ClientId;

			if (!string.IsNullOrEmpty(min_id)) url = url + "&min_id=" + min_id;
			if (!string.IsNullOrEmpty(max_id)) url = url + "&max_id=" + max_id;
			if (!string.IsNullOrEmpty(min_timestamp)) url = url + "&min_timestamp=" + min_timestamp;
			if (!string.IsNullOrEmpty(max_timestamp)) url = url + "&max_timestamp=" + max_timestamp;

			var json = RequestGetToUrl(url, Configuration.Proxy);
			if (string.IsNullOrEmpty(json))
				return new InstagramMedia[0];

			var res = DeserializeObject<InstagramResponse<InstagramMedia[]>>(json);
			return res.Data;
		}

		public Location[] LocationSearch(string lat, string lng, string foursquare_id, string foursquare_v2_id, string distance, string accessToken)
		{
			if (!string.IsNullOrEmpty(lat) && string.IsNullOrEmpty(lng) || !string.IsNullOrEmpty(lng) && string.IsNullOrEmpty(lat))
				throw new ArgumentException("if lat or lng are specified, both are required.");

			var url = Configuration.ApiBaseUrl + "locations/search?access_token=" + accessToken;
			if (string.IsNullOrEmpty(accessToken))
				url = Configuration.ApiBaseUrl + "locations/search?client_id=" + Configuration.ClientId;

			if (!string.IsNullOrEmpty(lat)) url = url + "&lat=" + lat;
			if (!string.IsNullOrEmpty(lng)) url = url + "&lng=" + lng;
			if (!string.IsNullOrEmpty(foursquare_id)) url = url + "&foursquare_id=" + foursquare_id;
			if (!string.IsNullOrEmpty(foursquare_v2_id)) url = url + "&foursquare_v2_id=" + foursquare_v2_id;
			if (!string.IsNullOrEmpty(distance)) url = url + "&distance=" + distance;

			var json = RequestGetToUrl(url, Configuration.Proxy);
			if (string.IsNullOrEmpty(json))
				return new Location[0];


			var res = DeserializeObject<InstagramResponse<Location[]>>(json);
			return res.Data;
		}

		#endregion

		#region Geography

		public InstagramMedia[] GeographyMedia(string geographyid, string accessToken)
		{
			var url = Configuration.ApiBaseUrl + "geographies/" + geographyid + "/media/recent?access_token=" + accessToken;
			var json = RequestGetToUrl(url, Configuration.Proxy);
			if (string.IsNullOrEmpty(json))
				return new InstagramMedia[0];

			var res = DeserializeObject<InstagramResponse<InstagramMedia[]>>(json);
			return res.Data;
		}

		#endregion

		#region OEmbed

		public Embed EmbedDetails(string url)
		{
			var embedUrl = Configuration.OEmbedUrl + "?url=" + url;

			var json = RequestGetToUrl(embedUrl, Configuration.Proxy);
			if (string.IsNullOrEmpty(json))
				return null;

			var res = DeserializeObject<Embed>(json);
			return res;
		}

		#endregion

	}
}
