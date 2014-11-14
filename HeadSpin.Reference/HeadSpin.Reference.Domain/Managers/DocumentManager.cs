using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeadSpin.Reference.Persistence.Repositories;
 using HeadSpin.Core.Utilities.Document;
 using HeadSpin.Core.Utilities.Logging;
 using HeadSpin.Core.Utilities;

namespace HeadSpin.Reference.Domain.Managers
{

    internal class DocumentManager : BaseManager
    {
        private IDocumentRepository _repo = null;

        public DocumentManager(IDocumentRepository argRepository)
        {
            _repo = argRepository;
        }

        public DTO.Document GetDocumentById(int Id)
        {
            return _repo.GetDocumentById(Id);
        }

        public void DeleteLibraryDocument(int Id, string userId)
        {
            var doc = GetDocumentById(Id);

            if (doc != null)
            {
                doc.DeleteDate = DateTime.Now;

                SaveDocument(doc, userId);
            }

        }


        public void DeleteSessionDocument(int Id, string userId)
        {
            var doc = GetDocumentById(Id);

            if (doc != null && doc.SessionId.HasValue)
            {
                doc.DeleteDate = DateTime.Now;

                SaveDocument(doc, userId);
            }
             
        }

        public IList<DTO.Document> GetDocumentsByFilenameAndCategory(string filename = null, string category = null)
        {
            IList<DTO.Document> docs = null;

            //do not perform a "search all". Note: we're not yet supporting wildcards
            if (!String.IsNullOrWhiteSpace(filename) && !(filename.Length == 1 && filename.Contains('.')))
            {
                docs = _repo.GetDocumentsByFilenameAndCategory(filename, category);
            }
            return docs;
        }
        

       
        public void SaveDocument(DTO.Document doc, string userId)
        {
            this.SetUserInfo(doc, userId);

            if (doc.DocumentPages != null)
            {
                foreach (var p in doc.DocumentPages)
                {
                    this.SetUserInfo(p, userId);
                }
            }

            _repo.SaveDocument(doc);
        }

        public byte[] GetPageImagePng(int documentId, int pageNo, string userId)
        {
            var doc = GetDocumentById(documentId);

            var page = (from p in doc.DocumentPages where p.PageNo == pageNo select p).SingleOrDefault();

            if (page != null)
            {
                //@@@ repo call.... s3 is technically a repository!
                return null;// Utilities.Document.DocumentSvc.DownloadFromStorage(page.StoredFilePath);
            }

            return null;
        }

        public byte[] GetPageImagePng(int documentId, int pageNo, int? h, int? w, string userId)
        {
            byte[] pageimage = GetPageImagePng(documentId, pageNo, userId);

            if (pageimage != null)
            {
                if (h.GetValueOrDefault() > 0 && w.GetValueOrDefault() > 0)
                {
                    return DocumentSvc.ResizePageImage(pageimage, h.Value, w.Value);
                }
                else
                {
                    return pageimage;
                }
            }

            return null;
        }

        public bool TryDownloadDocument(int id, out byte[] docFile, out string mimeType, out string filename )
        {
            var dto = GetDocumentById(id);

            if (dto == null)
            {
                docFile = null;
                mimeType = string.Empty;
                filename = string.Empty;

                return false;
            }

            mimeType = dto.MimeType;
            docFile = null; // todo repo call DocumentSvc.DownloadFromStorage(dto.StoredFilePathAndName);
            filename = dto.FileName;
            
            return true;
        }

        public DTO.Document SubmitDocument(string fullFilepath, int? sessionId, string category, string userId)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(fullFilepath);

            string originalFilename = System.IO.Path.GetFileName(fullFilepath);
            
            return SubmitDocument(fileBytes, originalFilename, null, null, category, userId);
        }

        public IList<string> GetDocumentCategories()
        {
            return _repo.GetDocumentCategories();
        }

        public DTO.Document SubmitDocument(byte[] fileBytes, string originalFilename, string mimeType, int? sessionId, string userId)
        {
            return SubmitDocument(fileBytes, originalFilename, mimeType, sessionId, null, userId);
        }

        public DTO.Document SubmitDocument(byte[] fileBytes, string originalFilename, string mimeType, int? sessionId, string category, string userId)
        {
            // submit of a doc means to...
            // - save a domain doc object to get the id
            // - load the original to s3 with a naming convention of doc-id/doc-id.doc
            // - convert the original pages to images 
            // - add pages to the document object and save it again...


            DTO.Document doc = null;

            try
            {   
                doc = new DTO.Document()
                {
                    FileName = DocumentSvc.CalculateValidFilename(originalFilename, mimeType),
                    SessionId = sessionId,
                    MimeType = mimeType,
                    Category = category
                };

                SaveDocument(doc, userId);

                //@@@ make repo call todo
                //DocumentSvc.UploadToStorage(fileBytes, doc.StoredFilePathAndName);

                var pageImages = DocumentSvc.ConvertPagesToImages(doc.StoredFilePathAndName);

                if (pageImages == null)
                {
                    throw new Exception("No valid page images as a result of document conversion...");
                }

                //

                
                doc.DocumentPages = new List<DTO.DocumentPage>();

                int pcount = 0;
                foreach (var pi in pageImages)
                {
                    pcount++;

                    var dp = new DTO.DocumentPage
                    {
                        ImageFileExt = System.IO.Path.GetExtension(pi),
                        StoredFilename = System.IO.Path.GetFileName(pi),
                        PageNo = pcount,
                        HeightPx = 0,
                        WidthPx = 0
                    };

                    doc.DocumentPages.Add(dp);
                }

                // save to give the pages an id
                SaveDocument(doc, userId);

                foreach(var dp in doc.DocumentPages)
                {
                    // now spin thru the pages and get the real size...

                    int hpx = 0;
                    int wpx = 0;

                    DocumentSvc.GetImageDimensions(dp.StoredFilePath, out hpx, out wpx);

                    dp.WidthPx = wpx;
                    dp.HeightPx = hpx;
                }

                // last save to make sure the h/w px is set properly..
                SaveDocument(doc, userId);
                

            }
            catch
            {
                if (doc != null)
                {
                    try
                    {
                        _repo.DeleteDocument(doc);
                    }
                    catch (Exception err)
                    {
                        // eat it... maybe log it Einstein...
                        (new FileLog(ConfigHelper.GetAppSettingValue("file-log"))).Log(err.Message);
                    }
                }
                throw;
            }

            return doc;
        }

    }
}