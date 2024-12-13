using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Database
{
    public class ObjectMetadata
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public long Size { get; set; }
        public DateTime UploadDate { get; set; }
        public ObjectContent Content { get; set; }
    }
}
