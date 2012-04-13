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
	public class User : InstagramBaseObject, IEquatable<User>
	{
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }

		[JsonProperty(PropertyName = "username")]
		public string Username { get; set; }

		[JsonProperty(PropertyName = "full_name")]
		public string FullName { get; set; }

		[JsonProperty(PropertyName = "profile_picture")]
		public string ProfilePicture { get; set; }

		[JsonProperty(PropertyName = "first_name")]
		public string FirstName { get; set; }

		[JsonProperty(PropertyName = "last_name")]
		public string LastName { get; set; }

		[JsonProperty(PropertyName = "bio")]
		public string Bio { get; set; }

		[JsonProperty(PropertyName = "website")]
		public string Website { get; set; }

		[JsonProperty(PropertyName = "type")]
		public string Type { get; set; }

		[JsonProperty(PropertyName = "counts")]
		public Counts Counts { get; set; }

		[JsonProperty(PropertyName = "isFollowing")]
		public bool IsFollowing { get; set; }

		[JsonProperty(PropertyName = "isFollowed")]
		public bool IsFollowed { get; set; }

		[JsonProperty(PropertyName = "isSelf")]
		public bool IsSelf { get; set; }

		public bool Equals(User other)
		{
			if (ReferenceEquals(other, null))
				return false;

			if (ReferenceEquals(this, other))
				return true;

			return Id.Equals(other.Id);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(obj, null))
				return false;

			if (ReferenceEquals(this, obj))
				return true;

			return obj is User && Id.Equals(((User)obj).Id);
		}

		public override int GetHashCode()
		{
			var hashProductName = Id == null ? 0 : int.Parse(Id);
			return hashProductName;
		}
	}
}
