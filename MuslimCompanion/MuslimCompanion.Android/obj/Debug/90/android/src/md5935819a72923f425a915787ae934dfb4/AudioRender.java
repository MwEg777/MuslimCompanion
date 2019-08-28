package md5935819a72923f425a915787ae934dfb4;


public class AudioRender
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("MuslimCompanion.Droid.AndroidCore.AudioRender, MuslimCompanion.Android", AudioRender.class, __md_methods);
	}


	public AudioRender ()
	{
		super ();
		if (getClass () == AudioRender.class)
			mono.android.TypeManager.Activate ("MuslimCompanion.Droid.AndroidCore.AudioRender, MuslimCompanion.Android", "", this, new java.lang.Object[] {  });
	}

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
