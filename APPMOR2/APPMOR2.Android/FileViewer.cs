using System;
using System.IO;
using Android.Content.Res;
using APPMOR2.Droid;
using Xamarin.Essentials;
using Xamarin.Forms;
using static APPMOR2.Infraestructure.Interfaces;

[assembly: Dependency(typeof(FileViewer))]

namespace APPMOR2.Droid
{
    public class FileViewer : IFileViewer
    {
        public async void ShowPDFTXTFromLocal(string filename)
        {
            try
            {
                // Obtener la ruta de almacenamiento externa
                string storageDir = Android.App.Application.Context.GetExternalFilesDir(null).AbsolutePath;

                // Acceder al AssetManager para obtener el archivo desde los Assets
                AssetManager assetManager = Android.App.Application.Context.Assets;

                // Crear la ruta de destino donde el archivo PDF será copiado
                string filePath = Path.Combine(storageDir, filename);

                // Verifica si el archivo ya existe, si no, lo copia desde los assets
                if (!File.Exists(filePath))
                {
                    using (Stream assetStream = assetManager.Open(filename))
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        await assetStream.CopyToAsync(fileStream);
                    }
                }

                // Abrir el archivo PDF con la aplicación predeterminada del dispositivo
                await Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(filePath)
                });
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                System.Diagnostics.Debug.WriteLine($"Error al abrir el PDF: {ex.Message}");
            }
        }
    }
}