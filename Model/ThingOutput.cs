using System.Collections.Generic;

namespace historianbigtableservice.Model
{
    public class ThingOutput
    {
        public int thingId{get;set;}
        public List<TagOutput> tags{get;set;}
    }
}