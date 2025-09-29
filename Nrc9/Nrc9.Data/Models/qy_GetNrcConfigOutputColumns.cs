using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nrc9.Data.Models
{
    public class qy_GetNrcConfigOutputColumns
    {
        public string ReadDirectory { get; set; }
		public string InputArchiveDirectory { get; set; }	
		public string OutputArchiveDirectory { get; set;}
        public string FilenameContainsString { get; set; }
        public int LineWhereDataStarts { get; set; }
        public string BaseWebApiUrl { get; set; }
        public string BulkInsertBaseWebApiUrl { get; set; }
        public string BulkInsertConnectionString { get; set; }
        public string BulkInsertDbName { get; set; }
        public string BulkInsertDbTableName { get; set; }
        public string ToEmailAddressList { get; set; }
        public string EmailBaseWebApiUrl { get; set; }
        public string EmailFromAddress { get; set; }

    }
}
