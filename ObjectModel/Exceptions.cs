using System;

namespace VersionOne.SDK.ObjectModel
{
	/// <summary>
	/// Base Exception for SDK Exceptions
	/// </summary>
	public abstract class SDKException : ApplicationException
	{
		internal SDKException(string message) : base(message) { }
	}

	/// <summary>
	/// Thrown when an instance is validated and cannot communicate with the VersionOne Application
	/// </summary>
	public class ApplicationUnavailableException : SDKException
	{
		internal ApplicationUnavailableException(string message) : base(message) { }
	}

	/// <summary>
	/// Throw when an instance is validated and the supplied credentials are invalid
	/// </summary>
	public class AuthenticationException : SDKException
	{
		internal AuthenticationException(string message) : base(message) { }
	}
	
	/// <summary>
	/// Thrown when an entity is saved and a rule or security violation has occurred.
	/// </summary>
	public class DataException : SDKException
	{
		internal DataException(string message) : base(message) { }
	}

	/// <summary>
	/// Thrown when an attachment stream exceeds the server's allowed size limit
	/// </summary>
	public class AttachmentLengthExceededException : SDKException
	{
		internal AttachmentLengthExceededException(string message) : base(message) { }
	}
}
