package md55d8e31d89b58142b7fd54dce39607607;


public class CustomEditText
	extends md51558244f76c53b6aeda52c8a337f2c37.FormsEditText
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onAttachedToWindow:()V:GetOnAttachedToWindowHandler\n" +
			"";
		mono.android.Runtime.register ("MuslimCompanion.Droid.Controls.CustomEditText, MuslimCompanion.Android", CustomEditText.class, __md_methods);
	}


	public CustomEditText (android.content.Context p0)
	{
		super (p0);
		if (getClass () == CustomEditText.class)
			mono.android.TypeManager.Activate ("MuslimCompanion.Droid.Controls.CustomEditText, MuslimCompanion.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public CustomEditText (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == CustomEditText.class)
			mono.android.TypeManager.Activate ("MuslimCompanion.Droid.Controls.CustomEditText, MuslimCompanion.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public CustomEditText (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == CustomEditText.class)
			mono.android.TypeManager.Activate ("MuslimCompanion.Droid.Controls.CustomEditText, MuslimCompanion.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public void onAttachedToWindow ()
	{
		n_onAttachedToWindow ();
	}

	private native void n_onAttachedToWindow ();

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
