﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace Light.Data
{
	static class Utility
	{
		const sbyte MIN_SBYTE = 0;

		const byte MIN_BYTE = 0;

		const short MIN_SHORT = 0;

		const ushort MIN_USHORT = 0;

		public static bool ParseDbType (string dbType, out DbType type)
		{
			type = DbType.String;
			int index = dbType.IndexOf ('(');
			string typeString = string.Empty;
			if (index < 0) {
				typeString = dbType;
			}
			else if (index == 0) {
				return false;
			}
			else {
				typeString = dbType.Substring (0, index);
			}
			bool result = false;
			try {
				type = (DbType)Enum.Parse (typeof(DbType), typeString, true);
				result = true;
			}
			catch {
				result = false;
			}
			if (typeString.Equals ("int", StringComparison.OrdinalIgnoreCase)) {
				type = DbType.Int32;
				return true;
			}
			else if (typeString.Equals ("short", StringComparison.OrdinalIgnoreCase)) {
				type = DbType.Int16;
				return true;
			}
			else if (typeString.Equals ("long", StringComparison.OrdinalIgnoreCase)) {
				type = DbType.Int64;
				return true;
			}
			else if (typeString.Equals ("uint", StringComparison.OrdinalIgnoreCase)) {
				type = DbType.UInt32;
				return true;
			}
			else if (typeString.Equals ("ushort", StringComparison.OrdinalIgnoreCase)) {
				type = DbType.UInt16;
				return true;
			}
			else if (typeString.Equals ("ulong", StringComparison.OrdinalIgnoreCase)) {
				type = DbType.UInt64;
				return true;
			}
			else if (typeString.Equals ("float", StringComparison.OrdinalIgnoreCase)) {
				type = DbType.Double;
				return true;
			}
			else if (typeString.Equals ("bool", StringComparison.OrdinalIgnoreCase)) {
				type = DbType.Boolean;
				return true;
			}

			return result;
		}

		public static bool ParseSize (string dbType, out int size)
		{
			size = 0;
			string result = Regex.Match (dbType, "(?<=\\u0028).*?(?=\\u0029)").Value;
			if (string.IsNullOrEmpty (result)) {
				return false;
			}
			string[] arr = result.Split (',');
			if (int.TryParse (arr [0], out size)) {
				return true;
			}
			return false;
		}

		public static bool EnumableObjectEquals (object value1, object value2)
		{
			if (Object.Equals (value1, value2)) {
				return true;
			}
			Type objType1 = value1.GetType ();
			Type objType2 = value2.GetType ();
			if (objType1 != objType2) {
				return false;
			}
			if (objType1 == typeof(string)) {
				return string.Equals (value1, value2);
			}
			if (value1 is IEnumerable) {
				System.Collections.IEnumerator e1 = (value1 as IEnumerable).GetEnumerator ();
				System.Collections.IEnumerator e2 = (value2 as IEnumerable).GetEnumerator ();

				while (true) {
					bool b1 = e1.MoveNext ();
					bool b2 = e2.MoveNext ();
					if (b1 && b2) {
						if (!Object.Equals (e1.Current, e2.Current)) {
							return false;
						}
					}
					else if (!b1 && !b2) {
						return true;
					}
					else {
						return false;
					}
				}
			}
			else {
				return false;
			}
		}

		public static int EnumableHashCode (IEnumerable e)
		{
			int result = 0;
			foreach (object obj in e) {
				result ^= obj.GetHashCode ();
			}
			return result;
		}

		public static object GetDefaultValue (TypeCode typeCode)
		{
			object obj;
			switch (typeCode) {
				case TypeCode.String:
					obj = string.Empty;
					break;
				case TypeCode.Boolean:
					obj = false;
					break;
				case TypeCode.Char:
					obj = Char.MinValue;
					break;
				case TypeCode.SByte:
					obj = MIN_SBYTE;
					break;
				case TypeCode.Byte:
					obj = MIN_BYTE;
					break;
				case TypeCode.Int16:
					obj = MIN_SHORT;
					break;
				case TypeCode.UInt16:
					obj = MIN_USHORT;
					break;
				case TypeCode.Int32:
					obj = 0;
					break;
				case TypeCode.UInt32:
					obj = 0u;
					break;
				case TypeCode.Int64:
					obj = 0L;
					break;
				case TypeCode.UInt64:
					obj = 0uL;
					break;
				case TypeCode.Single:
					obj = 0f;
					break;
				case TypeCode.Double:
					obj = 0d;
					break;
				case TypeCode.Decimal:
					obj = 0m;
					break;
				case TypeCode.DateTime:
					obj = DateTime.MinValue;
					break;
				case TypeCode.Object:
					obj = null;
					break;
				case TypeCode.DBNull:
					obj = DBNull.Value;
					break;
				default:
					obj = null;
					break;
			}
			return obj;
		}
	}
}
