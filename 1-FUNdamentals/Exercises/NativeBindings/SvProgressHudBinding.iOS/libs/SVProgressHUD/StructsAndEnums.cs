using System;
using ObjCRuntime;

namespace SVProgressHUD
{
	[Native]
	public enum SVProgressHUDStyle : nint
	{
		Light,
		Dark,
		Custom
	}

	[Native]
	public enum SVProgressHUDMaskType : nuint
	{
		None = 1,
		Clear,
		Black,
		Gradient,
		Custom
	}

	[Native]
	public enum SVProgressHUDAnimationType : nuint
	{
		Flat,
		Native
	}
}
