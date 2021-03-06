﻿using Microsoft.WindowsAzure.Storage.Table;
using System.ComponentModel.DataAnnotations;

namespace Lisa.Bulder.WebApi
{
    public class SubscriptionEntity : TableEntity
    {
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
    }
}
