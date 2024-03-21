namespace Company.PL.Helper
{
    public static class DocumentSettings
    {
        public static string UploadFile(IFormFile file, string folderName)
        {
            // 1. Get located folder path 
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files", folderName);

            // 2. Get File Name and make it Unique
            var fileName = $"{Guid.NewGuid()}-{Path.GetFileName(file.FileName)}";

            //3. Get file path
            var filePath = Path.Combine(folderPath, fileName);

            // 4. Save my file in file path => As Stream {data per time}
            using var fileStream = new FileStream(filePath, FileMode.Create);

            file.CopyTo(fileStream); //copy the content of uploaded file to target stream

            return fileName;

        }


		

		public static void DeleteFile(string fileName, string folderName)
		{
			if (fileName != null && folderName != null)
			{
				string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", folderName, fileName);
				if (File.Exists(filePath))
					File.Delete(filePath);
			}
		}

	}
}
