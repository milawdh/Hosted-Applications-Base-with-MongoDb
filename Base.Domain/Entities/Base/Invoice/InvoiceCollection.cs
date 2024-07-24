using Base.Domain.Audities;
using Base.Domain.Entities.Base.Users;
using Base.Domain.Enums.Base.Invoice;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Entities.Base.Invoice
{
    public class InvoiceCollection : FullAuditedEntity
    {
        public InvoiceCollection() : base() { }
        /// <summary>
        /// Invoice Desciption
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Invoice Unique Code
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Invoice Unique Display Code
        /// </summary>
        public string DCode { get; set; }

        /// <summary>
        /// Invoice Purchased by Customer Id
        /// </summary>
        public ObjectId CustomerId { get; set; }

        public CustomerCollection Customer { get; set; }

        /// <summary>
        /// Invoice Purchased Price
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Invoice Type
        /// </summary>
        public InvoiceType Type { get; set; }

        /// <summary>
        /// Invoice MetaDatas
        /// </summary>
        public object MetaData { get; set; }
    }
}
