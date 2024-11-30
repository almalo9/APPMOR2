using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using APPMOR2.Droid;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using static APPMOR2.Infraestructure.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(FileViewer))]
namespace APPMOR2.Droid
{
    public class FileViewer:IFileViewer
    {
        public async void ShowPDFTXTFromLocal(string filename)
        {
            
            string storageDir = Android.App.Application.Context.GetExternalFilesDir(null).AbsolutePath;
            string dir = "APPMOR2.Fonts.Help.pdf";
            Assembly assembly = typeof(App).GetTypeInfo().Assembly;
            Stream str = assembly.GetManifestResourceStream(dir);
            string pathFile = Path.Combine(storageDir, "Help.pdf");
            FileStream file = File.Create(pathFile);
            if (File.Exists(pathFile))
            {
                
                MemoryStream ms = new MemoryStream();
                str.CopyTo(ms);
                await file.WriteAsync(ms.ToArray(),0,ms.ToArray().Length);


                await Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(pathFile)
                });
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("not ok");
            }



            //System.Diagnostics.Debug.WriteLine(assembly.GetManifestResourceNames());

            //string dirPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.;
            //System.Diagnostics.Debug.WriteLine(dirPath);
            //System.Diagnostics.Debug.WriteLine(Path.Combine(dirPath, "Help.pdf"));
            // string dir = "/storage/emulated/0/Android/Download/Help.pdf";
            //var assetManager = Android.App.Application.Context.Assets.Open(filename);
            //FileStream file = File.Create(dir);
            //MemoryStream ms = new MemoryStream();
            //assetManager.CopyTo(ms);
            //file.Write(ms.ToArray(),0,ms.ToArray().Length);
            //System.Diagnostics.Debug.WriteLine(assetManager.ToString());
            //var file = new Java.IO.File(Android.App.Application.Context.Assets.Open("Hepl.pdf"));
            //var str = assetManager.Open("Help.pdf");
            //System.Diagnostics.Debug.WriteLine(assetManager.ToString());

            //string path = Android.Net.Uri.Parse("file:///android_asset/Help.pdf").ToString();
            //System.Diagnostics.Debug.WriteLine(path);
            /*string dest = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Help.pdf");
            System.Diagnostics.Debug.WriteLine(str.ToString());
            var file = new Java.IO.File("android_asset/Help.pdf");
            
            if (file.Exists())
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    var uri = Android.Net.Uri.FromFile(file);
                    Intent intent = new Intent(Intent.ActionView);
                    var mimetype = MimeTypeMap.Singleton.GetMimeTypeFromExtension(MimeTypeMap.GetFileExtensionFromUrl((string)uri).ToLower());
                    intent.SetDataAndType(uri, mimetype);
                    intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);

                    try
                    {
                        Android.App.Application.Context.StartActivity(intent);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.ToString());
                    }
                });
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("file not found");
            }*/
        }
    }
}
