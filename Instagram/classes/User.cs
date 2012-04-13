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
	public class User : InstagramBaseObject, IEquatable<User>
	{
		public string id;
		public string username;
		public string full_name;
		public string profile_picture;

		public string first_name;
		public string last_name;
		public string bio;
		public string website;
		public string type;

		public Counts counts;

		public bool isFollowing;
		public bool isFollowed;
		public bool isSelf;

		public bool Equals(User other)
		{
			if (ReferenceEquals(other, null))
				return false;

			if (ReferenceEquals(this, other))
				return true;

			return id.Equals(other.id);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(obj, null))
				return false;

			if (ReferenceEquals(this, obj))
				return true;

			return obj is User && id.Equals(((User) obj).id);
		}

		public override int GetHashCode()
		{
			var hashProductName = id == null ? 0 : int.Parse(id);
			return hashProductName;
		}
	}
}
