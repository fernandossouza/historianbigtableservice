namespace historianbigtableservice.Model
{
    public class TagInput
    {
        public int thingId{get;set;}
        public string tag{get;set;}
        public string value{get;set;}
        public string group{get;set;}
        public long date{get;set;}
    }
}