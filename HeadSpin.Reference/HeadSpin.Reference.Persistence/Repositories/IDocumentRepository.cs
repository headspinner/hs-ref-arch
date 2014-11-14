using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadSpin.Reference.Persistence.Repositories
{
    public interface IDocumentRepository
    {
        void SaveDocument(DTO.Document doc);

        void DeleteDocument(DTO.Document doc);

        IList<DTO.Document> GetDocumentsByFilenameAndCategory(string filename = null, string category = null);
 
        DTO.Document GetDocumentById(int Id);

        IList<string> GetDocumentCategories();

    }

}