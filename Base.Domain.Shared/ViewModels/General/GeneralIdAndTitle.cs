using Mapster;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Shared.ViewModels.General
{
    public class GeneralIdAndTitle
    {
        public string Id { get; set; }
        public string Title { get; set; }

    }

    public class GeneralIdAndTitle<TKey> : GeneralIdAndTitle
    {
        public TKey Id { get; set; }
        public string Title { get; set; }
    }
}
