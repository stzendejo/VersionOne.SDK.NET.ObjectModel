using System;
using VersionOne.SDK.ObjectModel.List;

namespace VersionOne.SDK.ObjectModel
{
	/// <summary>
	/// Provides access to custom fields
	/// </summary>
	public interface ICustomAttributeDictionary
	{
		/// <summary>
		/// Gets or Sets the value of the custom field.
		/// </summary>
		/// <param name="attributeName">The friendly name of the custom field, e.g. "Origin", not "Custom_Origin".</param>
		object this[string attributeName] { get; set;}

		/// <summary>
		/// Indicates the presence of a custom field.
		/// </summary>
		/// <param name="attributeName">The friendly name of the custom field, e.g. "Origin", not "Custom_Origin".</param>
		/// <returns>True if the field exists.</returns>
		bool ContainsKey(string attributeName);

		/// <summary>
		/// Gets the value of a Custom Field as a numeric.
		/// </summary>
		/// <param name="attributeName">The friendly name of the custom field, e.g. "Origin", not "Custom_Origin".</param>
		/// <returns>The value of the field as a double, or null</returns>
		double? GetNumeric(string attributeName);

		/// <summary>
		/// Gets the value of a Custom Field as a nullable boolean.
		/// </summary>
		/// <param name="attributeName">The friendly name of the custom field, e.g. "Origin", not "Custom_Origin".</param>
		/// <returns>The value of the field as a boolean, or null</returns>
		bool? GetBool(string attributeName);

		/// <summary>
		/// Gets the value of a Custom Field as a DateTime.
		/// </summary>
		/// <param name="attributeName">The friendly name of the custom field, e.g. "Origin", not "Custom_Origin".</param>
		/// <returns>The value of the field as a DateTime, or null</returns>
		DateTime? GetDate(string attributeName);

		/// <summary>
		/// Gets the value of a Custom Field as a string.
		/// </summary>
		/// <param name="attributeName">The friendly name of the custom field, e.g. "Origin", not "Custom_Origin".</param>
		/// <returns>The value of the field as a string, or null</returns>
		string GetString(string attributeName);
	}

	/// <summary>
	/// Provides acces to Custom List-Type values
	/// </summary>
	public interface ICustomDropdownDictionary
	{
		/// <summary>
		/// Gets or Sets the value of a custom relation.
		/// </summary>
		/// <param name="attributeName">The friendly name of the custom relation, e.g. "Origin", not "Custom_Origin".</param>
		/// <returns></returns>
		IListValueProperty this[string attributeName] { get; }
		/// <summary>
		/// Indicates the presence of a custom relation.
		/// </summary>
		/// <param name="attributeName">The friendly name of the custom relation, e.g. "Origin", not "Custom_Origin".</param>
		/// <returns>True if the custom relation exists.</returns>
		bool ContainsKey(string attributeName);
	}

	
}