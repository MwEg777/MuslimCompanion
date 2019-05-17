package md5935819a72923f425a915787ae934dfb4;


public class Receiver1
	extends android.content.BroadcastReceiver
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onReceive:(Landroid/content/Context;Landroid/content/Intent;)V:GetOnReceive_Landroid_content_Context_Landroid_content_Intent_Handler\n" +
			"";
		mono.android.Runtime.register ("MuslimCompanion.Droid.AndroidCore.Receiver1, MuslimCompanion.Android", Receiver1.class, __md_methods);
	}


	public Receiver1 ()
	{
		super ();
		if (getClass () == Receiver1.class)
			mono.android.TypeManager.Activate ("MuslimCompanion.Droid.AndroidCore.Receiver1, MuslimCompanion.Android", "", this, new java.lang.Object[] {  });
	}


	public void onReceive (android.content.Context p0, android.content.Intent p1)
	{
		n_onReceive (p0, p1);
	}

	private native void n_onReceive (android.content.Context p0, android.content.Intent p1);

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
