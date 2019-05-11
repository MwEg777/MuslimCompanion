package md5935819a72923f425a915787ae934dfb4;


public class AudioRender_OnCompletionHelper
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.media.MediaPlayer.OnCompletionListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCompletion:(Landroid/media/MediaPlayer;)V:GetOnCompletion_Landroid_media_MediaPlayer_Handler:Android.Media.MediaPlayer/IOnCompletionListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("MuslimCompanion.Droid.AndroidCore.AudioRender+OnCompletionHelper, MuslimCompanion.Android", AudioRender_OnCompletionHelper.class, __md_methods);
	}


	public AudioRender_OnCompletionHelper ()
	{
		super ();
		if (getClass () == AudioRender_OnCompletionHelper.class)
			mono.android.TypeManager.Activate ("MuslimCompanion.Droid.AndroidCore.AudioRender+OnCompletionHelper, MuslimCompanion.Android", "", this, new java.lang.Object[] {  });
	}


	public void onCompletion (android.media.MediaPlayer p0)
	{
		n_onCompletion (p0);
	}

	private native void n_onCompletion (android.media.MediaPlayer p0);

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
