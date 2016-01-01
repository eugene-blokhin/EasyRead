using System.Collections.Generic;

namespace EasyRead.BusinessServices.RemoteServices.TextService
{
    public class TextModel
    {
        public TextModel()
        {
            
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Body { get; set; }
        public List<string> Words { get; set; }
    }
}
