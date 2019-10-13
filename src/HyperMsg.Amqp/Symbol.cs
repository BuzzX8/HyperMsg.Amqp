//  ------------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation
//  All rights reserved. 
//  
//  Licensed under the Apache License, Version 2.0 (the ""License""); you may not use this 
//  file except in compliance with the License. You may obtain a copy of the License at 
//  http://www.apache.org/licenses/LICENSE-2.0  
//  
//  THIS CODE IS PROVIDED *AS IS* BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, 
//  EITHER EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION ANY IMPLIED WARRANTIES OR 
//  CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE, MERCHANTABLITY OR 
//  NON-INFRINGEMENT. 
// 
//  See the Apache Version 2.0 License for specific language governing permissions and 
//  limitations under the License.
//  ------------------------------------------------------------------------------------

using System;

namespace HyperMsg.Amqp
{
	/// <summary>
	/// Defines symbolic values from a constrained domain.
	/// </summary>
	public class Symbol
	{
		private readonly string value;

		/// <summary>
		/// Initializes a symbol value.
		/// </summary>
		/// <param name="value">The string value./</param>
		public Symbol(string value)
		{
			this.value = value ?? throw new ArgumentNullException(nameof(value));
		}

		/// <summary>
		/// Converts a string value to a symbol implicitly.
		/// </summary>
		/// <param name="value">The string value.</param>
		/// <returns></returns>
		public static implicit operator Symbol(string value)
		{
			return value == null ? null : new Symbol(value);
		}

		/// <summary>
		/// Converts a symbol to a string value implicitly.
		/// </summary>
		/// <param name="value">the symbol value.</param>
		/// <returns></returns>
		public static implicit operator string(Symbol value) => value?.value;

		/// <summary>
		/// Compares equality of an object with the current symbol.
		/// </summary>
		/// <param name="obj">The object to compare.</param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			Symbol other = obj as Symbol;
			return other != null && value.Equals(other.value);
		}

		/// <summary>
		/// Gets the hash code of the symbol object.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode() => value.GetHashCode();

		/// <summary>
		/// Returns a string that represents the current map object.
		/// </summary>
		/// <returns></returns>
		public override string ToString() => value;
	}
}
