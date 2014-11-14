using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeadSpin.Reference.Persistence.Repositories;

namespace HeadSpin.Reference.Domain.Services
{

    public class DocumentService
    {
        public static IList<string> GetDocumentCategories()
        {
            return (new Managers.DocumentManager(new DocumentRepository())).GetDocumentCategories();
        }

        public static void DeleteLibraryDocument(int Id, string userId)
        {
            (new Managers.DocumentManager(new DocumentRepository())).DeleteLibraryDocument(Id, userId);
        }

        public static void DeleteSessionDocument(int Id, string userId)
        {
             (new Managers.DocumentManager(new DocumentRepository())).DeleteSessionDocument(Id, userId);
        }

        public static DTO.Document GetDocumentById(int Id)
        {
            return (new Managers.DocumentManager(new DocumentRepository())).GetDocumentById(Id);
        }

        public static IList<DTO.Document> GetDocumentsByFilenameAndCategory(string filename = null, string category = null)
        {
            return (new Managers.DocumentManager(new DocumentRepository())).GetDocumentsByFilenameAndCategory(filename, category);
        }
        

 
        
        public static void SaveDocument(DTO.Document doc, string userId)
        {
            (new Managers.DocumentManager(new DocumentRepository())).SaveDocument(doc, userId);
        }

        public static DTO.Document SubmitDocument(string fullFilepath, int? sessionId, string category, string userId)
        {
            return (new Managers.DocumentManager(new DocumentRepository())).SubmitDocument(fullFilepath, sessionId, category, userId);
        }

        public static DTO.Document SubmitDocument(byte[] fileBytes, string originalFilename, string mimeType, int? sessionId, string category, string userId)
        {
            return (new Managers.DocumentManager(new DocumentRepository())).SubmitDocument(fileBytes, originalFilename, mimeType, sessionId, category, userId);
        }

        public static DTO.Document SubmitDocument(byte[] fileBytes, string originalFilename, string mimeType, int? sessionId, string userId)
        {
            return (new Managers.DocumentManager(new DocumentRepository())).SubmitDocument(fileBytes, originalFilename,mimeType, sessionId, userId);
        }

        public static bool TryDownloadDocument(int id, out byte[] docFile, out string mimeType, out string filename)
        {
            return (new Managers.DocumentManager(new DocumentRepository())).TryDownloadDocument(id, out docFile, out mimeType, out filename);
        }

        public static byte[] GetPageImagePng(int documentId, int pageNo, string userId)
        {
            return (new Managers.DocumentManager(new DocumentRepository())).GetPageImagePng(documentId, pageNo, userId);
        }

        public static byte[] GetPageImagePng(int documentId, int pageNo, int? h, int? w, string userId)
        {
            return (new Managers.DocumentManager(new DocumentRepository())).GetPageImagePng(documentId, pageNo, h, w, userId);
        }

    }
}