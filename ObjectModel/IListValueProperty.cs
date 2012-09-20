using System.Collections;
using System.Collections.Generic;
using VersionOne.SDK.ObjectModel.List;

namespace VersionOne.SDK.ObjectModel
{
	/// <summary>
	/// Represents a List Value property of an Entity
	/// </summary>
	public interface IListValueProperty
	{
		/// <summary>
		/// Gets or Sets the currently saved value
		/// </summary>
		string CurrentValue { get; set; }

		/// <summary>
		/// The names of all of the active values that this relation can be set to
		/// </summary>
		string[] AllValues { get;}

		/// <summary>
		/// Validates a value for this relation
		/// </summary>
		/// <param name="value">The name to try</param>
		/// <returns>True if the name matches an item in the list, otherwise false.</returns>
		bool IsValid(string value);

		/// <summary>
		/// Removes the currently selected value
		/// </summary>
		void ClearCurrentValue();
	}
}