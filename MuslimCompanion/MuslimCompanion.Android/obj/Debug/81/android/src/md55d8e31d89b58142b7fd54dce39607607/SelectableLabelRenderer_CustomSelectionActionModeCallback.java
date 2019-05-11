package md55d8e31d89b58142b7fd54dce39607607;


public class SelectableLabelRenderer_CustomSelectionActionModeCallback
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.view.ActionMode.Callback
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onActionItemClicked:(Landroid/view/ActionMode;Landroid/view/MenuItem;)Z:GetOnActionItemClicked_Landroid_view_ActionMode_Landroid_view_MenuItem_Handler:Android.Views.ActionMode/ICallbackInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_onCreateActionMode:(Landroid/view/ActionMode;Landroid/view/Menu;)Z:GetOnCreateActionMode_Landroid_view_ActionMode_Landroid_view_Menu_Handler:Android.Views.ActionMode/ICallbackInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_onDestroyActionMode:(Landroid/view/ActionMode;)V:GetOnDestroyActionMode_Landroid_view_ActionMode_Handler:Android.Views.ActionMode/ICallbackInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_onPrepareActionMode:(Landroid/view/ActionMode;Landroid/view/Menu;)Z:GetOnPrepareActionMode_Landroid_view_ActionMode_Landroid_view_Menu_Handler:Android.Views.ActionMode/ICallbackInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("MuslimCompanion.Droid.Controls.SelectableLabelRenderer+CustomSelectionActionModeCallback, MuslimCompanion.Android", SelectableLabelRenderer_CustomSelectionActionModeCallback.class, __md_methods);
	}


	public SelectableLabelRenderer_CustomSelectionActionModeCallback ()
	{
		super ();
		if (getClass () == SelectableLabelRenderer_CustomSelectionActionModeCallback.class)
			mono.android.TypeManager.Activate ("MuslimCompanion.Droid.Controls.SelectableLabelRenderer+CustomSelectionActionModeCallback, MuslimCompanion.Android", "", this, new java.lang.Object[] {  });
	}

	public SelectableLabelRenderer_CustomSelectionActionModeCallback (md55d8e31d89b58142b7fd54dce39607607.SelectableLabelRenderer p0)
	{
		super ();
		if (getClass () == SelectableLabelRenderer_CustomSelectionActionModeCallback.class)
			mono.android.TypeManager.Activate ("MuslimCompanion.Droid.Controls.SelectableLabelRenderer+CustomSelectionActionModeCallback, MuslimCompanion.Android", "MuslimCompanion.Droid.Controls.SelectableLabelRenderer, MuslimCompanion.Android", this, new java.lang.Object[] { p0 });
	}


	public boolean onActionItemClicked (android.view.ActionMode p0, android.view.MenuItem p1)
	{
		return n_onActionItemClicked (p0, p1);
	}

	private native boolean n_onActionItemClicked (android.view.ActionMode p0, android.view.MenuItem p1);


	public boolean onCreateActionMode (android.view.ActionMode p0, android.view.Menu p1)
	{
		return n_onCreateActionMode (p0, p1);
	}

	private native boolean n_onCreateActionMode (android.view.ActionMode p0, android.view.Menu p1);


	public void onDestroyActionMode (android.view.ActionMode p0)
	{
		n_onDestroyActionMode (p0);
	}

	private native void n_onDestroyActionMode (android.view.ActionMode p0);


	public boolean onPrepareActionMode (android.view.ActionMode p0, android.view.Menu p1)
	{
		return n_onPrepareActionMode (p0, p1);
	}

	private native boolean n_onPrepareActionMode (android.view.ActionMode p0, android.view.Menu p1);

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
