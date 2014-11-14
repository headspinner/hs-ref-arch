using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Insight.Database;
using System.Data.SqlClient;
using System.Configuration;

namespace HeadSpin.Reference.Persistence.Repositories
{
    public class DocumentRepository : BaseRepository, IDocumentRepository
    {
        public void DeleteDocument(DTO.Document doc)
        {
            var c = new OptimisticConnection(new SqlConnection(ConnectionString));

            try
            {
                using (var tx = c.OpenWithTransaction())
                {
                    if (doc.DocumentPages != null)
                    {
                        foreach (var p in doc.DocumentPages)
                        {
                            c.Execute("DeleteDocumentPage", p, transaction: tx);
                        }
                    }

                    c.Execute("DeleteDocument", doc, transaction: tx);

                    c.Commit();
                }
            }
            catch (OptimisticConcurrencyException ox)
            {
                throw new Exception("The information you're trying to delete was changed whilst you were in the process of deleting or did not exist. Concurrency Exception", ox);
            }
        }

        public void SaveDocument(DTO.Document doc)
        {
            var c = new OptimisticConnection(new SqlConnection(ConnectionString));

            try
            {
                using (var tx = c.OpenWithTransaction())
                {
                    c.Execute("SaveDocument", doc, transaction: tx);

                    if (doc.DocumentPages != null)
                    {
                        foreach (var p in doc.DocumentPages)
                        {
                            p.DocumentId = doc.Id;
                            c.Execute("SaveDocumentPage", p, transaction: tx);
                        }
                    }

                    c.Commit();
                }
            }
            catch (OptimisticConcurrencyException ox)
            {
                throw new Exception("The information you're trying to update was changed whilst you were in the process of updating. Concurrency Exception", ox);
            }
        }

      

        public IList<string> GetDocumentCategories()
        {
            var c = new SqlConnection(ConnectionString);

            IDocumentRepository repo = c.As<IDocumentRepository>();

            var result = repo.GetDocumentCategories();

            return result;
        }

        public IList<DTO.Document> GetDocumentsByFilenameAndCategory(string filename = null, string category = null)
        {
            var c = new SqlConnection(ConnectionString);

            IList<DTO.Document> docs = c.Query("GetDocumentsByFilenameAndCategory",
                new { Filename = filename, Category = category },
                Query.Returns(Some<DTO.Document>.Records)
                .ThenChildren(Some<DTO.DocumentPage>.Records));


            return docs;
        }

     

        public DTO.Document GetDocumentById(int Id)
        {
            
            var c = new SqlConnection(ConnectionString);

            var doc = c.Query("GetDocumentById",
                new { Id = Id },
                Query.Returns(Some<DTO.Document>.Records)
                .ThenChildren(Some<DTO.DocumentPage>.Records));

            return doc.SingleOrDefault();
        }
    }

}