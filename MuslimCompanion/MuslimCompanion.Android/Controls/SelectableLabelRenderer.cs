using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Text.Method;
using Android.Views;
using Android.Widget;
using MuslimCompanion.Controls;
using MuslimCompanion.Droid.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(SelectableLabel), typeof(SelectableLabelRenderer))]

namespace MuslimCompanion.Droid.Controls
{
    public class SelectableLabelRenderer : EditorRenderer
    {
        public SelectableLabelRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Control == null) 
                return;
            
            Control.Background = null;
            Control.SetPadding(0, 0, 0, 0);
            Control.ShowSoftInputOnFocus = false;
            Control.SetTextIsSelectable(true);
            Control.CustomSelectionActionModeCallback = new CustomSelectionActionModeCallback();
            Control.CustomInsertionActionModeCallback = new CustomInsertionActionModeCallback();

            if (GlobalVar.Get<bool>("SEARCHING_AYAH", true))
                SelectPartOfText();

        }


        public void SelectPartOfText()
        {

            if (!GlobalVar.Get<bool>("SEARCHING_AYAH_READY", false))
            {

                TryAgain();
                return;

            }

            Control.RequestFocus();
            int startIndex = GlobalVar.Get<int>("START_INDEX", 0);
            int endIndex = GlobalVar.Get<int>("END_INDEX", 0);
            Control.SetSelection(startIndex, endIndex);
            GlobalVar.Set("SEARCHING_AYAH", false);
            GlobalVar.Set("SEARCHING_AYAH_READY", false);

        }

        async void TryAgain()
        {

            await Task.Delay(100);
            SelectPartOfText();

        }
        

        private class CustomInsertionActionModeCallback : Java.Lang.Object, ActionMode.ICallback
        {
            public bool OnCreateActionMode(ActionMode mode, IMenu menu) => false;

            public bool OnActionItemClicked(ActionMode m, IMenuItem i) => false;

            public bool OnPrepareActionMode(ActionMode mode, IMenu menu) => true;

            public void OnDestroyActionMode(ActionMode mode) { }


        }

        private class CustomSelectionActionModeCallback : Java.Lang.Object, ActionMode.ICallback
        {
            private const int CopyId = Android.Resource.Id.Copy;

            public bool OnActionItemClicked(ActionMode m, IMenuItem i) => false;

            public bool OnCreateActionMode(ActionMode mode, IMenu menu) => true;

            public void OnDestroyActionMode(ActionMode mode) { }

            public bool OnPrepareActionMode(ActionMode mode, IMenu menu)
            {
                try
                {
                    var copyItem = menu.FindItem(CopyId);
                    var title = copyItem.TitleFormatted;
                    menu.Clear();
                    menu.Add(0, CopyId, 0, title);
                }
                catch
                {
                    // ignored
                }

                return true;
            }
        }
    }

    public class CustomEditText : FormsEditText
    {
        public CustomEditText(Context context) : base(context)
        {
        }

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();

            Enabled = false;
            Enabled = true;
        }
    }

}