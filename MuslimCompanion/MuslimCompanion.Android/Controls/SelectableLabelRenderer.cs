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
using static MuslimCompanion.Core.GeneralManager;



[assembly: ExportRenderer(typeof(SelectableLabel), typeof(SelectableLabelRenderer))]
[assembly: Dependency(typeof(SelectableLabel))]
[assembly: Dependency(typeof(SelectableLabelRenderer))]
[assembly: Dependency(typeof(SelectableInterfaceImplementation))]

namespace MuslimCompanion.Droid.Controls
{
    public class SelectableLabelRenderer : EditorRenderer
    {
        public SelectableLabelRenderer(Context context) : base(context)
        {

            GlobalVar.Set("activeControl", null);

        }

        //public SelectableInterface si;

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Control == null) 
                return;

            GlobalVar.Set("activeControl", Control);

            Control.Background = null;
            Control.SetPadding(0, 0, 0, 0);
            Control.ShowSoftInputOnFocus = false;
            Control.SetTextIsSelectable(true);
            Control.CustomSelectionActionModeCallback = new CustomSelectionActionModeCallback();
            Control.CustomInsertionActionModeCallback = new CustomInsertionActionModeCallback();

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

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

    public class SelectableInterfaceImplementation : ISelectableLabel
    {

        public async void SelectPartOfText(int startIndex, int endIndex)
        {

            FormsEditText fet = GlobalVar.Get<FormsEditText>("activeControl");

            int timeOutInSeconds = 1, awaitedMS = 0;

            do
            {

                await Task.Delay(100);
                awaitedMS += 100;
                fet = GlobalVar.Get<FormsEditText>("activeControl");
                if (awaitedMS >= timeOutInSeconds * 1000)
                    break;

            }
            while (fet == null);

            if (fet == null)
                return;

            fet.RequestFocus();

            try
            {

                fet.SetSelection(startIndex, endIndex);

            }
            catch (Exception ex)
            {

                int newIndex = endIndex - 1;

                while (newIndex > fet.Text.Length)
                {

                    newIndex--;

                }

                fet.SetSelection(startIndex, newIndex);

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