namespace MvcProyectoResauranteAPI.Helpers
{   
    public enum Folders {Images = 0, Uploads = 1, Facturas = 2, Temporal}

    public class HelperPathProvider
    {   

        private IWebHostEnvironment hostEnvironment;

        public HelperPathProvider(IWebHostEnvironment hostEnvironment) 
        { 
            this.hostEnvironment = hostEnvironment;
        
        }

        public string MapPath(string fileName, Folders folder)
        {
            string carpeta = "";
            if(folder == Folders.Images) 
            {
                carpeta = "images";
            
            }else if(folder == Folders.Uploads)
            {
                carpeta = "uploads";
            }else if (folder == Folders.Facturas) 
            {
                carpeta = "facturas";
            }else if (folder == Folders.Temporal) 
            {
                carpeta = "temp";
            
            }

            string rootPath = hostEnvironment.WebRootPath;
            string path = Path.Combine(rootPath,carpeta ,fileName);
            return path;


        }


    }
}
