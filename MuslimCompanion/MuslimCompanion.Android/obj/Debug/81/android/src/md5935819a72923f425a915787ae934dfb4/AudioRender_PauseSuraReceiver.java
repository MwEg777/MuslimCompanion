package md5935819a72923f425a915787ae934dfb4;


public class AudioRender_PauseSuraReceiver
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
		mono.android.Runtime.register ("MuslimCompanion.Droid.AndroidCore.AudioRender+PauseSuraReceiver, MuslimCompanion.Android", AudioRender_PauseSuraReceiver.class, __md_methods);
	}


	public AudioRender_PauseSuraReceiver ()
	{
		super ();
		if (getClass () == AudioRender_PauseSuraReceiver.class)
			mono.android.TypeManager.Activate ("MuslimCompanion.Droid.AndroidCore.AudioRender+PauseSuraReceiver, MuslimCompanion.Android", "", this, new java.lang.Object[] {  });
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
